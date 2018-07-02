using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamRotation : MonoBehaviour {

    private PlayerController player;


	// Use this for initialization
	void Start () {
        this.player = GetComponentInParent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.alive)
        {
            Vector3 direction = new Vector3(Input.GetAxis("HorizontalDirection" + player.player), Input.GetAxis("VerticalDirection" + player.player), 0);
            player.transform.rotation = Quaternion.LookRotation(player.transform.forward, direction);
        }
    }
}
