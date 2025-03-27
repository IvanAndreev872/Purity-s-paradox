using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Item", menuName = "Inventory/Weapons/Sword")]

public class SwordItem : WeaponItem
{
    
    public float swordDamage = 0;
    public float swordSpeedCoefficient = 1;
    public float swordAdditionalRadius = 0;
    public float bleedingSwordDamagePerSec = 0;
    public float poisonSwordDamagePerSec = 0;
    public float fireSwordDamagePerSec = 0;
    public float freezeSwordCoefficient = 1;
    public override void ApplyEffects(PlayerStats player)
    {
        player.swordDamage += swordDamage;
        player.swordSpeedCoefficient += (swordSpeedCoefficient - 1);
        player.swordRadius += swordAdditionalRadius;
        player.bleedingSwordDamagePerSec += bleedingSwordDamagePerSec;
        player.poisonSwordDamagePerSec += poisonSwordDamagePerSec;
        player.fireSwordDamagePerSec += fireSwordDamagePerSec;
        player.freezeSwordCoefficient += (freezeSwordCoefficient - 1);
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.swordDamage -= swordDamage;
        player.swordSpeedCoefficient -= (swordSpeedCoefficient - 1);
        player.swordRadius -= swordAdditionalRadius;
        player.bleedingSwordDamagePerSec -= bleedingSwordDamagePerSec;
        player.poisonSwordDamagePerSec -= poisonSwordDamagePerSec;
        player.fireSwordDamagePerSec -= fireSwordDamagePerSec;
        player.freezeSwordCoefficient -= (freezeSwordCoefficient - 1);
    }
}
