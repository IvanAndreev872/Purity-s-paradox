using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;
    public void ContinueGame()
    {
        SceneManager.LoadScene("Hub");
    }
    public void NewGame()
    {
        SceneManager.LoadScene("Hub");
    }
    public void QuitGame()
    {
        Debug.Log("QUIT!!");
        Application.Quit();
    }
    public void SetVolume()
    {
        PlayerPrefs.SetFloat("Volume", AudioListener.volume);
    }
}
