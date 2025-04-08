using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUploader : MonoBehaviour
{
    private static SceneUploader Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "LevelFarm")
        {
            StartCoroutine(GenerateItemsAfterSceneLoad());
        }
    }

    private IEnumerator GenerateItemsAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.1f);
        CorridorFirstDungeonGenerator generator = FindObjectOfType<CorridorFirstDungeonGenerator>();
        PlayerStats playerStats = GameObject.FindGameObjectWithTag("Character").transform.GetComponent<PlayerStats>();
        if (generator != null)
        {
            for (int level = 1; level <= playerStats.levelCompleted + 1; level++)
            {
                if (level == 2 || level >= 6) 
                {
                    continue;
                }
                Task<List<GameObject>> loadEnemiesTask = ItemsLoader.Instance.LoadAllEnemiesFromLevel(level);
                yield return new WaitUntil(() => loadEnemiesTask.IsCompleted);
                List<GameObject> enemies = loadEnemiesTask.Result;
                foreach (var enemy in enemies)
                {
                    if (!generator.EnemyPrefabs.Contains(enemy))
                    {
                        generator.EnemyPrefabs.Add(enemy);
                    }
                }
            }
            generator.GenerateDungeon();
        }
        else
        {
            Debug.LogError("Unluck");
        }
    }
}
