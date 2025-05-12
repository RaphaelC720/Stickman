using UnityEngine;

[CreateAssetMenu(fileName = "weaponbase", menuName = "Scriptable Objects/weaponbase")]
public class weaponbase : ScriptableObject
{
    public GameObject Weapon;
    public GameObject Projectile;
    public string WeaponName;
    public float WeaponDamage;
    public float WeaponCooldown;
    public Vector3 equipPosition;
    public Vector3 equipRotation;
    public Vector3 equipScale;
}
