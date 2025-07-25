using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public ZombieHand zomebieHand;
    public int zombieDamage;

    private void Start()
    {
        zomebieHand.damage = zombieDamage;
    }
}
