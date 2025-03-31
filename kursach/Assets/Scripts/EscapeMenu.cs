using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    public GameObject escapeMenu;
    private bool isPaused = false;
    private void Awake()
    {
        escapeMenu = transform.GetChild(0).gameObject;
        escapeMenu.SetActive(false);
        transform.gameObject.SetActive(true);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        isPaused = true;
        escapeMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        isPaused = false;
        escapeMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void SaveProgress()
    {
        Transform player = GameObject.FindGameObjectWithTag("Character").transform;
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
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
    }
    public void BackToMenu()
    {
        SaveProgress();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
