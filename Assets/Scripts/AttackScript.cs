using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    private int team;
    private Color flag;
    public bool dashing;
    public float ballSpeed;
    public float ballsColorDuration;
    private List<Collider> treatedColliders;


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
        treatedColliders = new List<Collider>();
        gameObject.SetActive(false);
    }

    public void OnActivation(int team, Color flag, bool dashing)
    {
        this.team = team;
        this.flag = flag;
        this.dashing = dashing;
        treatedColliders = new List<Collider>();
    }

    private void OnTriggerEnter(Collider collider)
    {
        BallBehavior script = collider.GetComponent<BallBehavior>();
        if (script != null)
        {
            script.OnShot(this.team, this.flag, ballsColorDuration);
            Vector3 movement = collider.transform.position - this.transform.position;
            movement.Normalize();
            if (dashing)
            {
                movement *= 2*ballSpeed;
            }
            else
            {
                movement *= ballSpeed;
            }
            collider.GetComponent<Rigidbody>().AddForce(movement);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if (!treatedColliders.Contains(collider))
        {
            OnTriggerEnter(collider);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        treatedColliders.Remove(collider);
    }
}
