using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float speed, attackDuration, attackCooldown;
    public int player;
    [HideInInspector]
    public Color team;
    private bool alive = true, cooldown = false;
    public AttackScript personnalField, personnalBeam;


    // Use this for initialization
    void Start () {
        team = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (alive)
        {
            Move();
            if (Input.GetButtonDown("Attack" + player))
            {
                StartCoroutine(Shoot());
            }
        }
    }

    public IEnumerator Shoot()
    {
        if (!cooldown)
        {
            personnalBeam.gameObject.SetActive(true);
            personnalBeam.OnActivation();
            personnalField.gameObject.SetActive(true);
            personnalField.OnActivation();
            yield return new WaitForSeconds(attackDuration);
            this.cooldown = true;
            personnalBeam.gameObject.SetActive(false);
            personnalField.gameObject.SetActive(false);
            yield return new WaitForSeconds(attackCooldown);
            this.cooldown = false;
        }
    }

    private void Move()
    {
        Vector2 movement = new Vector2();
        movement.x = Input.GetAxis("Horizontal" + player);
        movement.y = Input.GetAxis("Vertical" + player);
        movement.Normalize();
        movement *= speed;
        transform.Translate(movement);
    }
    
    private void OnCollisionEnter2D(Collision2D collider)
    {
        BallBehavior script = collider.collider.GetComponent<BallBehavior>();
        if (script!=null)
        {
            if (script.team != Color.clear && script.team!=this.team)
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
