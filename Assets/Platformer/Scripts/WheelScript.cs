using UnityEngine;

public class WheelScript : MonoBehaviour
{
    public GameObject player;
    public PlayerScript PS;
    public GameObject currentWeapon; 
    public weaponbase selectedWeapon; 

    public weaponbase[] availableWeapons; // Array of available weapons


    void Start()
    {
        PS = player.GetComponent<PlayerScript>();
    }
    void Update()
    {
        //Debug.Log(currentWeapon);
    }
    public void SelectWeaponFromButton(ScriptableObject obj)
    {
        SelectAndEquipWeapon(obj as weaponbase);
    }

    public void EquipSelectedWeapon()
    {
        EquipWeapon(selectedWeapon, PS.FacingLeft);
    }

    public void SelectAndEquipWeapon(weaponbase newWeapon)
    {
        selectedWeapon = newWeapon;
        EquipSelectedWeapon();
    }

    public void EquipWeapon(weaponbase weaponData, bool isFacingLeft)
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon); // Remove the previous weapon
        }

        Debug.Log("hi: " + weaponData);
        // Instantiate the selected weapon prefab
        currentWeapon = Instantiate(weaponData.Weapon);
        currentWeapon.transform.SetParent(player.transform, false);
        PS.ActiveWeaponWB = weaponData;
        PS.ActiveWeaponAnim = currentWeapon.GetComponent<Animator>();
        currentWeapon.SetActive(true);


        // Flip position and rotation based on facing direction
        if (isFacingLeft)
        {
            currentWeapon.transform.localPosition = new Vector3(
                -weaponData.equipPosition.x, 
                weaponData.equipPosition.y, 
                weaponData.equipPosition.z
            );

            currentWeapon.transform.localRotation = Quaternion.Euler(
                weaponData.equipRotation.x,
                -weaponData.equipRotation.y, // Flip Y rotation
                -weaponData.equipRotation.z  // Flip Z rotation
            );

            currentWeapon.transform.localScale = new Vector3(
                -weaponData.equipScale.x, // Flip X scale
                weaponData.equipScale.y,
                weaponData.equipScale.z
            );
        }
        else
        {
            currentWeapon.transform.localPosition = weaponData.equipPosition;
            currentWeapon.transform.localRotation = Quaternion.Euler(weaponData.equipRotation);
            currentWeapon.transform.localScale = weaponData.equipScale;
        }
    }
}
