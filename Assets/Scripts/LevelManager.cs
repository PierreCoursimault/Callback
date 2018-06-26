using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject[] balls;
	private int mode=0;
	private int entities=0;
	private string modePPKey = "Mode" ;
	private string entitiesPPKey = "Entities" ;


	// Use this for initialization
	void Start () {
		mode = PlayerPrefs.GetInt (modePPKey);
		entities = PlayerPrefs.GetInt (entitiesPPKey);
		SetupGame ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SetupGame(){
		SetupMode ();
		SetupEntities ();
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
}
