using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Boost Item", menuName = "Inventory/Items/Staff Boost Item")]

public class StaffBoostItem : AbilityItem
{
    [SerializeField] private float staffAdditionalDamage = 0;
    [SerializeField] private float staffAttackDelay = 0;
    [SerializeField] private float staffBulletSpeedCoefficient = 1;
    public override void ApplyEffects(PlayerStats player)
    {
        player.staffDamage += staffAdditionalDamage;
        player.staffAttackDelay -= staffAttackDelay;
        player.staffBulletSpeedCoefficient += (staffBulletSpeedCoefficient - 1);
        player.staffBulletSpeed = player.staffBulletSpeedBasic * player.staffBulletSpeedCoefficient;
        PlayerShooting shooter = player.transform.GetChild(0).GetComponent<PlayerShooting>();
        shooter.UpdateShooter(player);
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.staffDamage -= staffAdditionalDamage;
        player.staffAttackDelay += staffAttackDelay;
        player.staffBulletSpeedCoefficient -= (staffBulletSpeedCoefficient - 1);
        player.staffBulletSpeed = player.staffBulletSpeedBasic * player.staffBulletSpeedCoefficient;
        PlayerShooting shooter = player.transform.GetChild(0).GetComponent<PlayerShooting>();
        shooter.UpdateShooter(player);
    }
}
