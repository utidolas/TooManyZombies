using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    private GameObject player_controller;

    [SerializeField] public GameObject Zombie;
    [SerializeField] public float SpawnerTime = 1.5f;
    [SerializeField] public LayerMask LayerZombie;

    float timerCount = 0;
    private float spawnDist = 3;
    private float playerDistForSpawn = 20;

    private void Start()
    {
        player_controller = GameObject.FindWithTag("Jogador");
    }

    void Update() {
        //Spawning zombies defined by distance 
        if (Vector3.Distance(transform.position, player_controller.transform.position) > playerDistForSpawn) {  
            
            timerCount += Time.deltaTime;

            if(timerCount >= SpawnerTime) { 

                StartCoroutine(SpawnNewZombie());
                timerCount = 0;
            
            }
        }
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, spawnDist);
    }

    IEnumerator SpawnNewZombie() {

        Vector3 spawnPos = RandomPos();
        Collider[] colliders = Physics.OverlapSphere(spawnPos, 1, LayerZombie);

        while(colliders.Length > 0 ) {
            spawnPos = RandomPos();
            colliders = Physics.OverlapSphere(spawnPos, 1, LayerZombie);
            yield return null;
        }

        Instantiate(Zombie, spawnPos, transform.rotation);
    }

    Vector3 RandomPos() { 
        Vector3 pos = Random.insideUnitSphere * spawnDist;
        pos += transform.position;
        pos.y = 0;

        return pos;
    }

}
