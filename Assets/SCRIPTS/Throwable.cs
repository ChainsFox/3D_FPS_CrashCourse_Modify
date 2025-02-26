using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countDown;

    bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    { 
        Grenade
    
    }

    public ThrowableType throwableType;


    private void Start()
    {
        countDown = delay;

    }

    private void Update()
    {
        if(hasBeenThrown)
        {
            countDown -= Time.deltaTime;
            if(countDown <= 0f && !hasExploded)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    private void Explode()
    {
        GetThrownableEffect();

        Destroy(gameObject);
    }

    private void GetThrownableEffect()
    {
        switch(throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();
                break;  


        }
    }

    private void GrenadeEffect()
    {
        //Visual Effect
        GameObject explosionEffect = GlobalReferences.Instance.grenadeExplosionEffect;//get effect in global references 
        Instantiate(explosionEffect, transform.position, transform.rotation);

        //Physical Effect
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);//create a sphere shape that detect colliders(house, wall, enemy...), the size of the sphere is "damageRadis"
        foreach(Collider objectInRange in colliders)//loop over all coliders and apply effect...
        {
            Rigidbody rb = objectInRange.GetComponent<Rigidbody>();//get the rigidbody component 
            if(rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, damageRadius);
            }
        }

        //also apply damage to enemy over here
    }
}
