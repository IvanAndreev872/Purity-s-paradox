using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ItemsLoader : MonoBehaviour
{
    public static ItemsLoader Instance { get; private set; }
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
    public async Task<List<Item>> LoadAllItemsFromLevel(int level)
    {
        List<Item> res = new();
        string label = "level" + level.ToString();
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(label, null);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> items = handle.Result;
            foreach (var itemPrefab in items)
            {
                Item item = itemPrefab.GetComponent<Item>();
                res.Add(item);
            }
            Debug.Log("LOADED Items From level " + level + ": " + res.Count);
        }
        else
        {
            Debug.LogError("Failed to load items from level " + level);
        }
        return res;
    }
    public async Task<List<GameObject>> LoadAllEnemiesFromLevel(int level)
    {
        List<GameObject> res = new();
        string label = "EnemyLevel" + level.ToString();
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>(label, null);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> enemies = handle.Result;
            foreach (var enemy in enemies)
            {
                res.Add(enemy);
            }
            Debug.Log("LOADED Enemies from level " + level + ": " + res.Count);
        }
        else
        {
            Debug.LogError("Failed to load enemies from level " + level);
        }
        return res;
    }
    public async Task<List<GameObject>> LoadAllBulletsPrefabs()
    {
        List<GameObject> res = new();
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>("bullet", null);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> bullets = handle.Result;
            foreach (GameObject bulletPrefab in bullets)
            {
                res.Add(bulletPrefab);
            }
            Debug.Log("LOADED Bullets: " + res.Count);
        }
        else
        {
            Debug.LogError("Failed to load bullet prefabs");
        }
        return res;
    }
    public async Task<GameObject> LoadSwing()
    {
        GameObject res = null;
        AsyncOperationHandle<IList<GameObject>> handle = Addressables.LoadAssetsAsync<GameObject>("swing", null);
        await handle.Task;
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            res = handle.Result[0];
            Debug.Log("LOADED Swing: " + res.name);
        }
        else
        {
            Debug.LogError("Failed to load swing from prefabs");
        }
        return res;
    }
    public void SaveProgress(bool isDead)
    {
        Transform player = GameObject.FindGameObjectWithTag("Character").transform;
        InventoryManager inventoryManager = player.GetComponent<InventoryManager>();
        string filePath = Application.streamingAssetsPath + "/inventory.json";
        inventoryManager.SaveInventory(filePath);
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        Debug.Log("DEAD: " + isDead + " " + playerStats.health + " " + playerStats.maxHealth);
        if (isDead)
        {
            playerStats.health = playerStats.maxHealth;
        }
        filePath = Application.streamingAssetsPath + "/playerStats.json";
        playerStats.SaveToJson(filePath);
        StorageManager storage = FindObjectOfType<StorageManager>();
        if (storage != null)
        {
            filePath = Application.streamingAssetsPath + "/storage.json";
            storage.SaveStorage(filePath);
        }
    }
}
