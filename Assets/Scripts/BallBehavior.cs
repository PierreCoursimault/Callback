using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {

    public Color team=Color.clear;


	// Use this for initialization
	void Start () {
    }

    // Update is called once per frame
    void Update () {
    }

    public void OnShot(Color team)
    {
        this.team = team;
        this.GetComponent<SpriteRenderer>().color = team;
    }
}
