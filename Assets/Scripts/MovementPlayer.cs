using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour, IKillable, ICurable
{
    private Rigidbody rb;
    private PlayerFeatures playerFeatures_controller;
    public Status status_controller;
    private AnimationController animation_controller;

    [SerializeField] public LayerMask GroundMask;
    [SerializeField] public GameObject GameOverText;
    [SerializeField] public InterfaceController interface_controller;
    [SerializeField] public AudioClip DmgSound;

    Vector3 dir;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        rb = GetComponent<Rigidbody>();
        status_controller = GetComponent<Status>();
        playerFeatures_controller = GetComponent<PlayerFeatures>();
        animation_controller = GetComponent<AnimationController>();
    }


    // Update is called once per frame
    void Update()
    {
        //Adds "Raw" not to have the snap movement after releasing a key
        float x_mov = Input.GetAxisRaw("Horizontal");
        float z_mov = Input.GetAxisRaw("Vertical");

        dir = new Vector3(x_mov, 0, z_mov);

        //Changing animation of running
        animation_controller.AnimationMovement(dir.magnitude);


        //Moving player
        rb.velocity = status_controller.speed * dir.normalized;

    }

    private void FixedUpdate()
    {
        playerFeatures_controller.PlayerRotation(GroundMask);
    }

    // ******************* DAMAGE AND DIE *******************
    public void TakeDmg(int dmg) {

        status_controller.life -= dmg;
        interface_controller.UpdateSliderPlayerHealth();
        AudioController.soundInstance.PlayOneShot(DmgSound);

        if (status_controller.life <= 0) {
            Die();
        }
    }

    public void Die() {
        //Pausing game & Setting GameOverText to true
        Time.timeScale = 0;
        interface_controller.GameOver();
    }

    public void AnimationDieGround(){
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        GetComponent<Collider>().enabled = false;
    }
    // *********************************************************
    public void Rotate(Vector3 crosshairPos)
    {
        //New rotation by the ray and setting it 
        Quaternion newRotation = Quaternion.LookRotation(crosshairPos);
        rb.MoveRotation(newRotation);
    }

    // ******************* ItemsRelated *******************
    public void HealLife(int HealQnt) { 
        status_controller.life += HealQnt;

        if (status_controller.life > status_controller.initialLife) { 
            status_controller.life = status_controller.initialLife;
        }

        interface_controller.UpdateSliderPlayerHealth();
    }
    //*********************************************************
}
