using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost Item", menuName = "Inventory/Items/Speed Boost Item")]

public class SpeedItem : AbilityItem
{
    public float walkAccelerateCoefficient;
    public float dashAccelerateCoefficient;
    public float additionalDashDurationTime;
    public bool isDashInvulnerable;
    public bool isDashPoisoned;

    void Awake()
    {
        itemType = ItemType.Ability;
    }
    public override void ApplyEffects(PlayerStats player)
    {
        player.walkAccelerateCoefficient *= walkAccelerateCoefficient;
        player.dashAccelerateCoefficient *= dashAccelerateCoefficient;
        player.additionalDashDurationTime += additionalDashDurationTime;
        player.isDashInvulnerable = isDashInvulnerable;
        player.isDashPoisoned = isDashPoisoned;
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.walkAccelerateCoefficient = 1;
        player.dashAccelerateCoefficient = 1;
        player.additionalDashDurationTime -= additionalDashDurationTime;
        player.isDashInvulnerable = false;
        player.isDashPoisoned = false;
    }
}
