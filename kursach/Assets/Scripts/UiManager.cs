using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UiManager : MonoBehaviour
{
    public Transform UIPanel;
    TMP_Text hpText, moneyText;
    public PlayerStats playerStats;
    public void Awake()
    {
        playerStats = GameObject.FindGameObjectWithTag("Character").transform.GetComponent<PlayerStats>();
        hpText = UIPanel.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        moneyText = UIPanel.GetChild(1).GetChild(1).GetComponent<TMP_Text>();
        SetUI(playerStats.health, playerStats.money);
    }
    public void UpdateUI()
    {
        hpText.text = playerStats.health.ToString();
        moneyText.text = playerStats.money.ToString();
    }
    public void SetUI(float health, int money)
    {
        hpText.text = health.ToString();
        moneyText.text = money.ToString();
    }
}
