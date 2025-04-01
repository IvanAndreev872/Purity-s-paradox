using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Boost Item", menuName = "Inventory/Items/Sword Boost Item")]

public class SwordBoostItem : AbilityItem
{
    public float swordAdditionalDamage = 0;
    public float swordAttackDelay = 0;
    public override void ApplyEffects(PlayerStats player)
    {
        player.swordDamage += swordAdditionalDamage;
        player.swordAttackDelay += swordAttackDelay;
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.swordDamage -= swordAdditionalDamage;
        player.swordAttackDelay -= swordAttackDelay;
    }
}
