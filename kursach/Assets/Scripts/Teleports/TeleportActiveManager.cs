using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportActiveManager : MonoBehaviour
{
    PlayerStats playerStats;
    [SerializeField] private int levelRequired;
    void Start()
    {
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Character").transform.GetComponent<PlayerStats>();
        transform.GetChild(0).gameObject.SetActive(playerStats.levelCompleted >= levelRequired);
    }
}
