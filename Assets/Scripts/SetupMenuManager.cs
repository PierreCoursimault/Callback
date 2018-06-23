using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetupMenuManager : MonoBehaviour {

	public GameObject[] modeText;
	public GameObject[] levelText;
	public GameObject[] entitiesText;
	public string[] levels;
	private int mode;
	private int level;
	private int entities;
	private int current=0;


	// Use this for initialization
	void Start () {
		LoadSetup ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public void ValidMode(int mod){
		Debug.Log (mod);
		mode = mod;
		current++;
		EventSystem.current.SetSelectedGameObject (levelText[0]);
	}

	public void Cancel(){
		Debug.Log ("Current: " + current);
		switch (current) {
		case 0:
			Debug.Log ("Back To Title Screen");
			break;
		case 1:
			EventSystem.current.SetSelectedGameObject (modeText [mode]);
			break;
		case 2:
			EventSystem.current.SetSelectedGameObject (levelText [level]);
			break;		
		default:
			break;
		}
		current--;
	}

	public void ValidLevel(int lvl){
		Debug.Log (lvl);
		level = lvl;
		current++;
		EventSystem.current.SetSelectedGameObject (entitiesText[0]);
	}

	public void ValidEntities(int ent){
		entities = ent;
		SaveSetup ();
	}

	void SaveSetup(){
		Debug.Log (mode + ";" + level + ";" + entities);
	}

	void LoadSetup(){
		
	}
}
