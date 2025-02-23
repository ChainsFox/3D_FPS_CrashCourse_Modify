using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    public int ammoMount = 200;
    public AmmoType ammoType;

    public enum AmmoType
    { 
        RifleAmmo,
        PistolAmmo
    }


}
