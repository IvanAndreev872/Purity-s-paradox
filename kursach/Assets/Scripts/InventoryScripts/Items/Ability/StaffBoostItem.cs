using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Boost Item", menuName = "Inventory/Items/Staff Boost Item")]

public class StaffBoostItem : AbilityItem
{
    public float staffAdditionalDamage = 0;
    public float staffAdditionalRadius = 0;
    public float staffSpeedCoefficient = 1;
    public float staffBulletSpeedCoefficient = 1;
    public override void ApplyEffects(PlayerStats player)
    {
        player.staffDamage += staffAdditionalDamage;
        player.staffRadius += staffAdditionalRadius;
        player.staffSpeedCoefficient += staffSpeedCoefficient - 1;
        player.staffBulletSpeedCoefficient += staffBulletSpeedCoefficient - 1;
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.staffDamage -= staffAdditionalDamage;
        player.staffRadius -= staffAdditionalRadius;
        player.staffSpeedCoefficient -= (staffSpeedCoefficient - 1);
        player.staffBulletSpeedCoefficient -= (staffBulletSpeedCoefficient - 1);
    }
}
