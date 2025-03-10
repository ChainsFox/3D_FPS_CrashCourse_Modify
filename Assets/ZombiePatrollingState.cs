using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrollingState : StateMachineBehaviour
{

    float timer;
    public float patrollingTime = 10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpped = 2f;

    List<Transform> waypointsList = new List<Transform>();

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //initialization
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = animator.GetComponent<NavMeshAgent>();

        agent.speed = patrolSpped;
        timer = 0;

        //get all waypoints and move to first waypoint
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoints"); //parent object that contain all the waypoints
        foreach(Transform t in waypointCluster.transform) //add them to the list
        {
            waypointsList.Add(t);
        }

        Vector3 nextPosition = waypointsList[Random.Range(0, waypointsList.Count)].position;//look for a random waypoint inside this list and move to that position
        agent.SetDestination(nextPosition);

    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (SoundManager.Instance.zombieChannel.isPlaying == false) //check if the sound is already played, and if is not then we play it(so that it wont overlap sounds)
        {
            SoundManager.Instance.zombieChannel.clip = SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }


        //check if agent arrived at the waypoint, move on to next waypoint
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(waypointsList[Random.Range(0, waypointsList.Count)].position);
        }

        //transition into idle state
        timer += Time.deltaTime;
        if(timer > patrollingTime)
        {
            animator.SetBool("isPatrolling", false);
        }

        //transition into the chase state
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);//the distance between the player and the enemy
        if(distanceFromPlayer < detectionArea)//the player is inside the detection area radius
        {
            animator.SetBool("isChasing", true);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //stop the agent
        agent.SetDestination(agent.transform.position);
        SoundManager.Instance.zombieChannel.Stop();

    }
}
