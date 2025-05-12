using UnityEngine;

public class ShurikenScript : MonoBehaviour
{
    public float flySpeed = 10f;
    public bool isFacingLeft;
    private Rigidbody2D RB;

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        float direction = isFacingLeft ? -1f : 1f;
        RB.linearVelocity = new Vector2(direction * flySpeed, 0f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("enemy"))
        {
            //Debug.Log("dead");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            //Debug.Log("dead");
            Destroy(gameObject);
        }
    }

}