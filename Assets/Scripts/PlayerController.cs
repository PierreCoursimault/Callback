using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movespeed, dashSpeed, attackDuration, dashDuration, attackCooldown, dashCooldown, colorDuration, colorDurationOnDash;
	public int player, team, health, maxDashes;
	[HideInInspector]
	public Color flag;
    private bool alive = true, attackDown = false, dashing = false;
	private Dash[] availableDashes;
    private int currentDash=0;
    [HideInInspector]
    public AttackScript personnalField, personnalBeam;
	private Rigidbody myRb;
	private Vector3 dir;
    private ScoreManager scoreManager;


    // Use this for initialization
    void Start () {
		flag = GetComponent<MeshRenderer>().material.color;
		myRb = GetComponent<Rigidbody> ();
        availableDashes = new Dash[maxDashes];
        for(int i = 0; i < maxDashes; i++)
        {
            availableDashes[i] = new Dash(this);
        }
        scoreManager = Object.FindObjectOfType<ScoreManager>();

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
        dir = new Vector3(Input.GetAxis("Horizontal" + player), Input.GetAxis("Vertical" + player), 0);
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
		float triggers = Input.GetAxis("AttackOrDash" + player);
		if (Input.GetButtonDown("Attack" + player) || triggers < -0.1f)
		{
            Vector3 orientation = new Vector3(Input.GetAxis("HorizontalDirection" + player), Input.GetAxis("VerticalDirection" + player), 0);
            if (orientation == Vector3.zero)
            {
			    StartCoroutine(ShootField());
            }
            else
            {
                StartCoroutine(ShootBeam());
            }
        }
        if (Input.GetButtonDown("Dash" + player) || triggers > 0.1f)
		{
            TryDash();
		}
	}

	public IEnumerator ShootField()
	{
		if (!attackDown)
		{
			this.attackDown = true;
			personnalField.gameObject.SetActive(true);
			personnalField.OnActivation(team, flag, dashing);
			yield return new WaitForSeconds(attackDuration);
			personnalField.gameObject.SetActive(false);
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
            personnalBeam.OnActivation(team, flag, dashing);
            yield return new WaitForSeconds(attackDuration);
            personnalBeam.gameObject.SetActive(false);
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
			}

		}
	}

	public void Die()
	{
		alive = false;
		Debug.Log("Player " + player + " died !");
		Destroy(this.gameObject);
	}

    private class Dash
    {
        protected Vector3 force=Vector3.zero;
        private float speed, dashDuration, dashCooldown;
        private int player;
        private PlayerController pawn;
        private bool cooldown = false;
        protected Dash next=null;
        
        public Dash(PlayerController pawn)
        {
            this.pawn = pawn;
            this.player = pawn.player;
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
            Vector3 newForce = new Vector3(Input.GetAxis("Horizontal" + player), Input.GetAxis("Vertical" + player), 0);
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


