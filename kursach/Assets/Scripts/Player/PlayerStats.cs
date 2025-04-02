using System.IO;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // movement
    public float walkSpeedBasic = 5;
    public float walkSpeed = 5;
    public float dashSpeedBasic = 15;
    public float dashSpeed = 15;
    public float dashDuration = 0.5f;
    public float walkAccelerateCoefficient = 1;
    public float dashAccelerateCoefficient = 1;
    public int isDashPoisoned = 0;
    public int isDashInvulnerable = 0;
    // hp
    public float health = 100;
    public float maxHealth = 100;
    // money
    public int money = 0;
    // progress
    public int levelCompleted = 0;
    // combat
    // sword
    public float swordDamage = 0;
    public float swordRadius = 0;
    public float swordAttackDelay = 0;
    public float swordSlashDuration = 0;
    public int isSwordEquipped = 0;
    // sword with effect
    public int swordEffectAttacksCount = 0;
    public float swordEffectAttacksDelay = 0;
    public float swordEffectDamage = 0;
    public int isSwordPoisoned = 0;
    public int isSwordBleeding = 0;
    public int isSwordFired = 0;
    // freeze sword
    public float swordSlowdownTime = 0;
    public float swordSlowdownCoeffitient = 1;
    public int isSwordFreezed = 0;
    // staff
    public float staffDamage = 0;
    public float staffRadius = 0;
    public float staffSpeed = 0;
    public float bulletSpeed = 0;
    public float staffSpeedCoefficient = 1;
    public float staffBulletSpeedCoefficient = 1;
    public float poisonStaffDamagePerSec = 0;
    public float fireStaffDamagePerSec = 0;
    public float freezeStaffCoefficient = 1;
    public int isStaffEquipped = 0;


    private void Awake()
    {
        string filePath = Application.streamingAssetsPath + "/playerStats.json";
        LoadFromJson(filePath);
    }
    private void Start()
    {
        UpdateUI();   
    }
    public void SaveToJson(string filePath)
    {
        string json = JsonUtility.ToJson(this);
        File.WriteAllText(filePath, json);
        Debug.Log("Player stats saved to: " + filePath);
    }
    public void LoadFromJson(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
            Debug.Log("Player stats loaded from: " + filePath);
        }
        else
        {
            Debug.Log("Save file not found at: " + filePath);
        }
    }
    public void UpdateUI()
    {
        UiManager uiManager = transform.GetComponent<UiManager>();
        uiManager.UpdateUI();
    }
}
