using System.Collections;
using System.Collections.Generic;
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
        if (generator != null)
        {
            generator.RunGeneration();
        }
        else
        {
            Debug.LogError("Unluck");
        }
    }
}
