using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Sword Item", menuName = "Inventory/Weapons/Sword")]

public class SwordItem : WeaponItem
{
    
    public float swordDamage = 0;
    public float swordRadius = 0;
    public int isSwordEquipped = 1;
    public float swordAttackDelay = 0;
    public float swordSlashDuration = 0;
    public int swordEffectAttacksCount = 0;
    public float swordEffectAttacksDelay = 0;
    public float swordEffectDamage = 0;
    public int isSwordPoisoned = 0;
    public int isSwordBleeding = 0;
    public int isSwordFired = 0;
    public float swordSlowdownTime = 0;
    public float swordSlowdownCoeffitient = 1;
    public int isSwordFreezed = 0;
    public override async void ApplyEffects(PlayerStats player)
    {
        player.swordDamage += swordDamage;
        player.swordRadius += swordRadius;
        player.isSwordEquipped += isSwordEquipped;
        player.swordAttackDelay -= swordAttackDelay;
        player.swordSlashDuration += swordSlashDuration;
        player.swordEffectAttacksCount += swordEffectAttacksCount;
        player.swordEffectAttacksDelay += swordEffectAttacksDelay;
        player.swordEffectDamage += swordEffectDamage;
        player.isSwordPoisoned += isSwordPoisoned;
        player.isSwordBleeding += isSwordBleeding;
        player.isSwordFired += isSwordFired;
        player.swordSlowdownTime += swordSlowdownTime;
        player.swordSlowdownCoeffitient += (swordSlowdownCoeffitient - 1);
        player.isSwordFreezed += isSwordFreezed;
        GameObject swingPrefab = await ItemsLoader.Instance.LoadSwing();
        Transform playerTransform = player.transform;
        if (isSwordFreezed > 0)
        {
            FreezePlayerMelee freeze = playerTransform.AddComponent<FreezePlayerMelee>();
            freeze.swingPrefab = swingPrefab;
            freeze.UpdateFreezeSword(player);
        }
        else if (isSwordPoisoned > 0)
        {
            PoisonPlayerMelee poison = playerTransform.AddComponent<PoisonPlayerMelee>();
            poison.swingPrefab = swingPrefab;
            poison.UpdatePoisonedSword(player);
        }
        else if (isSwordBleeding > 0)
        {
            BleedingPlayerMelee bleed = playerTransform.AddComponent<BleedingPlayerMelee>();
            bleed.swingPrefab = swingPrefab;
            bleed.UpdateBleedingSword(player);
        }
        else if (isSwordFired > 0)
        {
            FirePlayerMelee fire = playerTransform.AddComponent<FirePlayerMelee>();
            fire.swingPrefab = swingPrefab;
            fire.UpdateFireSword(player);
        }
        else if (isSwordEquipped > 0)
        {
            BasePlayerMelee melee = playerTransform.AddComponent<BasePlayerMelee>();
            melee.swingPrefab = swingPrefab;
            melee.UpdateBaseSword(player);
        }
    }
    public override void DiscardEffects(PlayerStats player)
    {
        Transform playerTransform = player.transform;
        if (isSwordFreezed > 0)
        {
            FreezePlayerMelee freeze = playerTransform.GetComponent<FreezePlayerMelee>();
            Destroy(freeze);
        }
        else if (isSwordPoisoned > 0)
        {
            PoisonPlayerMelee poison = playerTransform.GetComponent<PoisonPlayerMelee>();
            Destroy(poison);
        }
        else if (isSwordBleeding > 0)
        {
            BleedingPlayerMelee bleed = playerTransform.GetComponent<BleedingPlayerMelee>();
            Destroy(bleed);
        }
        else if (isSwordFired > 0)
        {
            FirePlayerMelee fire = playerTransform.GetComponent<FirePlayerMelee>();
            Destroy(fire);
        }
        else if (isSwordEquipped > 0)
        {
            BasePlayerMelee melee = playerTransform.GetComponent<BasePlayerMelee>();
            Destroy(melee);
        }
        player.swordDamage -= swordDamage;
        player.swordRadius -= swordRadius;
        player.isSwordEquipped -= isSwordEquipped;
        player.swordAttackDelay += swordAttackDelay;
        player.swordSlashDuration -= swordSlashDuration;
        player.swordEffectAttacksCount -= swordEffectAttacksCount;
        player.swordEffectAttacksDelay -= swordEffectAttacksDelay;
        player.swordEffectDamage -= swordEffectDamage;
        player.isSwordPoisoned -= isSwordPoisoned;
        player.isSwordBleeding -= isSwordBleeding;
        player.isSwordFired -= isSwordFired;
        player.swordSlowdownTime -= swordSlowdownTime;
        player.swordSlowdownCoeffitient -= (swordSlowdownCoeffitient - 1);
        player.isSwordFreezed -= isSwordFreezed;
    }
}
