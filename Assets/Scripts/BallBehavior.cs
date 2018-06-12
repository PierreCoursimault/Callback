using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {

    public int team=-1;


    // Use this for initialization
    void Start () {
    }

    // Update is called once per frame
    void Update () {
    }

    public void OnShot(int team, Color flag, float resetTime)
    {
        StartCoroutine(Reset(resetTime));
        this.team = team;
        this.GetComponent<SpriteRenderer>().color = flag;
    }

    public IEnumerator Reset(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        this.GetComponent<SpriteRenderer>().color = Color.clear;
        this.team = -1;
    }
}
