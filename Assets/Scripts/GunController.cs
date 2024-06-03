using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] public GameObject Bullet;
    [SerializeField] public GameObject BulletPos;
    [SerializeField] public AudioClip BulletSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1")){
            Instantiate(Bullet, BulletPos.transform.position, BulletPos.transform.rotation);
            AudioController.soundInstance.PlayOneShot(BulletSound);
        }
    }
}
