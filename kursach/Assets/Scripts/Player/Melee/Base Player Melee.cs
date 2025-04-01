using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayerMelee : PlayerMelee
{
    protected override void MakeEffect(Collider2D hitCollider)
    {
        DamageInterface enemy = hitCollider.gameObject.GetComponent<DamageInterface>();
        if (enemy != null)
        {
            enemy.Hit(damage);
        }
    }
}
