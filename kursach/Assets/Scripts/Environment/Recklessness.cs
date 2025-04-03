using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recklessness : MonoBehaviour
{
    public int count = 0;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            playerStats.money += count;
            playerStats.UpdateUI();
            Destroy(gameObject);
        }
    }
}
