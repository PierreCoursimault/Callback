﻿using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movespeed, dashSpeed, attackDuration, dashDuration, attackCooldown, dashCooldown, colorDuration, colorDurationOnDash;
	public int player,controller, team, health, maxDashes;
	[HideInInspector]
	public Color flag;
    [HideInInspector]
    public bool alive = true;
    private bool attackDown = false, dashing = false, leftTriggerDown = false;
	private Dash[] availableDashes;
    private int currentDash=0;
    [HideInInspector]
    public AttackScript personnalField, personnalBeam;
	private Rigidbody myRb;
	private Vector3 dir;
    private ScoreManager scoreManager;
	private Rotate rot;


    // Use this for initialization
    void Start () {
		rot = GetComponent<Rotate> ();
        flag = GetComponent<Renderer>().material.color;
		myRb = GetComponent<Rigidbody> ();
        availableDashes = new Dash[maxDashes];
        for(int i = 0; i < maxDashes; i++)
        {
            availableDashes[i] = new Dash(this);
        }
        scoreManager = Object.FindObjectOfType<ScoreManager>();
        personnalBeam.GetComponent<Renderer>().enabled = false;
    }


	// Update is called once per frame
	void FixedUpdate () {
		if (alive)
		{
			Move();
			Act();   
		}
	}

    private void Move()
    {
		dir = new Vector3(Input.GetAxis("Horizontal" + controller), Input.GetAxis("Vertical" + controller), 0);
        if (dashing)
        {
            myRb.AddForce(availableDashes[currentDash].GetForce(), ForceMode.VelocityChange);
        }
        else
        {
            myRb.AddForce(dir * movespeed, ForceMode.VelocityChange);
        }
    }

    private void Act()
	{
		Vector3 orientation = new Vector3(Input.GetAxis("HorizontalDirection" + controller), Input.GetAxis("VerticalDirection" + controller), 0);
		float triggers = Input.GetAxis("AttackOrDash" + controller);
        if(triggers > 0.1f && !leftTriggerDown)
        {
            leftTriggerDown = true;
            TryDash();
        }
        if (triggers < 0.1f)
        {
            leftTriggerDown = false;
        }
		if(Input.GetButtonDown("Dash" + controller))
        {
            TryDash();
        }
        if (orientation == Vector3.zero)
        {
			rot._isAiming = false;
            personnalField.GetComponent<Renderer>().enabled = true;
            personnalBeam.GetComponent<Renderer>().enabled = false;
			if (Input.GetButtonDown("Attack" + controller) || triggers < -0.1f)
            {
                StartCoroutine(ShootField());
            }
        }
        else
		{	rot._isAiming = true;
            personnalField.GetComponent<Renderer>().enabled = false;
            personnalBeam.GetComponent<Renderer>().enabled = true;
			if (Input.GetButtonDown("Attack" + controller) || triggers < -0.1f)
            {
                StartCoroutine(ShootBeam());
            }
        }
        
	}

	public IEnumerator ShootField()
	{
		if (!attackDown)
		{
			this.attackDown = true;
			personnalField.Activate(dashing);
			yield return new WaitForSeconds(attackDuration);
            personnalField.Desactivate();
            yield return new WaitForSeconds(attackCooldown);
			this.attackDown = false;
		}
	}

    public IEnumerator ShootBeam()
    {
        if (!attackDown)
        {
            this.attackDown = true;
            personnalBeam.gameObject.SetActive(true);
            personnalBeam.Activate(dashing);
            yield return new WaitForSeconds(attackDuration);
            personnalBeam.Desactivate();
            yield return new WaitForSeconds(attackCooldown);
            this.attackDown = false;
        }
    }

    private void TryDash()
	{

		if (availableDashes[currentDash].IsReady())
		{
            StartCoroutine(availableDashes[currentDash].Launch());
        }
	}

	private void OnCollisionEnter(Collision collider)
	{
		BallBehavior script = collider.collider.GetComponent<BallBehavior>();
		if (script!=null)
		{
			if (script.team != -1 && script.team!=this.team)
			{
                scoreManager.UpdateScore(script.team, 1);
                script.Reset();
                scoreManager.UpdateHealth(this, -1);
                scoreManager.TempFreeze();
            }
		}
	}

	public void Die()
	{
		alive = false;
		Destroy(this.gameObject);
	}

    private class Dash
    {
        protected Vector3 force=Vector3.zero;
        private float speed, dashDuration, dashCooldown;
        private int controller;
        private PlayerController pawn;
        private bool cooldown = false;
        protected Dash next=null;
        
        public Dash(PlayerController pawn)
        {
            this.pawn = pawn;
			this.controller = pawn.controller;
            this.speed = pawn.dashSpeed;
            this.dashDuration = pawn.dashDuration;
            this.dashCooldown = pawn.dashCooldown;
        }

        public bool IsReady()
        {
            return !cooldown;
        }

        public Vector3 GetForce()
        {
            return force;
        }

        public IEnumerator Launch()
        {
            this.cooldown = true;
			Vector3 newForce = new Vector3(Input.GetAxis("Horizontal" + controller), Input.GetAxis("Vertical" + controller), 0);
            newForce.Normalize();
            newForce *= speed;
            pawn.dashing = true;
            pawn.personnalBeam.dashing = true;
            pawn.personnalField.dashing = true;
            force = newForce;
            yield return new WaitForSeconds(dashDuration);
            UpdateIndex();
            pawn.dashing = false;
            pawn.personnalBeam.dashing = false;
            pawn.personnalField.dashing = false;
            force = Vector3.zero;
            yield return new WaitForSeconds(dashCooldown - dashDuration);
            this.cooldown = false;
        }

        private void UpdateIndex()
        {
            if (pawn.currentDash + 2 > pawn.maxDashes)
            {
                pawn.currentDash = 0;
            }
            else
            {
                pawn.currentDash++;
            }
        }

        override public string ToString()
        {
            return force.ToString();
        }
    }
}


