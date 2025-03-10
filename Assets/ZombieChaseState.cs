using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieChaseState : StateMachineBehaviour
{

    NavMeshAgent agent;
    Transform player;

    public float chaseSpeed = 6f;
    public float stopChasingDistance = 21f;
    public float attackingDistance = 2.5f;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialization
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = chaseSpeed;
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false) //check if the sound is already played, and if is not then we play it(so that it wont overlap sounds)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieChase;
            
        }

        agent.SetDestination(player.position);//move zombie to the position of the player - chase the player
        animator.transform.LookAt(player);//face the player

        float distaceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        //check if the agent should stop chasing 
        if(distaceFromPlayer > stopChasingDistance)
        {
            animator.SetBool("isChasing", false);
        }

        //check if the agent should attack
        if (distaceFromPlayer < attackingDistance)
        {
            animator.SetBool("isAttacking", true);
        }


    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);
        SoundManager.Instance.zombieChannel.Stop();
    }
}
