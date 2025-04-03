using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Item", menuName = "Inventory/Weapons/Staff")]

public class StaffItem : WeaponItem
{
    
    public float staffDamage = 0;
    public float staffRange = 0;
    public float staffAttackDelay = 0;
    public float staffBulletSpeed = 0;
    public int isStaffEquipped = 1;
    public int staffEffectAttacksCount = 0;
    public float staffEffectAttacksDelay = 0;
    public float staffEffectDamage = 0;
    public int isStaffPoisoned = 0;
    public int isStaffFired = 0;
    public float staffSlowdownTime = 0;
    public float staffSlowdownCoeffitient = 1;
    public int isStaffFreezed = 0;
    public float staffFireCoefficient = 1;

    public override async void ApplyEffects(PlayerStats player)
    {
        player.staffDamage += staffDamage;
        player.staffRange += staffRange;
        player.staffAttackDelay += staffAttackDelay;
        player.staffBulletSpeed += staffBulletSpeed;
        player.isStaffEquipped += isStaffEquipped;
        player.staffEffectAttacksCount += staffEffectAttacksCount;
        player.staffEffectAttacksDelay += staffEffectAttacksDelay;
        player.staffEffectDamage += staffEffectDamage;
        player.isStaffPoisoned += isStaffPoisoned;
        player.isStaffFired += isStaffFired;
        player.staffSlowdownTime += staffSlowdownTime;
        player.staffSlowdownCoeffitient += (staffSlowdownCoeffitient - 1);
        player.isStaffFreezed += isStaffFreezed;
        player.staffFireCoefficient += (staffFireCoefficient - 1);
        PlayerShooting shooter = player.transform.GetChild(0).GetComponent<PlayerShooting>();
        shooter.UpdateShooter(player);
        List<GameObject> bullets = await ItemsLoader.Instance.LoadAllBulletsPrefabs();
        if (isStaffFreezed > 0)
        {
            foreach (GameObject bullet in bullets)
            {
                if (bullet.GetComponent<FreezeProjectile>() != null)
                {
                    bullet.GetComponent<FreezeProjectile>().UpdateFreezeBullet(player);
                    shooter.bullet_prefab = bullet;
                    break;
                }
            }
        }
        else if (isStaffFired > 0)
        {
            foreach (GameObject bullet in bullets)
            {
                if (bullet.GetComponent<FireProjectile>() != null)
                {
                    bullet.GetComponent<FireProjectile>().UpdateFireBullet(player);
                    shooter.bullet_prefab = bullet;
                    break;
                }
            }
        }
        else if (isStaffPoisoned > 0)
        {
            foreach (GameObject bullet in bullets)
            {
                if (bullet.GetComponent<PoisonProjectile>() != null)
                {
                    bullet.GetComponent<PoisonProjectile>().UpdatePoisonBullet(player);
                    shooter.bullet_prefab = bullet;
                    break;
                }
            }
        }
        else if (isStaffEquipped > 0)
        {
            foreach (GameObject bullet in bullets)
            {
                if (bullet.GetComponent<WaterProjectile>() != null)
                {
                    bullet.GetComponent<WaterProjectile>().UpdateWaterBullet(player);
                    shooter.bullet_prefab = bullet;
                    break;
                }
            }
        }
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.staffDamage -= staffDamage;
        player.staffRange -= staffRange;
        player.staffAttackDelay -= staffAttackDelay;
        player.staffBulletSpeed -= staffBulletSpeed;
        player.isStaffEquipped -= isStaffEquipped;
        player.staffEffectAttacksCount -= staffEffectAttacksCount;
        player.staffEffectAttacksDelay -= staffEffectAttacksDelay;
        player.staffEffectDamage -= staffEffectDamage;
        player.isStaffPoisoned -= isStaffPoisoned;
        player.isStaffFired -= isStaffFired;
        player.staffSlowdownTime -= staffSlowdownTime;
        player.staffSlowdownCoeffitient -= (staffSlowdownCoeffitient - 1);
        player.isStaffFreezed -= isStaffFreezed;
        player.staffFireCoefficient -= (staffFireCoefficient - 1);
        PlayerShooting shooter = player.transform.GetChild(0).GetComponent<PlayerShooting>();
        shooter.UpdateShooter(player);
    }
}
