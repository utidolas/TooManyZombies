using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20;
    [SerializeField] private float knockbackForce = 10;
    [SerializeField] private float knockbackDecelerationTime = 5;
    [SerializeField] private int bulletDmg = 1;

    private bool hasHitEnemy = false; // Flag to track if the bullet has hit an enemy
    private Rigidbody rb;

    void Start(){
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        rb.MovePosition(rb.position + transform.forward * bulletSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider_object){


        // If object collided is enemy, destroy self and -1 enemy's life
        if (!hasHitEnemy && collider_object.CompareTag("Inimigo")){

            // Get the EnemyController component of the collided enemy
            EnemyController enemyController = collider_object.GetComponent<EnemyController>();


            //If enemy is not Null
            if (enemyController != null && enemyController.status_controller.life > 0){

                //reduce enemy's life and debug
                enemyController.TakeDmg(bulletDmg);

                //Calculate knockback direction from bullet to enemy
                Vector3 knockbackDirection = collider_object.transform.position - transform.position;
                enemyController.Knockback(knockbackDirection.normalized, knockbackForce, knockbackDecelerationTime);

                hasHitEnemy = true;

            }
            Destroy(gameObject);
        }
        else if (!hasHitEnemy && collider_object.CompareTag("Chefe")){
            // Get the BossController component of the collided boss
            BossController enemyController = collider_object.GetComponent<BossController>();


            //If boss is not Null
            if (enemyController != null && enemyController.status_controller.life > 0){

                //reduce boss's life and debug
                enemyController.TakeDmg(bulletDmg);
                hasHitEnemy = true;

            }
            Destroy(gameObject);
        }
    }

}
