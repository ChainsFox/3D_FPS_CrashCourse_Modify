using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUBManager : MonoBehaviour
{
    //Singleton basic:
    public static HUBManager Instance { get; set; }
    ////UI(old - remove in p11)
    //public TextMeshProUGUI ammoDisplay; 

    //NEW
    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalTypeUI;
    public TextMeshProUGUI lethalAmountUI;

    public Image tactialUI;
    public TextMeshProUGUI tacticalAmountUI;

    public Sprite emptySlot;

    public GameObject middledDot;


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

    private void Update()
    {
        Weapon activeWeapon = WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>(); //get the active weapon slot and find the active weapon
        Weapon unActiveWeapon = GetUnactiveWeapon().GetComponentInChildren<Weapon>(); //this method is going to get the unactive weapon

        if(activeWeapon)
        {
            magazineAmmoUI.text = $"{activeWeapon.bulletsLeft / activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";
            //totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}"; - old remove in p12

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel; //get the active weapon model and store it in model
            ammoTypeUI.sprite = GetAmmoSprite(model);

            activeWeaponUI.sprite = GetWeaponSprite(model);

            if(unActiveWeapon)
            {
                unActiveWeaponUI.sprite = GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }

        }
        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;

            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;

        }


    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch(model)
        {
            case Weapon.WeaponModel.M1911://Original code(Have been modified - remove instantiate) for better optimization: Instantiate(Resources.Load<GameObject>("Pistol1911_Weapon")).GetComponent<SpriteRenderer>().sprite; 
                return Resources.Load<GameObject>("Pistol1911_Weapon").GetComponent<SpriteRenderer>().sprite; 
            case Weapon.WeaponModel.M4:
                return Resources.Load<GameObject>("M4_Weapon").GetComponent<SpriteRenderer>().sprite;//instantiate the srpite from the resource folder(inside this folder will store the srpite...)

            default:
                return null;
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.M1911:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.M4:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;

            default:
                return null;
        }
    }

    private GameObject GetUnactiveWeapon()
    {
        foreach(GameObject weaponSlot in WeaponManager.Instance.weaponSlots)//loop over all of our slots
        {
            if(weaponSlot != WeaponManager.Instance.activeWeaponSlot)//if this weaponSlot is not the active weapon, then it is the unactive one
            {
                return weaponSlot;
            }
        }
        return null; //this will never happen but we need to return something
    }
}
