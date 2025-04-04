using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreedMelee : EnemyMelee
{
    PlayerStats playerStats;
    public int taken = 0;
    protected override void Start()
    {
        base.Start();
        playerStats = player.GetComponent<PlayerStats>();
    }

    protected override void Update()
    {
        if (playerStats.money > 0)
        {
            if (Time.time > attackTime + attackDelay)
            {
                Attack();
                TakeMoney();
            }
        }
    }

    private void TakeMoney()
    {
        if (Vector2.Distance(transform.position, player.position) < radius)
        {
            playerStats.money = Math.Max(0, playerStats.money - 1);
            playerStats.UpdateUI();
            taken += 1;
        }
    }
}
