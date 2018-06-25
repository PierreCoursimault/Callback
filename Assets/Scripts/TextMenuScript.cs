using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMenuScript : MonoBehaviour {

	public Sprite unlight;
	public Sprite highlight;
	private UnityEngine.UI.Image img;

	// Use this for initialization
	void Start () {
		img = GetComponent< UnityEngine.UI.Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void highlightText(){
		img.sprite = highlight;
	}

	public void unlightText(){
		img.sprite = unlight;
	}
}
