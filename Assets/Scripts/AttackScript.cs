using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour {

    private int team;
    private Color flag;
    public bool dashing;
    public float ballForce, ballDashForce;
    public float playerForce, playerDashForce;
    public float ballsColorDuration;
    private List<Collider> treatedColliders;


    // Use this for initialization
    void Start () {
        PlayerController player=GetComponentInParent<PlayerController>();
        if (GetComponent<BeamRotation>() == null)
        {
            player.personnalField = this;
        }
        else
        {
            player.personnalBeam = this;
        }
        treatedColliders = new List<Collider>();
        this.team = player.team;
        this.flag = player.flag;
        GetComponent<Collider>().enabled = false;
    }

    public void Activate(bool dashing)
    {
        this.dashing = dashing;
        GetComponent<Collider>().enabled=true;
    }

    public void Desactivate()
    {
        GetComponent<Collider>().enabled = false;
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
                movement *= ballDashForce;
            }
            else
            {
                movement *= ballForce;
            }
            collider.GetComponent<Rigidbody>().AddForce(movement);
        }
        if (collider.tag=="Player")
        {
            Vector3 movement = collider.transform.position - this.transform.position;
            movement.Normalize();
            if (dashing)
            {
                movement *= playerDashForce;
            }
            else
            {
                movement *= playerForce;
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
