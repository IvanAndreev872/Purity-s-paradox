using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost Item", menuName = "Inventory/Items/Speed Boost Item")]

public class SpeedItem : AbilityItem
{
    public float walkAccelerateCoefficient;
    public float additionalWalkSpeed;
    public float dashAccelerateCoefficient;
    public float additionalDashSpeed;
    public float dashDurationAccelerateCoefficient;
    public float additionalDashDurationTime;

    void Awake()
    {
        itemType = ItemType.Ability;
    }
    public override void ApplyEffects(PlayerStats player)
    {
        player.walkAccelerateCoefficient *= walkAccelerateCoefficient;
        player.additionalWalkSpeed += additionalWalkSpeed;
        player.dashAccelerateCoefficient *= dashAccelerateCoefficient;
        player.additionalDashSpeed += additionalDashSpeed;
        player.dashDurationAccelerateCoefficient *= dashDurationAccelerateCoefficient;
        player.additionalDashDurationTime += additionalDashDurationTime;
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.walkAccelerateCoefficient = 1;
        player.additionalWalkSpeed = 0;
        player.dashAccelerateCoefficient = 1;
        player.additionalDashSpeed = 0;
        player.dashDurationAccelerateCoefficient = 1;
        player.additionalDashDurationTime = 0;
    }
}
