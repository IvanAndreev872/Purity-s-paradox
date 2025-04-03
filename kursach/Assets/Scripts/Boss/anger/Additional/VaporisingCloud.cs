using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaporisingCloud : MonoBehaviour
{
    private Animator animator;
    private float animationLength;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animationLength = animator.runtimeAnimatorController.animationClips[0].length;
        Animate();
    }

    void Animate()
    {
        animator.Play(0);
        Destroy(gameObject, animationLength);
    }
}
