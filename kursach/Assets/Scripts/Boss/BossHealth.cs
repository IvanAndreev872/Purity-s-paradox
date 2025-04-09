using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UI;

public class BossHealth : Health
{
    public Slider slider;
    protected override void Start()
    {
        base.Start();
        slider.value = 1;
    }
    public override void Hit(float damage)
    {
        if (damagable)
        {
            if (currentHealth <= damage)
            {
                slider.gameObject.SetActive(false);
                Die();
            }
            currentHealth -= damage;
            UpdateHealthBar();
        }
    }
    private void UpdateHealthBar()
    {
        slider.value = (float)currentHealth / maxHealthpoint;
    }
}
