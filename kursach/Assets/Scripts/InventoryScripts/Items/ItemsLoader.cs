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
            Debug.Log("LOADED " + res.Count);
        }
        else
        {
            Debug.LogError("Failed to load items from level " + level);
        }
        return res;
    }
    public void SaveProgress(bool isDead)
    {
        Transform player = GameObject.FindGameObjectWithTag("Character").transform;
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (isDead)
        {
            playerStats.health = playerStats.maxHealth;
        }
        string filePath = Application.streamingAssetsPath + "/playerStats.json";
        playerStats.SaveToJson(filePath);
        InventoryManager inventoryManager = player.GetComponent<InventoryManager>();
        filePath = Application.streamingAssetsPath + "/inventory.json";
        inventoryManager.SaveInventory(filePath);
        StorageManager storage = FindObjectOfType<StorageManager>();
        if (storage != null)
        {
            filePath = Application.streamingAssetsPath + "/storage.json";
            storage.SaveStorage(filePath);
        }
    }
}
