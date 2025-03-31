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
            if (levelCompleted >= requiredLevelCompleted || true)
            {
                ItemsLoader.Instance.SaveProgress(false);
                SceneManager.LoadScene(nextLevelName);
            }
            else
            {
                Debug.Log("Level is unavailable");
            }
        }
    }
}
