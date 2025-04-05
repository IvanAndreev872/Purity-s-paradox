using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Boost Item", menuName = "Inventory/Items/Sword Boost Item")]

public class SwordBoostItem : AbilityItem
{
    [SerializeField] private float swordAdditionalDamage = 0;
    [SerializeField] private float swordAttackDelay = 0;
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
