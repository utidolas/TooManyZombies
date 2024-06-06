using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    private float timeNextGen = 0;
    public float timeBetweenGen = 40;
    [SerializeField] public GameObject bossPrefab;

    private void Start()
    {
        timeNextGen = timeBetweenGen;
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > timeNextGen){
            Instantiate(bossPrefab, transform.position, Quaternion.identity);
            timeNextGen = Time.timeSinceLevelLoad + timeBetweenGen;
        }
    }
}
