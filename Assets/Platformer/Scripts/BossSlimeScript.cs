using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSlimeScript : MonoBehaviour
{
    public int Health;
    public float Damage;
    public float Speed;
    public float JumpPower;
    public float JumpCooldown;
    public float Stunned;

    public GameObject target;
    public SpriteRenderer SR;
    public Rigidbody2D RB;
    public Animator Anim;
    public bool OnGround = true;
    private float jumpTimer;
    private float range = 30;

    public GameObject Slime;
    public float AttackCD;
    private float AttackTimer;

    void Start()
    {
        RB.gravityScale = 2f;
        jumpTimer = JumpCooldown;
        AttackTimer = AttackCD;

    }

    private void Update()
    {
        if (Stunned > 0)
        {
            Stunned -= Time.deltaTime;
            if (Stunned > 0)
                SR.color = Color.red;
            else
                SR.color = Color.white;
            return;
        }

        float distanceToPlayer = UnityEngine.Vector2.Distance(target.transform.position, transform.position);
        if (distanceToPlayer <= range)
        {
            UnityEngine.Vector2 vel;
            vel = (target.transform.position - transform.position).normalized;
            RB.linearVelocity = new UnityEngine.Vector2(vel.x * Speed, RB.linearVelocity.y);

            if (OnGround)
            {
                AttackTimer -= Time.deltaTime;


                if (AttackTimer <= 0f)
                {
                    Vector2 spawnPos = transform.position + new Vector3(1f * transform.localScale.x, 0, 0); // in front of boss
                    GameObject newSlime = Instantiate(Slime, spawnPos, Quaternion.identity);
                    newSlime.GetComponent<SlimeScript>().target = target;

                    AttackTimer = AttackCD;
                }
                jumpTimer -= Time.deltaTime;

                if (jumpTimer <= 0f)
                {
                    jump();
                    Anim.Play("BossJump");
                    jumpTimer = JumpCooldown; // reset timer AFTER jumping
                }

            }
            
        }


    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
            OnGround = true;

        if (other.gameObject.tag == "Player")
        {
            target.GetComponent<PlayerScript>().Health -= Damage;
        }
        if (other.gameObject.tag == "Destroy")
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float dmg)
    {
        Health -= (int)dmg;
        Stunned = 0.1f;
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
        {
            Destroy(gameObject);
            SceneManager.LoadScene("You Win");
        }
    }

    public void jump()
    {
        RB.linearVelocity = new UnityEngine.Vector2(RB.linearVelocity.x, JumpPower);
        OnGround = false;
    }


}
