using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour, IKillable
{
    private Transform player_controller;
    public Status status_controller;
    private AnimationController animation_controller;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private SkinnedMeshRenderer skinnedMeshRenderer;

    [SerializeField] public GameObject MedKit;
    [SerializeField] public Slider healthBarBoss;
    [SerializeField] public Image fillSlider;
    [SerializeField] public Color maxHealthColor, minHealthColor;
    private Vector3 dir;
    Color origColor;
    float flashTime = 0.12f;

    private void Start(){
        player_controller = GameObject.FindWithTag("Jogador").transform;
        agent = GetComponent<NavMeshAgent>();
        status_controller = GetComponent<Status>();
        animation_controller = GetComponent<AnimationController>();
        rb = GetComponent<Rigidbody>();

        //vars to 'flash red'
        skinnedMeshRenderer = transform.GetChild(1).GetComponent<SkinnedMeshRenderer>();
        origColor = skinnedMeshRenderer.material.color;

        agent.speed = status_controller.speed;
        healthBarBoss.maxValue = status_controller.initialLife;
        UpdateInterface();
    }

    private void Update(){

        agent.SetDestination(player_controller.position);
        animation_controller.AnimationMovement(agent.velocity.magnitude);

        if (agent.hasPath == true){
        
            bool isCloseToPlayer = agent.remainingDistance <= agent.stoppingDistance;

            if (isCloseToPlayer){
                animation_controller.AnimationAttack(true);

                dir = player_controller.position - transform.position;
                Quaternion newRotation = Quaternion.LookRotation(dir);
                rb.MoveRotation(newRotation);
                animation_controller.AnimationMovement(dir.magnitude);
            }
            else{
                animation_controller.AnimationAttack(false);
            }
        }
    }

    void UpdateInterface() {
        healthBarBoss.value = status_controller.life;
        float healthPercentage = (float)status_controller.life / status_controller.initialLife;
        Color healthColor = Color.Lerp(minHealthColor, maxHealthColor, healthPercentage);
        fillSlider.color = healthColor; 
    }

    //****************** IKILLABLE INTERFACE ******************
    private void AttackPlayer(){
        int dmg = Random.Range(30, 40);
        player_controller.GetComponent<MovementPlayer>().TakeDmg(dmg);
    }

    public void TakeDmg(int dmg)
    {
        status_controller.life--;
        UpdateInterface();
        FlashRed();

        if (status_controller.life <= 0){
            Die();
        }
    }

    public void Die()
    {
        animation_controller.AnimationDie();
        AnimationDieGround();
        this.enabled = false;
        agent.enabled = false;
        Instantiate(MedKit, transform.position, Quaternion.identity);
        Destroy(gameObject, 2);
    }

    public void AnimationDieGround()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        rb.isKinematic = false;
        GetComponent<Collider>().enabled = false;
    }
    //******************************************************

    //****************** FLASH RED ******************
    public void FlashRed()
    {
        skinnedMeshRenderer.material.color = Color.red;
        Invoke("UnflashRed", flashTime);
    }

    public void UnflashRed()
    {
        skinnedMeshRenderer.material.color = origColor;
    }
    //******************************************************
}
