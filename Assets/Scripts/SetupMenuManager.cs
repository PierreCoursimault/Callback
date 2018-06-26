using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SetupMenuManager : MonoBehaviour {

	public UnityEngine.UI.Text[] TexteMenu;
	public GameObject[] modeBtn;
	public GameObject[] levelBtn;
	public GameObject[] entitiesBtn;
	public string[] levelsName;
	public Color highlight;
	public Color unlight;
	private int mode=0;
	private int level=0;
	private int entities=0;
	private int current=0;
	private string modePPKey = "Mode" ;
	private string lvlPPKey = "Level" ;
	private string entitiesPPKey = "Entities" ;

	// Use this for initialization
	void Start () {
		LoadSetup ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Valider")){
			LaunchGame();
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
	}

	void LoadSetup(){
		mode = PlayerPrefs.GetInt (modePPKey);
		level = PlayerPrefs.GetInt (lvlPPKey);
		entities = PlayerPrefs.GetInt (entitiesPPKey);
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
}
