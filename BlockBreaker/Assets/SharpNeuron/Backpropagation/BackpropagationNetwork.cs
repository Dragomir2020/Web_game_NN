﻿namespace SharpNeuron.Backpropagation
{
    /// <summary>
    /// This class extends a <see cref="Network"/> and represents a Backpropagation neural network.
    /// </summary>

    public class BackpropagationNetwork : Network
    {
        private double meanSquaredError;
        private bool isValidMSE;

        /// <summary>
        /// Gets the value of mean squared error
        /// </summary>
        /// <value>
        /// Mean squared value of error in current training epoch
        /// </value>
        public double MeanSquaredError
        {
            get { return isValidMSE ? meanSquaredError : 0d; }
        }

        /// <summary>
        /// Creates a new Back Propagation Network, with the specified input and output layers. (You
        /// are required to connect all layers using appropriate synapses, before using the constructor.
        /// Any changes made to the structure of the network after its creation may lead to complete
        /// malfunctioning)
        /// </summary>
        /// <param name="inputLayer">
        /// The input layer
        /// </param>
        /// <param name="outputLayer">
        /// The output layer
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <c>inputLayer</c> or <c>outputLayer</c> is <c>null</c>
        /// </exception>
        public BackpropagationNetwork(ActivationLayer inputLayer, ActivationLayer outputLayer)
            : base(inputLayer, outputLayer, TrainingMethod.Supervised)
        {
            meanSquaredError = 0d;
            isValidMSE = false;
        }

        /// <summary>
        /// <para>
        /// Trains the network for the given training sample (Online training mode). Note that this
        /// method trains the sample only once irrespective of the values of <c>currentIteration</c>
        /// and <c>trainingEpochs</c>. Those arguments are just used to adjust training parameters
        /// which are dependent on training progress.
        /// </para>
        /// </summary>
        /// <param name="trainingSample">
        /// Training sample to use
        /// </param>
        /// <param name="currentIteration">
        /// Current training epoch
        /// </param>
        /// <param name="trainingEpochs">
        /// Number of training epochs
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// If <c>trainingSample</c> is <c>null</c>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// If <c>trainingEpochs</c> is not positive, or if <c>currentIteration</c> is negative or if
        /// <c>currentIteration</c> is less than <c>trainingEpochs</c>
        /// </exception>
        public override void Learn(TrainingSample trainingSample, int currentIteration, int trainingEpochs)
        {
            meanSquaredError = 0d;
            isValidMSE = true;
            base.Learn(trainingSample, currentIteration, trainingEpochs);
        }

        /// <summary>
        /// Invokes BeginEpochEvent
        /// </summary>
        /// <param name="currentIteration">
        /// Current training iteration
        /// </param>
        /// <param name="trainingSet">
        /// Training set which is about to be trained
        /// </param>
        protected override void OnBeginEpoch(int currentIteration, TrainingSet trainingSet)
        {
            meanSquaredError = 0d;
            isValidMSE = false;
            base.OnBeginEpoch(currentIteration, trainingSet);
        }

        /// <summary>
        /// Invokes EndEpochEvent
        /// </summary>
        /// <param name="currentIteration">
        /// Current training iteration
        /// </param>
        /// <param name="trainingSet">
        /// Training set which got trained successfully this epoch
        /// </param>
        protected override void OnEndEpoch(int currentIteration, TrainingSet trainingSet)
        {
            meanSquaredError /= trainingSet.TrainingSampleCount;
            isValidMSE = true;
            base.OnEndEpoch(currentIteration, trainingSet);
        }

        /// <summary>
        /// A protected helper function used to train single learning sample
        /// </summary>
        /// <param name="trainingSample">
        /// Training sample to use
        /// </param>
        /// <param name="currentIteration">
        /// Current training epoch (Assumed to be positive and less than <c>trainingEpochs</c>)
        /// </param>
        /// <param name="trainingEpochs">
        /// Number of training epochs (Assumed to be positive)
        /// </param>
        protected override void LearnSample(TrainingSample trainingSample, int currentIteration, int trainingEpochs)
        {
            // No validation here
            int layerCount = layers.Count;

            // Set input vector
            inputLayer.SetInput(trainingSample.InputVector);

            for (int i = 0; i < layerCount; i++)
            {
                layers[i].Run();
            }

            // Set Errors
            meanSquaredError += (outputLayer as ActivationLayer).SetErrors(trainingSample.OutputVector);

            // Backpropagate errors
            for (int i = layerCount; i > 0;)
            {
                var layer = layers[--i] as ActivationLayer;
                if (layer != null)
                {
                    layer.EvaluateErrors();
                }
            }

            // Optimize synapse weights and neuron bias values
            for (int i = 0; i < layerCount; i++)
            {
                layers[i].Learn(currentIteration, trainingEpochs);
            }
        }
    }
}