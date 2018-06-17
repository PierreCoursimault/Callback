using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxspeed,AForce,DForce, dashSpeed, attackDuration, dashDuration, attackCooldown, dashCooldown, colorDuration, colorDurationOnDash;
    public int player;
    public int team;
    [HideInInspector]
    public Color flag;
    private bool alive = true, attackDown = false, dashDown = false;
    private Vector2 currentDash = new Vector2(0, 0);
    public AttackScript personnalField, personnalBeam;
	private Vector2 v,zero;
	private Rigidbody2D myRb2D;


    // Use this for initialization
    void Start () {
        flag = GetComponent<SpriteRenderer>().color;
		myRb2D = GetComponent<Rigidbody2D> ();
    }

	void Update () {
		v = new Vector2(Input.GetAxis("Horizontal"+ player), Input.GetAxis("Vertical"+ player));
		zero = new Vector2(0,0);
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

        if (currentDash.x == 0 && currentDash.y == 0)
        {
			if(v == zero){
				myRb2D.velocity = myRb2D.velocity * DForce;
			}
			else{
				myRb2D.velocity = Vector2.ClampMagnitude (myRb2D.velocity, maxspeed);
				myRb2D.AddForce(v.normalized * AForce);
			}

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
			currentDash = v;
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
