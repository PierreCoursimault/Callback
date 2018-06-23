using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBehavior : MonoBehaviour {

    public int team=-1;


    public void OnShot(int team, Color flag, float resetTime)
    {
        StartCoroutine(ProgrammReset(flag, resetTime));
        this.team = team;
        this.GetComponent<MeshRenderer>().material.color = flag;
    }

    private IEnumerator ProgrammReset(Color flag, float resetTime)
    {
        yield return new WaitForSeconds(resetTime);
        if (flag == this.GetComponent<MeshRenderer>().material.color)
        {
            Reset();
        }
    }

    public void Reset()
    {
        this.GetComponent<MeshRenderer>().material.color = Color.white;
        this.team = -1;
    }
}
