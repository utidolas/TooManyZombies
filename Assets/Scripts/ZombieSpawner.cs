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
    private float playerDistForSpawn = 15;
    private int limitZombiesAlive = 10;
    private int currentZombiesAlive = 2;
    private float nextDifficultLevel = 30;
    private float counterDifficultLevel;

    private void Start()
    {
        player_controller = GameObject.FindWithTag("Jogador");
        counterDifficultLevel = nextDifficultLevel;
        for(int i = 0; i < limitZombiesAlive / 2; i++){
            StartCoroutine(SpawnNewZombie());
        }
    }

    void Update() {
        //Spawning zombies defined by distance 
        bool canGenZombie = Vector3.Distance(transform.position, player_controller.transform.position) > playerDistForSpawn;

        if (canGenZombie == true && currentZombiesAlive < limitZombiesAlive) {  
            
            timerCount += Time.deltaTime;

            if(timerCount >= SpawnerTime) { 

                StartCoroutine(SpawnNewZombie());
                timerCount = 0;
            
            }
        }

        if(Time.timeSinceLevelLoad > counterDifficultLevel){
            limitZombiesAlive += 2;
            counterDifficultLevel = Time.timeSinceLevelLoad + nextDifficultLevel;
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

        EnemyController zombie = Instantiate(Zombie, spawnPos, transform.rotation).GetComponent<EnemyController>();
        zombie.zombiespawner_controller = this;
        limitZombiesAlive++;
    }

    Vector3 RandomPos() { 
        Vector3 pos = Random.insideUnitSphere * spawnDist;
        pos += transform.position;
        pos.y = 0;

        return pos;
    }

    public void DecreaseQntZombieAlive(){
        currentZombiesAlive--;
    }

}
