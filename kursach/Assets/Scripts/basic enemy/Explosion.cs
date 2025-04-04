using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float radius;
    public float damage;

    private Animator animator;
    private float animationLength;

    private LayerMask playerLayer;
    private void Start()
    {
        playerLayer = LayerMask.GetMask("Character");
        animator = GetComponent<Animator>();
        animationLength = animator.runtimeAnimatorController.animationClips[0].length;
        Debug.Log(animationLength + " animation length");
        Explode();
    }

    public void Explode()   
    {
        animator.Play(0);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, playerLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            DamageInterface player = hitCollider.gameObject.GetComponent<DamageInterface>();
            if (player != null)
            {
                player.Hit(damage);
            }
        }
        Destroy(gameObject, animationLength);
    }

    private void OnDrawGizmos()
    {
        // ”становите цвет Gizmos
        Gizmos.color = Color.red;

        // –исуем сферу, представл€ющую радиус взрыва
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
