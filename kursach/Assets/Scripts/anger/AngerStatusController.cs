using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngerStatusController : MonoBehaviour, DamageInterface
{
    public enum AngerLevel { Calm, Raged, Enraged }
    public AngerLevel current_enragement { get; private set; }

    public delegate void OnAngerChanged(AngerLevel new_level);
    public event OnAngerChanged EnragementChanged;

    public float max_healthpoint;

    public float raged_after_healthpoint;
    public float enraged_after_healthpoint;

    private float current_health;
    // Start is called before the first frame updateú
    void Awake()
    {
        current_health = max_healthpoint;
        current_enragement = AngerLevel.Calm;
    }

    public void Hit(float damage)
    {
        current_health -= damage;
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (current_health < 0) 
        {
            Die();
        } 
        else
        {
            AngerLevel new_enragement_level = current_enragement;
            if (current_health <= enraged_after_healthpoint)
            {
                Debug.Log(1);
                new_enragement_level = AngerLevel.Enraged;
            }
            else if (current_health <= raged_after_healthpoint)
            {
                Debug.Log(0);
                new_enragement_level = AngerLevel.Raged;
            }

            if (current_enragement != new_enragement_level)
            {
                Debug.Log(2);
                current_enragement = new_enragement_level;
                EnragementChanged?.Invoke(new_enragement_level);
            }
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
