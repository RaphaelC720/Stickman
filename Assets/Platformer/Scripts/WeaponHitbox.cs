using UnityEngine;

public class WeaponHitbox : MonoBehaviour
{
    public float damage;

    void Update()
    {
    
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("enemy"))
        {
            SlimeScript slime = other.GetComponent<SlimeScript>();
            if (slime != null)
            {
                slime.TakeDamage(damage);
            }

            BossSlimeScript boss = other.GetComponent<BossSlimeScript>(); 
            
            if (boss != null)
            {
                boss.TakeDamage(damage);
            }
        }
    }
}
