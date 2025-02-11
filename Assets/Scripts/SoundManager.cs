using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    public AudioSource shootingSound_M1911;
    public AudioSource reloadSound_M1911;
    public AudioSource emptyMagazineSound_M1911;

    private void Awake()
    {
        if (Instance != null && Instance != this) //if the instance is not null and is not this one then we destroy it
        {
            Destroy(gameObject); //we only want 1 instance of this global references all of the time
        }
        else
        {
            Instance = this; //this is also the usual way we create a singleton
        }
    }
}
