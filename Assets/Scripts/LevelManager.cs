using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject[] balls;
	public GameObject player1;
	public GameObject player2;
	public GameObject player3;
	public GameObject player4;

	private int mode=0;
	private string modePPKey = "Mode" ;
	private int entities=0;
	private string entitiesPPKey = "Entities" ;
	private int score;
	private string scorePPKey = "Score";
	private int scoreMode;
	private string scoreModePPKey = "ScoreMode";
	private string playerConfig;
	private string[] playerConfTab;
	private int nbplayer;
	private string playerConfPPKey = "PlayerConfig";


	// Use this for initialization
	void Start () {
		LoadPlayerPrefs ();
		SetupGame ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetupGame(){
		SetupMode ();
		SetupEntities ();
		SetupPlayer ();
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
		//scoreMode : 1=score a atteindre,0=vie
		gameObject.GetComponent<ScoreManager> ().SetScoreMode (scoreMode, score,nbplayer);
	}

	void LoadPlayerPrefs(){
		mode = PlayerPrefs.GetInt (modePPKey);
		entities = PlayerPrefs.GetInt (entitiesPPKey);
		score = PlayerPrefs.GetInt (scorePPKey);
		scoreMode = PlayerPrefs.GetInt (scoreModePPKey);
		playerConfig = PlayerPrefs.GetString (playerConfPPKey);
	}

	void SetupPlayer (){
		playerConfTab = playerConfig.Split(";"[0]);
		nbplayer = int.Parse(playerConfTab [0]);
		if (nbplayer > 0) {
			player1.SetActive (true);
			player1.GetComponent<PlayerController> ().controller =int.Parse(playerConfTab [1]);
		} else {
			player1.SetActive (false);
		}
		if (nbplayer > 1) {
			player2.SetActive (true);
			player2.GetComponent<PlayerController> ().controller =int.Parse(playerConfTab [2]);
		} else {
			player2.SetActive (false);
		}

		if (nbplayer > 2) {
			player3.SetActive (true);
			player3.GetComponent<PlayerController> ().controller = int.Parse(playerConfTab [3]);
		} else {
			player3.SetActive (false);
		}

		if (nbplayer > 3) {
			player4.SetActive (true);
			player4.GetComponent<PlayerController> ().controller = int.Parse(playerConfTab [4]);
		} else {
			player4.SetActive (false);
		}


	}
}
