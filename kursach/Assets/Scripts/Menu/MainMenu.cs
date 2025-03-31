using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueButton;
    private void Awake()
    {
        continueButton = transform.GetChild(0).gameObject;
        string filePath = Application.persistentDataPath + "/playerStats.json";
        if (File.Exists(filePath))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }
    public void ContinueGame()
    {
        SceneManager.LoadScene("Hub");
    }
    public void NewGame()
    {
        string filePath = Application.persistentDataPath + "/playerStats.json";
        string filePathNewGame = Application.persistentDataPath + "/playerStatsNewGame.json";
        CopyJson(filePathNewGame, filePath);
        filePath = Application.persistentDataPath + "/inventory.json";
        filePathNewGame = Application.persistentDataPath + "/inventoryNewGame.json";
        CopyJson(filePathNewGame, filePath);
        filePath = Application.persistentDataPath + "/storage.json";
        filePathNewGame = Application.persistentDataPath + "/storageNewGame.json";
        CopyJson(filePathNewGame, filePath);
        SceneManager.LoadScene("Hub");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void CopyJson(string sourcePath, string destinationPath)
    {
        if (File.Exists(sourcePath))
        {
            File.Copy(sourcePath, destinationPath, overwrite: true);
        }
    }
}
