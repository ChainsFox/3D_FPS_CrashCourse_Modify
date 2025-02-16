using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; set; }

    public List<GameObject> weaponSlots;

    public GameObject activeWeaponSlot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
    }

    private void Update()
    {
        /*We are looping all over these weapon in weaponSlots(list), and if one of these slot is the
         * active slot, then we are going to make the gameobject of this slot active, else false.
         */
        foreach(GameObject weaponSlot in weaponSlots) 
        {
            if(weaponSlot == activeWeaponSlot)
            {
                weaponSlot.SetActive(true);
            }
            else
            {
                weaponSlot.SetActive(false);
            }
        }
    }

    public void PickupWeapon(GameObject pickedupWeapon) //a dedicated method that is going to equip the weapon into the active weapon slot
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    public void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false); //take this weapon transform, and set its parent to be the active weapon slot, active weapon slot always get updated

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>(); //grab the Weapon script from this weapon, 

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z); //and then set the local position/rotation of this model to be according to the spawnPosition/Rotation 
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z); //that we have set in the editor

        weapon.isActiveWeapon = true;
    }
}
