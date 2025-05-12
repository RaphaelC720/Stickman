using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;

public class PlayerScript : MonoBehaviour
{
    public SpriteRenderer SR;
    public Sprite FaceLeft;
    public Sprite FaceRight;
    public Sprite JumpSprite;
    public Rigidbody2D RB;
    public Collider2D Coll;
    public ParticleSystem PS;
    public AudioSource AS;
    public static PlayerScript PyS;
    public weaponbase ActiveWeaponWB;
    public Animator ActiveWeaponAnim;
    public float Health = 100;
    public float Maxhealth = 100;
    public float Speed;
    public float JumpPower;
    public float Gravity;
    
    public bool OnGround = true;
    public bool FacingLeft = false;
    public List<GameObject> ObjectsTouching = new List<GameObject>();
    public LayerMask GroundLayer;
    public bool touchingWall = false; 
    public LayerMask WallLayer;
    public float wallCheckDistance = 0.1f;


    public float Stunned = 0;
    
    public AudioClip JumpSFX;

    public GameObject Wheel;
    public WheelScript wheelScript;
    public GameObject Shuriken;
    
    void Start()
    {
        RB.gravityScale = Gravity;
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = FaceRight;
        Speed = 10;
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
        touchingWall = IsTouchingWall();

        Vector2 vel = RB.linearVelocity;

        bool moveRight = Input.GetKey(KeyCode.D);
        bool moveLeft = Input.GetKey(KeyCode.A);

        bool blockedRight = touchingWall && moveRight && !OnGround && !FacingLeft;
        bool blockedLeft = touchingWall && moveLeft && !OnGround && FacingLeft;

        bool didJump = false;

        if (!blockedRight && !blockedLeft || didJump)
        {
            if (moveRight)
            {
                vel.x = Speed;
                transform.localScale = new Vector3(1, 1, 1);
                FacingLeft = false;
            }
            else if (moveLeft)
            {
                vel.x = -Speed;
                transform.localScale = new Vector3(-1, 1, 1);
                FacingLeft = true;
            }
            else
            {
                vel.x = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        {
            vel.y = JumpPower;
            didJump = true;
            PS.Emit(5);
        }



        RB.linearVelocity = vel;
        if (transform.position.y < -20)
        {
            SceneManager.LoadScene("You Lose");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            OpenWheel();
        }

        if(Health <= 0)
            die();

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            UseWeapon();
        }    
    }

    public void die()
    {
        if(Health <= 0)
        {
            SceneManager.LoadScene("Platformer");
        }
    }


    public void OpenWheel()
    {
        Wheel.SetActive(!Wheel.activeSelf);
    }
    public bool CanJump()
    {
        return OnGround;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if((GroundLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            foreach (ContactPoint2D contact in other.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    OnGround = true;
                    if (!ObjectsTouching.Contains(other.gameObject))
                        ObjectsTouching.Add(other.gameObject);
                }
                else if (Mathf.Abs(contact.normal.x) > 0.5f)
                {
                    touchingWall = true;
                }
            }
        }


        if (other.gameObject.tag == "enemy")
        {
            Stunned = 0.75f;
            Vector2 vel = new Vector2(5, 5);
            if (other.transform.position.x > transform.position.x)
                vel.x *= -2;
            RB.AddForce(vel,ForceMode2D.Impulse);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        ObjectsTouching.Remove(other.gameObject);
        OnGround = ObjectsTouching.Count > 0;

        if ((GroundLayer.value & (1 << other.gameObject.layer)) > 0)
        {
            touchingWall = false;

        }

    }
    public bool IsTouchingWall()
    {
        Vector2 direction = FacingLeft ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallCheckDistance, WallLayer);
        return hit.collider != null;
    }


    public void UseWeapon()
    {
        if(ActiveWeaponWB.WeaponName == ("Sword"))
        {
            Speed = 15;
            ActiveWeaponAnim.Play("SwordSwing");
            Speed = 10;
        }
        if(ActiveWeaponWB.WeaponName == ("Axe"))
        {
            ActiveWeaponAnim.Play("AxeSwing");
        }
        if(ActiveWeaponWB.WeaponName == "Shuriken")
        {
            ActiveWeaponAnim = Shuriken.GetComponent<Animator>();
            Shuriken.SetActive(true);
            Vector3 offset = FacingLeft ? Vector3.left : Vector3.right;
            GameObject obj = Instantiate(Shuriken, transform.position + offset * 0.5f, Quaternion.identity);
            obj.GetComponent<ShurikenScript>().isFacingLeft = FacingLeft;
        }
        if(ActiveWeaponWB.WeaponName == "Scythe")
        {
            ActiveWeaponAnim.Play("ScytheSwing");
        }
    }
}
