﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SetupMenuManager : MonoBehaviour {
	public GameObject[] menulayer;
	public UnityEngine.UI.Text[] TexteMenu;
	public GameObject[] modeBtn;
	public GameObject[] levelBtn;
	public GameObject[] entitiesBtn;
	public string[] levelsName;
	public Color highlight;
	public Color unlight;

	public GameObject ControlPanel;
	public UnityEngine.UI.Text ctrlText;

	public GameObject dizaineObj;
	public GameObject uniteObj;

	public UnityEngine.UI.Text u_unit;
	public UnityEngine.UI.Text u_diz;
	public UnityEngine.UI.Text d_unit;
	public UnityEngine.UI.Text d_diz;


	private int mode=0;
	private int level=0;
	private int entities=0;
	private int current=0;
	private int diz = 0;
	private int unit = 0;
	private int scoreMode = 1;
	private string modePPKey = "Mode" ;
	private string lvlPPKey = "Level" ;
	private string entitiesPPKey = "Entities" ;
	private string scorePPKey = "Score" ;
	private string scoreModePPKey = "ScoreMode";
	private bool seeCtrl;
	private int couchemenu;
	private bool unitSelected = true;

	private int timer;
	private int timebeforeInput = 15;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < menulayer.Length ; i++) {
			menulayer [i].SetActive (false);
		}
		SelectMenu (0);
		LoadSetup ();
		SetControlPanel (false);

	}
	
	// Update is called once per frame
	void Update () {
		switch (couchemenu) {
		case 0: //Setup Mode, level, entities
			if(Input.GetButtonDown("Valider")){
				SelectMenu (1);
			}

			if(Input.GetKeyDown(KeyCode.Joystick1Button2)){
				SetControlPanel(!seeCtrl);
			}
			break;
		case 1: // Setup Score
			if (Input.GetButtonDown ("Valider")) {
				LaunchGame ();
			}

			if (timer > timebeforeInput) {
				if (Input.GetAxis ("Horizontal1") > 0.5 || Input.GetAxis ("Horizontal1") < -0.5) {

					if (unitSelected) {

						unitSelected = false;
						uniteObj.SetActive (false);
						dizaineObj.SetActive (true);

						timer = 0;

					} else {

						unitSelected = true;
						uniteObj.SetActive (true);
						dizaineObj.SetActive (false);

						timer = 0;

					}
				}
				if (Input.GetAxis ("Vertical1") > 0.5) {

					if (unitSelected) {

						if (unit != 9) {

							unit++;
							u_unit.text = unit.ToString ();
							d_unit.text = unit.ToString ();

							timer = 0;

						} else {

							if (diz != 9) {

								unit = 0;
								u_unit.text = unit.ToString ();
								d_unit.text = unit.ToString ();

								diz++;
								u_diz.text = diz.ToString ();
								d_diz.text = diz.ToString ();

								timer = 0;

							}

						}

					} else {

						if (diz != 9) {

							diz++;
							u_diz.text = diz.ToString ();
							d_diz.text = diz.ToString ();

							timer = 0;

						}

					}

				}

				if (Input.GetAxis ("Vertical1") < -0.5) {

					if (unitSelected) {

						if (unit != 0) {

							if (diz == 0 && unit == 1) {

								// ne rien faire

							} else {

								unit--;

								u_unit.text = unit.ToString ();
								d_unit.text = unit.ToString ();

								timer = 0;

							}

						} else {

							if (diz != 0) {

								unit = 9;
								u_unit.text = unit.ToString ();
								d_unit.text = unit.ToString ();

								diz--;
								u_diz.text = diz.ToString ();
								d_diz.text = diz.ToString ();

								timer = 0;

							}

						}

					} else {

						if (diz != 0) {

							diz--;
							u_diz.text = diz.ToString ();
							d_diz.text = diz.ToString ();

							timer = 0;

						}

					}

				}
			} else {
				timer++;
			}
			break;

		default:
			break;
		}
			
	}
		

	public void ValidMode(int mod){
		modeBtn [mode].GetComponent<TextMenuScript>().unlightText();
		mode = mod;
		current=0;
		modeBtn [mode].GetComponent<TextMenuScript> ().highlightText();
		SaveSetup ();
	}

	public void ValidLevel(int lvl){
		levelBtn [level].GetComponent<TextMenuScript> ().unlightText();
		level = lvl;
		current = 1;
		levelBtn [level].GetComponent<TextMenuScript> ().highlightText();
		SaveSetup ();
	}

	public void ValidEntities(int ent){
		entitiesBtn[entities].GetComponent<TextMenuScript> ().unlightText();
		entities = ent;
		current = 2;
		entitiesBtn[entities].GetComponent<TextMenuScript> ().highlightText();
		SaveSetup ();

	}

	void SaveSetup(){
		PlayerPrefs.SetInt(modePPKey,mode);
		PlayerPrefs.SetInt(lvlPPKey,level);
		PlayerPrefs.SetInt(entitiesPPKey,entities);
		PlayerPrefs.SetInt(scorePPKey, (diz*10+unit));
		PlayerPrefs.SetInt (scoreModePPKey, scoreMode);
	}

	void LoadSetup(){
		mode = PlayerPrefs.GetInt (modePPKey);
		level = PlayerPrefs.GetInt (lvlPPKey);
		entities = PlayerPrefs.GetInt (entitiesPPKey);
		LoadScore(PlayerPrefs.GetInt (scorePPKey));
		scoreMode = PlayerPrefs.GetInt (scoreModePPKey);
		Debug.Log (mode + ";" + level + ";" + entities);
		ValidMode (mode);
		ValidLevel (level);
		ValidEntities (entities);
		UnlightAll ();
		SelectMode ();


	}

	public void SelectMode(){
		TexteMenu [current].color = unlight;
		EventSystem.current.SetSelectedGameObject (modeBtn[mode]);
		TexteMenu [0].color = highlight;
	}
	public void SelectLevel(){
		TexteMenu [current].color = unlight;
		EventSystem.current.SetSelectedGameObject (levelBtn[level]);
		TexteMenu [1].color = highlight;
	}
	public void SelectEntities(){
		TexteMenu [current].color = unlight;
		EventSystem.current.SetSelectedGameObject (entitiesBtn[entities]);
		TexteMenu [2].color = highlight;
	}

	private void UnlightAll(){
		TexteMenu [0].color = unlight;
		TexteMenu [1].color = unlight;
		TexteMenu [2].color = unlight;
	}

	public void Cancel(){
		Debug.Log ("Current: " + current);
		switch (current) {
		case 0:
			Debug.Log ("Back To Title Screen");
			break;
		case 1:
			SelectMode ();
			break;
		case 2:
			SelectLevel ();
			break;		
		default:
			break;
		}
		current--;
	}

	public void LaunchGame(){
		SaveSetup ();
		//SceneManager.LoadScene (levelsName, LoadSceneMode.Single);
		SceneManager.LoadScene("TestLevel");
	}

	private void SetControlPanel(bool see){
		ControlPanel.SetActive (see);
		if (see) {
			ctrlText.text = "Hide Controls";
		} else {
			ctrlText.text = "Show Controls";
		}
		seeCtrl = see;
	}

	public void SelectMenu(int menu){
		menulayer [couchemenu].SetActive (false);
		couchemenu = menu;
		menulayer [couchemenu].SetActive (true);
	}

	private void LoadScore (int score){
		diz = score / 10;
		unit = score % 10;
		u_unit.text = unit.ToString ();
		d_unit.text = unit.ToString ();
		u_diz.text = diz.ToString ();
		d_diz.text = diz.ToString ();
	}
		
}
