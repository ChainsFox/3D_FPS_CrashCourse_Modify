using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ZombieIdleState : StateMachineBehaviour
{
    float timer;
    public float idleTime = 0;

    Transform player;

    public float detectionAreaRadius = 18f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //transition into the patrolling state
        timer += Time.deltaTime;
        if(timer > idleTime)
        {
            animator.SetBool("isPatrolling", true);
        }

        //transition into the chase state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);//the distance between the player and the enemy
        if(distanceFromPlayer < detectionAreaRadius)//the player is inside the detection area radius
        {
            animator.SetBool("isChasing", true);
        }

    }

}
