using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // basics stats
    public float walkSpeed = 0;
    public float dashSpeed = 0;
    public float dashDuration = 0;
    public float health = 100;
    public int money = 0;
    // hp
    public float extraHealth = 0;
    // speed
    public float walkAccelerateCoefficient = 1;
    public float dashAccelerateCoefficient = 1;
    public float additionalDashDurationTime = 0;
    public bool isDashPoisoned = false;
    public bool isDashInvulnerable = false;
    // combat
    public float swordDamage = 0;
    public float staffDamage = 0;
    public float swordRadius = 0;
    public float staffRadius = 0;
    public float swordSpeed = 0;
    public float staffSpeed = 0;
    public float bulletSpeed = 0;
    public float swordAdditionalDamage = 0;
    public float staffAdditionalDamage = 0;
    public float poisonDamagePerSec = 0;
    public float fireDamagePerSec = 0;
    public float bleedingDamagePerSec = 0;
    public float freezeCoefficient = 1;
    public float swordAdditionalRadius = 0;
    public float staffAdditionalRadius = 0;
    public float swordSpeedCoefficient = 1;
    public float staffSpeedCoefficient = 1;
    public float staffBulletSpeedCoefficient = 1;

}
