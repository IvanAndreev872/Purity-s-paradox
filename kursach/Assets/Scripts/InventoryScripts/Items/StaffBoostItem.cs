using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Boost Item", menuName = "Inventory/Items/Staff Boost Item")]

public class StaffBoostItem : AbilityItem
{
    public float staffAdditionalDamage = 0;
    public float poisonDamagePerSec = 0;
    public float fireDamagePerSec = 0;
    public float bleedingDamagePerSec = 0;
    public float freezeCoefficient = 1;
    public float staffAdditionalRadius = 0;
    public float staffSpeedCoefficient = 1;
    public float staffBulletSpeedCoefficient = 1;
    void Awake()
    {
        itemType = ItemType.Ability;
    }
    public override void ApplyEffects(PlayerStats player)
    {
        player.staffAdditionalDamage = staffAdditionalDamage;
        player.poisonDamagePerSec += poisonDamagePerSec;
        player.fireDamagePerSec += fireDamagePerSec;
        player.bleedingDamagePerSec += bleedingDamagePerSec;
        player.freezeCoefficient += freezeCoefficient - 1;
        player.swordAdditionalRadius += staffAdditionalRadius;
        player.staffSpeed *= staffSpeedCoefficient;
        player.staffBulletSpeedCoefficient *= staffBulletSpeedCoefficient;
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.staffAdditionalDamage = 0;
        player.poisonDamagePerSec -= poisonDamagePerSec;
        player.fireDamagePerSec -= fireDamagePerSec;
        player.bleedingDamagePerSec -= bleedingDamagePerSec;
        player.freezeCoefficient -= (freezeCoefficient - 1);
        player.swordAdditionalRadius -= staffAdditionalRadius;
        player.staffSpeed = 1;
        player.staffBulletSpeedCoefficient = 1;
    }
}
