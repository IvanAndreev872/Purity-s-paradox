using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityItem : ItemScriptableObject
{
    public abstract void ApplyEffects(PlayerStats playerStats);
    public abstract void DiscardEffects(PlayerStats playerStats);
}