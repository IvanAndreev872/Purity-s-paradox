using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Boost Item", menuName = "Inventory/Items/Sword Boost Item")]

public class SwordBoostItem : AbilityItem
{
    public float swordAdditionalDamage = 0;
    public float poisonDamagePerSec = 0;
    public float fireDamagePerSec = 0;
    public float bleedingDamagePerSec = 0;
    public float freezeCoefficient = 1;
    public float swordAdditionalRadius = 0;
    public float swordSpeedCoefficient = 1;
    void Awake()
    {
        itemType = ItemType.Ability;
    }
    public override void ApplyEffects(PlayerStats player)
    {
        player.swordAdditionalDamage = swordAdditionalDamage;
        player.poisonDamagePerSec += poisonDamagePerSec;
        player.fireDamagePerSec += fireDamagePerSec;
        player.bleedingDamagePerSec += bleedingDamagePerSec;
        player.freezeCoefficient += freezeCoefficient - 1;
        player.swordAdditionalRadius += swordAdditionalRadius;
        player.swordSpeed *= swordSpeedCoefficient;

    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.swordAdditionalDamage = 0;
        player.poisonDamagePerSec -= poisonDamagePerSec;
        player.fireDamagePerSec -= fireDamagePerSec;
        player.bleedingDamagePerSec -= bleedingDamagePerSec;
        player.freezeCoefficient -= (freezeCoefficient - 1);
        player.swordAdditionalRadius = 0;
        player.swordSpeed = 1;
    }
}
