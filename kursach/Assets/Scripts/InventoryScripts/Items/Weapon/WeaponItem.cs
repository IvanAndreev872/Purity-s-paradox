using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponItem : ItemScriptableObject
{
    public abstract void ApplyEffects(PlayerStats playerStats);
    public abstract void DiscardEffects(PlayerStats playerStats);
}