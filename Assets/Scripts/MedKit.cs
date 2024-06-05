using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    private int healQnt = 15;
    private int healDestructionTime = 5;

    private void Start(){
        Destroy(gameObject, healDestructionTime);
    }

    private void OnTriggerEnter(Collider colliderObject){

        //Destroy medkit if player collide
        if (colliderObject.tag == "Jogador"){
            colliderObject.GetComponent<MovementPlayer>().HealLife(healQnt);
            Destroy(gameObject);
        } 
    }
}
