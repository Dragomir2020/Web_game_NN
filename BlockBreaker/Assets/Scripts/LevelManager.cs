﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public GameObject Brick1;
	public GameObject Brick2;
	public GameObject Brick3;
	public GameObject NoBreak;
	public GameObject PlaySpace;

	public int level = 0;

	static LevelManager instance = null;

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		SceneManager.LoadScene (name);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}

	void Start(){
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
		}
	}

	void Update(){
		if (!GameObject.FindObjectOfType<Brick> () && SceneManager.GetActiveScene ().name.Equals ("Levels")) {
			//No more bricks left
			level++;
			LoadNextLevel ();
		}
		if (!GameObject.FindObjectOfType<Brick> () && SceneManager.GetActiveScene ().name.Equals ("Start Menu")) {
			//Have AI play on start menu
			Debug.LogWarning("Yes I am working");
			StartMenu();
		}
	}
				
	//Load levels
	private void LoadNextLevel () {
		//Destroy and create new game space each level
		Destroy(GameObject.FindGameObjectWithTag("PlaySpace"));
		Instantiate (PlaySpace, new Vector3 (8f, 3f, 0f), Quaternion.identity);
		//Load Bricks for each level here and add new levels
		if (level == 1) {
			Instantiate (Brick1, new Vector3 (7.5f, 8f, -5f), Quaternion.identity);
		}
		if (level == 2) {
			for (int i = 4; i < 12; i += 1) {
				Instantiate (Brick1, new Vector3 (i, 8f, -5f), Quaternion.identity);
				Instantiate (Brick2, new Vector3 (i, 8.5f, -5f), Quaternion.identity);
			}
		}
		if (level == 3) {
			for (int i = 4; i < 12; i += 1) {
				Instantiate (Brick1, new Vector3 (i, 8f, -5f), Quaternion.identity);
				Instantiate (Brick2, new Vector3 (i, 8.5f, -5f), Quaternion.identity);
				Instantiate (Brick3, new Vector3 (i, 9f, -5f), Quaternion.identity);
			}
		}
		if (level == 4) {
			this.LoadLevel ("Win Screen");
		}

	}

	private void StartMenu(){
		//Render Play Space
		Destroy(GameObject.FindGameObjectWithTag("PlaySpace"));
		Instantiate (PlaySpace, new Vector3 (8f, 3f, 0f), Quaternion.identity);
		//Destroy Image
		Destroy(GameObject.FindGameObjectWithTag("Background"));
		//Load TitleBlocks
		int count = 0;
		for(float j = 11.68f; j > 8; j -= 0.3203f){
			count++;
			for(float i = 0; i < 16; i++){
				if ((count == 1 || count == 12) || (i == 0 || i == 15)) {
					Instantiate (NoBreak, new Vector3 (i, j, -5f), Quaternion.identity);
				}
			}
		}
		//Load Menu Blocks
		count = 0;
		for(float j = 6f; j > 2; j -= 0.3203f){
			count++;
			for(float i = 5f; i < 11; i++){
				if ((count == 1 || count == 13) || (i == 5f || i == 10f)) {
					Instantiate (NoBreak, new Vector3 (i, j, -5f), Quaternion.identity);
				}
			}
		}
		//Load Breakable Blocks

		//Turn on basic AI for home screen
		GameObject.FindObjectOfType<Paddle>().autoPlay = true;
	}



}
