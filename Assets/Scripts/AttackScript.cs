using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    private int team;
    private Color flag;
    public float speed;
    private float ballsColorDuration;
    private List<Collider2D> treatedColliders;

    // Use this for initialization
    void Start () {
        if (GetComponent<BeamRotation>() == null)
        {
            GetComponentInParent<PlayerController>().personnalField = this;
        }
        else
        {
            GetComponentInParent<PlayerController>().personnalBeam = this;
        }
        treatedColliders = new List<Collider2D>();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void OnActivation(int team, Color flag, float ballsColorDuration)
    {
        this.team = team;
        this.flag = flag;
        this.ballsColorDuration = ballsColorDuration;
        treatedColliders = new List<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        BallBehavior script = collider.GetComponent<BallBehavior>();
        if (script != null)
        {
            script.OnShot(this.team, this.flag, ballsColorDuration);
            Vector2 movement = collider.transform.position - this.transform.position;
            movement.Normalize();
            movement*= speed*20;
            collider.GetComponent<Rigidbody2D>().AddForce(movement);
            Debug.Log(collider.GetComponent<Rigidbody2D>().velocity);
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        if (!treatedColliders.Contains(collider))
        {
            OnTriggerEnter2D(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        treatedColliders.Remove(collider);
    }
}
