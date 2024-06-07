using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private float timeNextGen = 0;
    public float timeBetweenGen = 40;
    [SerializeField] public GameObject bossPrefab;
    [SerializeField] public Transform[] bossGenPositions;

    private InterfaceController interface_controller;
    private Transform player_controller;

    private void Start(){
        timeNextGen = timeBetweenGen;
        interface_controller = GameObject.FindAnyObjectByType<InterfaceController>();
        player_controller = GameObject.FindWithTag("Jogador").transform;
    }

    private void Update(){
        if (Time.timeSinceLevelLoad > timeNextGen){
            Vector3 instancePos = computeFurthestPos();
            Instantiate(bossPrefab, instancePos, Quaternion.identity);
            interface_controller.BossWarning();
            timeNextGen = Time.timeSinceLevelLoad + timeBetweenGen;
        }
    }

    Vector3 computeFurthestPos(){
        Vector3 furthestPos = Vector3.zero;
        float greatestDistance = 0;

        foreach(Transform pos in bossGenPositions){
            float distToPlayer = Vector3.Distance(pos.position, player_controller.position);
            if(distToPlayer > greatestDistance)
            {
                greatestDistance = distToPlayer;
                furthestPos = pos.position;
            }

        }

        return furthestPos;
    }
}
