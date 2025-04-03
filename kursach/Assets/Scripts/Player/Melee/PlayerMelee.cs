using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public abstract class PlayerMelee : MonoBehaviour
{
    public float radius;
    public float damage;
    public float slashDuration;
    public float attackDelay;
    private float attackTime;
    public GameObject swingPrefab;

    private LayerMask enemyLayer;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        attackTime = Time.time;
        enemyLayer = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (CheckAttack() && Time.time > attackDelay + attackTime)
        {
            attackTime = Time.time;
            Attack();
            StartCoroutine(MakeSlash());
        }
    }

    protected virtual void Attack()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);
        foreach (Collider2D hitCollider in hitColliders)
        {
            MakeEffect(hitCollider);
        }
    }

    protected abstract void MakeEffect(Collider2D hitCollider);

    protected virtual bool CheckAttack()
    {
        return Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(1);
    }

    protected virtual IEnumerator MakeSlash()
    {
        GameObject swing = Instantiate(swingPrefab, transform.position, Quaternion.identity, transform);
        swing.transform.localScale = Vector2.one * radius / 2f;

        yield return new WaitForSeconds(slashDuration);
        Destroy(swing);
    }
}
