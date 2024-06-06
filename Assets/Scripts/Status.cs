using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    [SerializeField] public int initialLife = 100;
    [SerializeField] public int life;
    [SerializeField] public float speed = 13;

    private void Awake(){

        life = initialLife;

    }
}
