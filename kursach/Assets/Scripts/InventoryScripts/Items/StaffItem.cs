using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Item", menuName = "Inventory/Weapons/Staff")]

public class StaffItem : WeaponItem
{
    
    public float staffDamage = 0;
    public float staffAdditionalRadius = 0;
    public float staffSpeedCoefficient = 1;
    public float staffBulletSpeedCoefficient = 1;
    public float poisonStaffDamagePerSec = 0;
    public float fireStaffDamagePerSec = 0;
    public float freezeStaffCoefficient = 1;
    public override void ApplyEffects(PlayerStats player)
    {
        player.staffDamage += staffDamage;
        player.staffRadius += staffAdditionalRadius;
        player.staffSpeedCoefficient += (staffSpeedCoefficient - 1);
        player.staffBulletSpeedCoefficient += (staffBulletSpeedCoefficient - 1);
        player.poisonStaffDamagePerSec += poisonStaffDamagePerSec;
        player.fireStaffDamagePerSec += fireStaffDamagePerSec;
        player.freezeStaffCoefficient += (freezeStaffCoefficient - 1);
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.staffDamage -= staffDamage;
        player.staffRadius -= staffAdditionalRadius;
        player.staffSpeedCoefficient -= (staffSpeedCoefficient - 1);
        player.staffBulletSpeedCoefficient -= (staffBulletSpeedCoefficient - 1);
        player.poisonStaffDamagePerSec -= poisonStaffDamagePerSec;
        player.fireStaffDamagePerSec -= fireStaffDamagePerSec;
        player.freezeStaffCoefficient -= (freezeStaffCoefficient - 1);
    }
}
