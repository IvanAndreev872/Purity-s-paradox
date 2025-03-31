using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class TeleportToNewLevel : MonoBehaviour
{
    public string nextLevelName;
    public int requiredLevelCompleted;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            Transform player = other.transform;
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            int levelCompleted = playerStats.levelCompleted;
            if (levelCompleted >= requiredLevelCompleted)
            {
                string filePath = Application.persistentDataPath + "/playerStats.json";
                playerStats.SaveToJson(filePath);
                InventoryManager inventoryManager = player.GetComponent<InventoryManager>();
                filePath = Application.persistentDataPath + "/inventory.json";
                inventoryManager.SaveInventory(filePath);
                StorageManager storage = FindObjectOfType<StorageManager>();
                if (storage != null)
                {
                    filePath = Application.persistentDataPath + "/storage.json";
                    storage.SaveStorage(filePath);
                }
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.Log("Level is unavailable");
            }
        }
    }
}
