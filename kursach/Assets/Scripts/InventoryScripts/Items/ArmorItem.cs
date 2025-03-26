using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor Item", menuName = "Inventory/Items/Armor Item")]

public class ArmorItem : AbilityItem
{
    public int extraHealth;

    void Awake()
    {
        itemType = ItemType.Ability;
    }
    public override void ApplyEffects(PlayerStats player)
    {
        player.extraHealth += extraHealth;
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.extraHealth = 0;
    }
}
