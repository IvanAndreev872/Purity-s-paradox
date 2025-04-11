using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportToNewLevel : MonoBehaviour
{
    [SerializeField] private string nextLevelName;
    [SerializeField] private int requiredLevelCompleted;
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            Transform player = other.transform;
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            int levelCompleted = playerStats.levelCompleted;
            if (levelCompleted >= requiredLevelCompleted)
            {
                string name = SceneManager.GetActiveScene().name;
                Debug.Log(name);
                name = name.ToLower();
                if (name == "First Boss".ToLower())
                {
                    playerStats.levelCompleted = 1;
                }
                else if (name == "Level2".ToLower())
                {
                    playerStats.levelCompleted = 2;
                }
                else if (name == "Second Boss".ToLower())
                {
                    playerStats.levelCompleted = 3;
                }
                else if (name == "Level4".ToLower())
                {
                    playerStats.levelCompleted = 4;
                }
                else if (name == "Third Boss".ToLower())
                {
                    playerStats.levelCompleted = 5;
                }
                ItemsLoader.Instance.SaveProgress(true);
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                // Debug.Log("Level is unavailable");
            }
        }
    }
}
