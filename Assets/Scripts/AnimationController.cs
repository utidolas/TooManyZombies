using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator anim;

    private void Awake(){
        anim = GetComponent<Animator>();
    }

    public void AnimationAttack(bool condition) {
        anim.SetBool("Attacking", condition);
    }

    public void AnimationMovement(float movementValue) {
        anim.SetFloat("Running", movementValue);
    }

    public void AnimationDie(){
        anim.SetTrigger("Die");
    }
}
