using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost Item", menuName = "Inventory/Items/Speed Boost Item")]

public class SpeedItem : AbilityItem
{
    [SerializeField] private float walkAccelerateCoefficient;
    [SerializeField] private float dashAccelerateCoefficient;
    [SerializeField] private float additionalDashDurationTime;
    [SerializeField] private int isDashInvulnerable;
    [SerializeField] private int isDashPoisoned;
    public override void ApplyEffects(PlayerStats player)
    {
        player.walkAccelerateCoefficient += (walkAccelerateCoefficient - 1);
        player.walkSpeed = player.walkSpeedBasic * player.walkAccelerateCoefficient;

        player.dashAccelerateCoefficient += (dashAccelerateCoefficient - 1);
        player.dashSpeed = player.dashSpeedBasic * player.dashAccelerateCoefficient;

        player.dashDuration += additionalDashDurationTime;

        player.isDashInvulnerable += isDashInvulnerable;
        
        player.isDashPoisoned += isDashPoisoned;
    }
    public override void DiscardEffects(PlayerStats player)
    {
        player.walkAccelerateCoefficient -= (walkAccelerateCoefficient - 1);
        player.walkSpeed = player.walkSpeedBasic * player.walkAccelerateCoefficient;

        player.dashAccelerateCoefficient -= (dashAccelerateCoefficient - 1);
        player.dashSpeed = player.dashSpeedBasic * player.dashAccelerateCoefficient;

        player.dashDuration -= additionalDashDurationTime;

        player.isDashInvulnerable -= isDashInvulnerable;

        player.isDashPoisoned -= isDashPoisoned;
    }
}
