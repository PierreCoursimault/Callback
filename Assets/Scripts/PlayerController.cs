using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float movespeed, dashSpeed, attackDuration, dashDuration, attackCooldown, dashCooldown, colorDuration, colorDurationOnDash;
	public int player;
	public int team;
	[HideInInspector]
	public Color flag;
	private bool alive = true, attackDown = false, dashDown = false;
	private Vector2 currentDash = new Vector2(0, 0);
	public AttackScript personnalField, personnalBeam;
	private Rigidbody myRb;
	private Vector3 dir;

	// Use this for initialization
	void Start () {
		//flag = GetComponent<SpriteRenderer>().color;
		myRb = GetComponent<Rigidbody> ();
	}


	// Update is called once per frame
	void FixedUpdate () {
		if (alive)
		{
			Move();
			Act();   
		}
	}

	private void Act()
	{
		float triggers = Input.GetAxis("AttackOrDash" + player);
		if (Input.GetButtonDown("Attack" + player) || triggers < -0.1f)
		{
			StartCoroutine(Shoot());
		}
		if (Input.GetButtonDown("Dash" + player) || triggers > 0.1f)
		{
			StartCoroutine(Dash());
		}
	}

	public IEnumerator Shoot()
	{
		if (!attackDown)
		{
			this.attackDown = true;
			personnalBeam.gameObject.SetActive(true);
			personnalField.gameObject.SetActive(true);
			if (currentDash.x == 0 && currentDash.y == 0)
			{
				personnalBeam.OnActivation(team, flag, colorDuration);
				personnalField.OnActivation(team, flag, colorDuration);
			}
			else
			{
				personnalBeam.OnActivation(team, flag, colorDurationOnDash);
				personnalField.OnActivation(team, flag, colorDurationOnDash);
			}
			yield return new WaitForSeconds(attackDuration);
			personnalBeam.gameObject.SetActive(false);
			personnalField.gameObject.SetActive(false);
			yield return new WaitForSeconds(attackCooldown);
			this.attackDown = false;
		}
	}

	private void Move()
	{
		dir = new Vector3(Input.GetAxis("Horizontal"+ player), Input.GetAxis("Vertical"+ player),0);
		if (currentDash.x == 0 && currentDash.y == 0)
		{
			myRb.AddForce (dir * movespeed, ForceMode.VelocityChange );



			/*Vector2 movement = new Vector2();
            movement.x = Input.GetAxis("Horizontal" + player);
            movement.y = Input.GetAxis("Vertical" + player);
            movement.Normalize();
            movement *= speed;
            transform.Translate(movement);*/
		}
		else
		{
			transform.Translate(currentDash);
		}

	}

	private IEnumerator Dash()
	{
		if (!dashDown)
		{
			this.dashDown = true;
			currentDash.Normalize();
			currentDash *= dashSpeed;
			yield return new WaitForSeconds(dashDuration);
			currentDash = new Vector2(0, 0);
			yield return new WaitForSeconds(dashCooldown-dashDuration);
			this.dashDown = false;

		}
	}

	private void OnCollisionEnter2D(Collision2D collider)
	{
		BallBehavior script = collider.collider.GetComponent<BallBehavior>();
		if (script!=null)
		{
			if (script.team != -1 && script.team!=this.team)
			{
				this.Die();
			}

		}
	}

	public void Die()
	{
		alive = false;
		Debug.Log("Player " + player + " died !");
		Destroy(this.gameObject);
	}
}
