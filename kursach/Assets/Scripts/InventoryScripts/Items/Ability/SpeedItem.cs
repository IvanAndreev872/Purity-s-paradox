using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Speed Boost Item", menuName = "Inventory/Items/Speed Boost Item")]

public class SpeedItem : AbilityItem
{
    public float walkAccelerateCoefficient;
    public float dashAccelerateCoefficient;
    public float additionalDashDurationTime;
    public int isDashInvulnerable;
    public int isDashPoisoned;
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
