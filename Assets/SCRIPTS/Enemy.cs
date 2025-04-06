using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int HP = 100;
    private Animator animator;
    private NavMeshAgent navAgent;

    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;

        if(HP<=0)
        {
            int randomValue = Random.Range(0, 2);// 0 or 1
            if(randomValue == 1)
            {
                animator.SetTrigger("DIE1");
            }
            else
            {
                animator.SetTrigger("DIE2");
            }
            isDead = true;
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);

        }
        else
        {
            animator.SetTrigger("DAMAGE");
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }


    private void OnDrawGizmos() //visual indicator for different state of the zombie
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2.5f); //attacking & stop attacking

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 100f); //detection(start chasing)

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 101f); //stop chasing
    }

}
