using System.Numerics;
using UnityEngine;

public class SlimeScript : MonoBehaviour
{
    public int Health;
    public float Damage;
    public float Speed; 
    public float JumpPower;
    public float JumpCooldown = 2f;
    public float Stunned = 0f;

    public GameObject target;
    public SpriteRenderer SR;
    public Rigidbody2D RB;
    public Animator Anim;
    public bool OnGround = true;
    private float jumpTimer;
    private float range = 10;

    //I might have to add a jump thing, because I want the slime to jump while going towards the player. Also maybe add a little more gravity because it goes down pretty slow. 

    void Start()
    {
        RB.gravityScale = 2f;
        jumpTimer = JumpCooldown;
    }

    void Update()
    {
        if (Stunned > 0)
        {
            Stunned -= Time.deltaTime;
            if(Stunned > 0)
                SR.color = Color.red;
            else 
                SR.color = Color.white;
            return;
        }

        float distanceToPlayer = UnityEngine.Vector2.Distance(target.transform.position, transform.position);
        if(distanceToPlayer <= range)
        {
            UnityEngine.Vector2 vel;
            vel = (target.transform.position - transform.position).normalized;
            RB.linearVelocity = new UnityEngine.Vector2(vel.x*Speed, RB.linearVelocity.y);

            if (OnGround)
            {
                jumpTimer -= Time.deltaTime;

                if (jumpTimer <= 0f)
                {
                jump();
                Anim.Play("slimeJump");
                jumpTimer = JumpCooldown; // reset timer AFTER jumping
                }
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground")) 
            OnGround = true;

        if(other.gameObject.tag == "Player")
        {
            target.GetComponent<PlayerScript>().Health -= 10;
        }
    }
    
    public void TakeDamage(float dmg)
    {
        Health -= (int)dmg;
        Stunned = 1f;
        UnityEngine.Vector2 vel = new UnityEngine.Vector2(3, 3);
        if (target.transform.position.x > transform.position.x)
            vel.x *= -2;
        RB.AddForce(vel, ForceMode2D.Impulse);

        if (Health <= 0)
            die();
    }


    public void die()
    {
        if (Health <= 0)
            Destroy(gameObject);
    }

    public void jump()
    {        
        RB.linearVelocity = new UnityEngine.Vector2(RB.linearVelocity.x, JumpPower);
        OnGround = false;
    }
}

