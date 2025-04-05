using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeMenu : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenu;
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
    public void BackToMenu()
    {
        ItemsLoader.Instance.SaveProgress(true);
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
