using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Armor Item", menuName = "Inventory/Items/Armor Item")]

public class ArmorItem : AbilityItem
{
    public int extraHealth;
    public override void ApplyEffects(PlayerStats player)
    {
        player.maxHealth += extraHealth;
        if (SceneManager.GetActiveScene().name == "Hub")
        {
            player.health = player.maxHealth;
            player.UpdateUI();
        }
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.maxHealth -= extraHealth;
        if (SceneManager.GetActiveScene().name == "Hub")
        {
            player.health = player.maxHealth;
        }
        else
        {
            player.health = Math.Min(player.health, player.maxHealth);
        }
        player.UpdateUI();
    }
}
