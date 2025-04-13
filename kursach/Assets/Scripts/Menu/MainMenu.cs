using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject continueButton;
    private void Awake()
    {
        continueButton = transform.GetChild(0).gameObject;
        ConfigData configData = ConfigManager.LoadConfig();
        if (configData.hasGuideShowed)
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
    void EnsureFileExists(string filePath, string defaultContent = "{}")
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, defaultContent);
        }
    }
    public void NewGame()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "playerStats.json");
        string filePathNewGame = Path.Combine(Application.streamingAssetsPath + "playerStatsNewGame.json");
        EnsureFileExists(filePath);
        CopyJson(filePathNewGame, filePath);
        filePath = Path.Combine(Application.streamingAssetsPath + "inventory.json");
        filePathNewGame = Path.Combine(Application.streamingAssetsPath + "inventoryNewGame.json");
        EnsureFileExists(filePath);
        CopyJson(filePathNewGame, filePath);
        filePath = Path.Combine(Application.streamingAssetsPath + "storage.json");
        filePathNewGame = Path.Combine(Application.streamingAssetsPath + "storageNewGame.json");
        EnsureFileExists(filePath);
        CopyJson(filePathNewGame, filePath);
        ConfigData configData = ConfigManager.LoadConfig();
        configData.hasGuideShowed = false;
        ConfigManager.SaveConfig(configData);
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
