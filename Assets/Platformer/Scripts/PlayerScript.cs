using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    public float Stunned = 0;
    
    public AudioClip JumpSFX;

    public GameObject Wheel;
    public WheelScript wheelScript;
    
    void Start()
    {
        RB.gravityScale = Gravity;
        SR = GetComponent<SpriteRenderer>();
        SR.sprite = FaceRight;
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
        
        Vector2 vel = RB.linearVelocity;

        if (Input.GetKey(KeyCode.D))
        { 
            vel.x = Speed;
            transform.localScale = new Vector3(1, 1, 1); 
        }
        else if (Input.GetKey(KeyCode.A))
        { 
            vel.x = -Speed;
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {  
            vel.x = 0;
        }

        if (Input.GetKeyDown(KeyCode.Space) && CanJump())
        { 
            vel.y = JumpPower;
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
        return ObjectsTouching.Count > 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {  
        OnGround = true;
        ObjectsTouching.Add(other.gameObject);

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
        //If I stop touching something solid, mark me as not being on the ground
        OnGround = false;
        ObjectsTouching.Remove(other.gameObject);
    }

    public void UseWeapon()
    {
        if(ActiveWeaponWB.WeaponName == ("Sword"))
        {
            ActiveWeaponAnim.Play("SwordSwing");
        }
        if(ActiveWeaponWB.WeaponName == ("Axe"))
        {
            ActiveWeaponAnim.Play("AxeSwing");
        }
    }
}
