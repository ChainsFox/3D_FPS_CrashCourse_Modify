using System;
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

        if(Input.GetKeyDown(KeyCode.Alpha1)) //alpha1 = number 1 key
        {
            SwitchActiveSlot(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchActiveSlot(1);
        }
    }

    public void PickupWeapon(GameObject pickedupWeapon) //a dedicated method that is going to equip the weapon into the active weapon slot
    {
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    public void AddWeaponIntoActiveSlot(GameObject pickedupWeapon)
    {
        DropCurrentWeapon(pickedupWeapon);

        pickedupWeapon.transform.SetParent(activeWeaponSlot.transform, false); //take this weapon transform, and set its parent to be the active weapon slot, active weapon slot always get updated

        Weapon weapon = pickedupWeapon.GetComponent<Weapon>(); //grab the Weapon script from this weapon, 

        pickedupWeapon.transform.localPosition = new Vector3(weapon.spawnPosition.x, weapon.spawnPosition.y, weapon.spawnPosition.z); //and then set the local position/rotation of this model to be according to the spawnPosition/Rotation 
        pickedupWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z); //that we have set in the editor

        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    internal void PickupAmmo(string name)
    {
        print($"pick up ammo: {name}");
    }

    private void DropCurrentWeapon(GameObject pickedupWeapon)
    {
        if(activeWeaponSlot.transform.childCount > 0) //check if the active weapon slot already has something inside of it
        {
            var weaponToDrop = activeWeaponSlot.transform.GetChild(0).gameObject; //get the weapon that we are using and saving it in this variable

            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false; //disable it from being the active weapon
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickedupWeapon.transform.parent); //set parent of the new weapon that we pick up to be the same parent of the old weapon 
            weaponToDrop.transform.localPosition = pickedupWeapon.transform.localPosition; //also set the same position/rotation = to the old weapon
            weaponToDrop.transform.localRotation = pickedupWeapon.transform.localRotation; //=> basically we are switching values between the 2
        }
    }

    public void SwitchActiveSlot(int slotNumber) //this function will receive a number to switch between weapon slot
    {
        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if(activeWeaponSlot.transform.childCount > 0)
        {
            Weapon newWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            newWeapon.isActiveWeapon = true;
        }

    }

}
