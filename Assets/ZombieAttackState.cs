using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    Transform player;

    public float stopAttackingDistance = 2.5f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialization
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false) //check if the sound is already played, and if is not then we play it(so that it wont overlap sounds)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieAttack;
        }

        LookAtPlayer();

        //check if the agent should stop attacking

        float distaceFromPlayer = Vector3.Distance(player.position, animator.transform.position);

        //check if the agent should stop chasing 
        if (distaceFromPlayer > stopAttackingDistance)
        {
            animator.SetBool("isAttacking", false);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);//?
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        SoundManager.Instance.zombieChannel.Stop();
    }
}
