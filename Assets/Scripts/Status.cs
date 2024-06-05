using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
    public int initialLife = 100;
    public int life;
    public float speed = 13;

    private void Awake(){

        life = initialLife;

    }
}
