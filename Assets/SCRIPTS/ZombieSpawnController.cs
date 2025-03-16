using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using UnityEngine;
using UnityEngine.UIElements;


public class ZombieSpawnController : MonoBehaviour
{
    public int initialZombiesPerWave = 2;
    public int currentZombiesPerWave;

    public float spawnDelay = 0.5f; // Delay between spawining each zombie in a wave;
    
    public int currentWave = 0;
    public float waveCooldown = 10.0f; // Time in seconds between waves;
    
    public bool inCooldown;
    public float cooldownCounter = 0; // We only use this for testing and the UI;
    
    public List<Enemy> currentZombiesAlive;
    public GameObject zombiePrefab;

    private void Start()
    {
        currentZombiesPerWave = initialZombiesPerWave;

        StartNextWave();
    }

    private void StartNextWave()
    {
        currentZombiesAlive.Clear();

        currentWave++;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for(int i=0; i<currentZombiesPerWave; i++) //loop over the zombie, which mean we do i amount of time
        {
            //generate a random offset within a specifed range
            Vector3 spawnOffset = new Vector3(UnityEngine.Random.Range(-1, 1f), 0f, UnityEngine.Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;

            //instantate the zombie
            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);

            //get the enemy script
            Enemy enemyScript = zombie.GetComponent<Enemy>();

            //track this zombie
            currentZombiesAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        //get all dead zombies
        List<Enemy> zombiesToRemove = new List<Enemy>();

        foreach (Enemy zombie in currentZombiesAlive)
        {
            if(zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        //actually remove all dead zombies
        foreach (Enemy zombie in currentZombiesAlive)
        {
            currentZombiesAlive.Remove(zombie);
        }

        zombiesToRemove.Clear();

        //start cooldown if all zombies are dead
        if(currentZombiesAlive.Count == 0 && inCooldown == false)
        {
            //start cooldown for next wave
            StartCoroutine(WaveCooldown());
        }

        //run the cooldown counter
        if(inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;
        }


    }

    private IEnumerator WaveCooldown()
    {
        inCooldown = true;

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;

        //currentZombiesPerWave *= 2; //multiply the wave
        currentZombiesPerWave += 1; //add to the wave
        StartNextWave();

    }
}
