using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonSpawner : MonoBehaviour
{
    [SerializeField]
    private float mySpawnRate;

    [SerializeField]
    private GameObject myEnemySpawn;
    [SerializeField]
    private GameObject myTarget;
    private float myTimer;

    void Update()
    {
        if(myTimer < 0)
        {
            GameObject Spawn = Instantiate(myEnemySpawn, transform.position, Quaternion.identity);
            Spawn.GetComponent<NavMeshAgent>().SetDestination(myTarget.transform.position);
            myTimer = mySpawnRate;
        }
        myTimer -= Time.deltaTime;
    }
}
