using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMelee : MonoBehaviour
{
    public float radius;
    public float damage;
    public float attackDelay;
    public Transform player;
    public float slashDuration;
    public GameObject swingPrefab;

    protected float attackTime;
    private DamageInterface attack_player;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        attackTime = Time.time;
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Character").transform;
        }
        attack_player = player.gameObject.GetComponent<DamageInterface>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (Time.time > attackTime + attackDelay)
        {
            Attack();
        }
    }

    protected void Attack()
    {
        if (Vector2.Distance(transform.position, player.position) < radius)
        {
            attack_player.Hit(damage);
            StartCoroutine(MakeSlash());
        }
        attackTime = Time.time;
    }

    protected virtual IEnumerator MakeSlash()
    {
        GameObject swing = Instantiate(swingPrefab, transform.position, Quaternion.identity, transform);
        swing.transform.localScale = Vector2.one * radius * 2;
        Vector2 parentScale = swing.transform.parent.lossyScale;
        swing.transform.localScale = new Vector2(
            swing.transform.localScale.x / parentScale.x,
            swing.transform.localScale.y / parentScale.y
        );

        yield return new WaitForSeconds(slashDuration);
        Destroy(swing);
    }
}
