using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{

    [SerializeField] public GameObject Player;
    Vector3 camDist;

    // Start is called before the first frame update
    void Start(){
        camDist = transform.position - Player.transform.position;
    }

    // Update is called once per frame
    void Update(){
        transform.position = Player.transform.position + camDist;
    }
}
