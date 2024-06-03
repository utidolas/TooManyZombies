using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource myAudioSource;
    public static AudioSource soundInstance;

    // Awake runs before Start()
    void Awake(){
        myAudioSource = GetComponent<AudioSource>();
        soundInstance = myAudioSource;
    }

    // Update is called once per frame
    void Update(){
        
    }
}
