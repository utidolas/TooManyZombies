using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyController : MonoBehaviour, IKillable
{
    private Rigidbody rb;
    private Animator anim;
    private MovementPlayer player_controller;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private AnimationController animation_controller;
    public Status status_controller;
    private InterfaceController interface_controller;

    [SerializeField] public GameObject Player;
    [SerializeField] public GameObject MedKit;
    [SerializeField] public GameObject Particle_BloodZombie;
    public ZombieSpawner zombiespawner_controller;
    [SerializeField] public AudioClip EnemyDieSound;
    [SerializeField] public int Zumbi_dmg = 20;
    Color origColor;
    float flashTime = 0.12f;

    private Vector3 randomPos;
    private Vector3 dir;
    private float wanderCounter;
    private float timeBetweenRandomPos = 4;
    private float percentageMedkit = 0.1f;


    // Start is called before the first frame update
    [System.Obsolete]
    void Start(){

        //Spawining randoms zombies
        int spawn_zombie_type = Random.Range(1, transform.childCount);
        transform.GetChild(spawn_zombie_type).gameObject.SetActive(true);

        // Getting renderer component of the generated zombie && storing original color
        skinnedMeshRenderer = transform.GetChild(spawn_zombie_type).GetComponent<SkinnedMeshRenderer>();
        origColor = skinnedMeshRenderer.material.color;

        //Getting object with tag "Player" and attaching to "Player" var
        Player = GameObject.FindWithTag("Jogador");

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        animation_controller = GetComponent<AnimationController>();
        player_controller = Player.GetComponent<MovementPlayer>();
        status_controller = GetComponent<Status>();
        interface_controller = GameObject.FindObjectOfType(typeof(InterfaceController)) as InterfaceController;
    }

    private void FixedUpdate()
    {

        float dist = Vector3.Distance(transform.position, Player.transform.position);

        // Rotating to player
        Quaternion newRotation = Quaternion.LookRotation(dir);
        rb.MoveRotation(newRotation);
        animation_controller.AnimationMovement(dir.magnitude);

        //Zombie movement and attacking's animation
        if (dist > 15) {
            Wander();
        }
        else if (dist > 3) {
            dir = Player.transform.position - transform.position;
            rb.MovePosition(rb.position + (status_controller.speed * Time.deltaTime * dir.normalized));
            animation_controller.AnimationAttack(false);
        }
        else {
            dir = Player.transform.position - transform.position;
            animation_controller.AnimationAttack(true);
        }
    }

    // ****************** REDUCE LIFE AND DIE ******************
    public void TakeDmg(int dmg){
        status_controller.life-= dmg;
        FlashRed();

        if (status_controller.life <= 0) {
            Die();
        }
    }

    public void BloodParticle(Vector3 position, Quaternion rotation){
        Instantiate(Particle_BloodZombie, position, rotation);
    }

    public void Die(){
        GetComponent<Collider>().enabled = false;
        this.enabled = false;
        animation_controller.AnimationDie();
        AnimationDieGround(); 
        AudioController.soundInstance.PlayOneShot(EnemyDieSound);
        CheckMedkitGen(percentageMedkit);
        interface_controller.UpdateQuantityEnemiesKilled();
        zombiespawner_controller.DecreaseQntZombieAlive();
        Destroy(gameObject, 2);
    }

    public void AnimationDieGround()
    {
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
    }
    //******************************************************

    // ****************** MEDKIT ******************
    public void CheckMedkitGen(float percentage) {
        if(Random.value <= percentage){
            Instantiate(MedKit, transform.position, Quaternion.identity);
        }
    }
    //******************************************************

    // ****************** FLASH WHEN HIT ******************
    public void FlashRed(){
        skinnedMeshRenderer.material.color = Color.red;
        Invoke("UnflashRed", flashTime);
    }

    public void UnflashRed() {
        skinnedMeshRenderer.material.color = origColor;
    }

    // ******************************************************

    // ****************** KNOCKBACK ******************
    public void Knockback(Vector3 direction, float force, float decelerationTime) {

        Debug.Log("Knockbacked! Force: " + force);
        rb.AddForce(direction * force, ForceMode.Impulse);
        StartCoroutine(DecelerateKnockback(decelerationTime));


    }

    private IEnumerator DecelerateKnockback(float decelerationTime) {

        // Calculate deceleration per frame
        Vector3 decelerationPerFrame = rb.velocity / decelerationTime;

        // Keep applying deceleration until velocity magnitude is very small
        while (rb.velocity.magnitude > 0.1f)
        {
            rb.velocity -= decelerationPerFrame * Time.deltaTime;
            yield return null;
        }

        // Ensure velocity is zero
        rb.velocity = Vector3.zero;

    }
    //************************************

    // ****************** ATTACK PLAYER ******************
    void AttackPlayer() {

        player_controller.TakeDmg(Zumbi_dmg);

    }
    // ******************************************************

    // ****************** WANDER ******************
    void Wander() {
        wanderCounter -= Time.deltaTime;

        if (wanderCounter <= 0){
            randomPos = RandomPos();
            wanderCounter += timeBetweenRandomPos + Random.Range(-1f, 1f);
        }

        bool isCloseEnough = Vector3.Distance(transform.position, randomPos) <= 0.05;
        if (isCloseEnough == false) { 

            dir = randomPos - transform.position;
            Quaternion newRotation = Quaternion.LookRotation(dir);
            rb.MoveRotation(newRotation);
            rb.MovePosition(rb.position + (status_controller.speed * Time.deltaTime * dir.normalized));
            animation_controller.AnimationAttack(false);
            
        }

    }

    Vector3 RandomPos() {
        Vector3 pos = Random.insideUnitSphere * 10;
        pos += transform.position;
        pos.y = transform.position.y; // Cancelling y's pos 

        return pos;
    }
    // ******************************************************
}
