using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    //Mike code version: 
    //public List<Rigidbody> allParts = new List<Rigidbody>();
    //public GameObject normalVersion;

    //public void Shatter()
    //{
    //    Destroy(normalVersion);
    //    foreach (Rigidbody part in allParts)
    //    {
    //        part.isKinematic = false;
    //    }

    //}

    //Brackey code version:
    public GameObject destroyedVersion;

    public void Shatter2()
    {
        Instantiate(destroyedVersion, transform.position, transform.rotation);
        Destroy(gameObject);

    }


}
