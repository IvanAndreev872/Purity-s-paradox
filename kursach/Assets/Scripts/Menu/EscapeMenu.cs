using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenu;
    [SerializeField] private Sprite shootOn;
    [SerializeField] private Sprite shootOff;
    [SerializeField] private GameObject shootButton;
    private bool isPaused = false, shootMode = true, inMenu = true;
    private void Awake()
    {
        string name = SceneManager.GetActiveScene().name.ToLower();
        inMenu = name == "Menu".ToLower();
        if (!inMenu)
        {
            escapeMenu = transform.GetChild(0).gameObject;
            escapeMenu.SetActive(false);
            transform.gameObject.SetActive(true);
        }
    }
    private void Start()
    {
        shootMode = ConfigManager.LoadConfig().shootOnCursor;
        UpdateShootButtonImage();
    }
    void Update()
    {
        if (!inMenu && Input.GetKeyDown(KeyCode.Escape))
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
    public void BackToMenu()
    {
        ItemsLoader.Instance.SaveProgress(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void UpdateShootOnCursor()
    {
        ConfigData configData = ConfigManager.LoadConfig();
        configData.shootOnCursor = !configData.shootOnCursor;
        ConfigManager.SaveConfig(configData);
        GameObject player = GameObject.FindGameObjectWithTag("Character");
        if (player != null)
        {
            player.transform.GetChild(0).GetComponent<PlayerShooting>().UpdateConfig();
        }
    }
    public void UpdateShootButtonImage()
    {
        if (shootMode)
        {
            shootButton.GetComponent<Image>().sprite = shootOn;
        }
        else
        {
            shootButton.GetComponent<Image>().sprite = shootOff;
        }
    }
    public void ClickShootButton()
    {
        UpdateShootOnCursor();
        shootMode = !shootMode;
        UpdateShootButtonImage();
    }
}
