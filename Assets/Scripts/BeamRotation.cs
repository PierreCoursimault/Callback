using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamRotation : MonoBehaviour {

    private int player;


	// Use this for initialization
	void Start () {
        this.player = GetComponentInParent<PlayerController>().player;
    }

    // Update is called once per frame
    void Update () {
        Vector3 direction = new Vector3(Input.GetAxis("HorizontalDirection" + player), Input.GetAxis("VerticalDirection" + player), 0);
        transform.rotation = Quaternion.LookRotation(transform.forward, direction);
    }
}
