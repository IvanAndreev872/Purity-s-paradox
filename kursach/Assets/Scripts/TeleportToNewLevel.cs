using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToNewLevel : MonoBehaviour
{
    public string nextLevelName;
    public int requiredLevelCompleted;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int levelCompleted = PlayerPrefs.GetInt("LevelCompleted", 0);
            if (levelCompleted >= requiredLevelCompleted)
            {
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.Log("Level is unavailable");
            }
        }
    }
}
