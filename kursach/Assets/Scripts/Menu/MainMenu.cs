using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private void Awake()
    {
        continueButton = transform.GetChild(0).gameObject;
        string filePath = Application.streamingAssetsPath + "/playerStats.json";
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
        string filePath = Application.streamingAssetsPath + "/playerStats.json";
        string filePathNewGame = Application.streamingAssetsPath + "/playerStatsNewGame.json";
        CopyJson(filePathNewGame, filePath);
        filePath = Application.streamingAssetsPath + "/inventory.json";
        filePathNewGame = Application.streamingAssetsPath + "/inventoryNewGame.json";
        CopyJson(filePathNewGame, filePath);
        filePath = Application.streamingAssetsPath + "/storage.json";
        filePathNewGame = Application.streamingAssetsPath + "/storageNewGame.json";
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
