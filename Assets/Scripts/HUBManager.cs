using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUBManager : MonoBehaviour
{
    ////Singleton basic:
    //public static HUBManager Instance { get; set; }
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
            totalAmmoUI.text = $"{activeWeapon.magazineSize / activeWeapon.bulletsPerBurst}";

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
        }


    }
}
