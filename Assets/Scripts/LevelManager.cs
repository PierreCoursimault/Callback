using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject[] balls;
	private int mode=0;
	private string modePPKey = "Mode" ;
	private int entities=0;
	private string entitiesPPKey = "Entities" ;
	private int score;
	private string scorePPKey = "Score";
	private int scoreMode;
	private string scoreModePPKey = "ScoreMode";


	// Use this for initialization
	void Start () {
		mode = PlayerPrefs.GetInt (modePPKey);
		entities = PlayerPrefs.GetInt (entitiesPPKey);
		score = PlayerPrefs.GetInt (scorePPKey);
		scoreMode = PlayerPrefs.GetInt (scoreModePPKey);
		SetupGame ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetupGame(){
		SetupMode ();
		SetupEntities ();
		SetupScore ();
	}

	void SetupMode(){
		
	}

	void SetupEntities(){
		Debug.Log ("Entities level : " + entities);
		for (int i = 0; i < balls.Length ; i++) {
			if (i <= entities) {
				balls [i].SetActive (true);
			} else {
				balls [i].SetActive (false);
			}
				
		}
	}

	void SetupScore(){
		//gameObject.GetComponent<ScoreManager> ().SetScoreMode (scoreMode, score);
	}
}
