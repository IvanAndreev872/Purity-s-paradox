using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Staff Item", menuName = "Inventory/Weapons/Staff")]

public class StaffItem : WeaponItem
{
    
    [SerializeField] private float staffDamage = 0;
    [SerializeField] private float staffRange = 0;
    [SerializeField] private float staffAttackDelay = 0;
    [SerializeField] private float staffBulletSpeed = 0;
    [SerializeField] private int isStaffEquipped = 1;
    [SerializeField] private int staffEffectAttacksCount = 0;
    [SerializeField] private float staffEffectAttacksDelay = 0;
    [SerializeField] private float staffEffectDamage = 0;
    [SerializeField] private int isStaffPoisoned = 0;
    [SerializeField] private int isStaffFired = 0;
    [SerializeField] private float staffSlowdownTime = 0;
    [SerializeField] private float staffSlowdownCoeffitient = 1;
    [SerializeField] private int isStaffFreezed = 0;
    [SerializeField] private float staffFireCoefficient = 1;

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
                    shooter.bulletPrefab = bullet;
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
                    shooter.bulletPrefab = bullet;
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
                    shooter.bulletPrefab = bullet;
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
                    shooter.bulletPrefab = bullet;
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
