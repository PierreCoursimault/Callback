using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {

    public int team=-1;


    public void OnShot(int team, Color flag, float resetTime)
    {
        StartCoroutine(ProgrammReset(resetTime));
        this.team = team;
        this.GetComponent<MeshRenderer>().material.color = flag;
    }

    private IEnumerator ProgrammReset(float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        Reset();
    }

    public void Reset()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.white;
        this.team = -1;
    }




}
