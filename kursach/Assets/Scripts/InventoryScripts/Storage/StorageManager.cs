using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class StorageManager : MonoBehaviour
{
    public Transform storage;
    private int indexOfCurrentPage = 0;
    public List<StoragePage> pages = new List<StoragePage>();
    public TMP_Text itemDescriptionText, costText, pageText;
    public Button takeButton, nextButton, prevButton, storageButton, inventoryButton;
    public int slotIdClicked = -1;
    public InventoryManager inventoryManager;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseStorage();
        }
    }
    public void Awake()
    {
        for (int i = 0; i < storage.childCount; i++)
        {
            if (storage.GetChild(i).GetComponent<StoragePage>() != null)
            {
                pages.Add(storage.GetChild(i).GetComponent<StoragePage>());
            }
        }
        storage.gameObject.SetActive(true);
        itemDescriptionText = storage.GetChild(5).GetComponent<TMP_Text>();
        itemDescriptionText.text = "";
        takeButton = storage.GetChild(6).GetComponent<Button>();
        takeButton.gameObject.SetActive(false);
        prevButton = storage.GetChild(7).GetComponent<Button>();
        prevButton.gameObject.SetActive(false);
        nextButton = storage.GetChild(8).GetComponent<Button>();
        nextButton.gameObject.SetActive(false);
        costText = storage.GetChild(9).GetComponent<TMP_Text>();
        costText.text = "";
        costText.gameObject.SetActive(false);
        storageButton = storage.GetChild(10).GetComponent<Button>();
        storageButton.gameObject.SetActive(false);
        inventoryButton = storage.GetChild(11).GetComponent<Button>();
        inventoryButton.gameObject.SetActive(false);
        pageText = storage.GetChild(12).GetComponent<TMP_Text>();
        pageText.gameObject.SetActive(false);
        Transform player = GameObject.FindGameObjectWithTag("Character").transform;
        inventoryManager = player.GetComponent<InventoryManager>();
    }
    private void Start()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath + "storage.json");
        LoadStorage(filePath);
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            OpenStorage();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Character"))
        {
            CloseStorage();
        }
    }
    public void OpenStorage()
    {
        storage.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        prevButton.gameObject.SetActive(true);
        storageButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        pageText.text = (indexOfCurrentPage + 1).ToString() + " / " + pages.Count;
        pageText.gameObject.SetActive(true);
        pages[indexOfCurrentPage].gameObject.SetActive(true);
        inventoryManager.isStorageOpened = true;
        inventoryManager.statsText.gameObject.SetActive(true);
        inventoryManager.UpdateStatsText();
    }
    public void CloseStorage()
    {
        storage.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        storageButton.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        pageText.gameObject.SetActive(false);
        pages[indexOfCurrentPage].gameObject.SetActive(false);
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        indexOfCurrentPage = 0;
        slotIdClicked = -1;
        inventoryManager.isStorageOpened = false;
        inventoryManager.CloseInventory();
    }
    public void ShowInventory()
    {
        pageText.gameObject.SetActive(false);
        pages[indexOfCurrentPage].ClosePage();
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        slotIdClicked = -1;
        inventoryManager.OpenInventory();
    }
    public void ShowStorage()
    {
        inventoryManager.CloseInventory();
        OpenStorage();
    }
    public void ShowNextPage()
    {
        pages[indexOfCurrentPage].ClosePage();
        slotIdClicked = -1;
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        indexOfCurrentPage = (indexOfCurrentPage + 1) % pages.Count;
        pageText.text = (indexOfCurrentPage + 1).ToString() + " / " + pages.Count;
        pages[indexOfCurrentPage].OpenPage();
    }
    public void ShowPrevPage()
    {
        pages[indexOfCurrentPage].ClosePage();
        slotIdClicked = -1;
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        indexOfCurrentPage = (indexOfCurrentPage - 1 + pages.Count) % pages.Count;
        pageText.text = (indexOfCurrentPage + 1).ToString() + " / " + pages.Count;
        pages[indexOfCurrentPage].OpenPage();
    }
    public void TakeItem()
    {
        foreach (StorageSlot slot in pages[indexOfCurrentPage].slots)
        {
            if (slot.id == slotIdClicked)
            {
                bool inStorage = true;
                foreach (InventorySlot inventorySlot in inventoryManager.slots)
                {
                    if (inventorySlot.isEmpty)
                    {
                        inStorage = false;
                    }
                }
                if (!inStorage)
                {
                    inventoryManager.AddItem(slot.item, 1);
                    itemDescriptionText.text = "";
                    costText.text = "";
                    costText.gameObject.SetActive(false);
                    takeButton.gameObject.SetActive(false);
                    slot.NullifyData();
                }
                else
                {
                    // Debug.Log("UNLUCK");
                }
                return;
            }
        }
    }
    public bool AddItem(ItemScriptableObject item, int count, int pageId)
    {
        // foreach (StorageSlot slot in pages[pageId].slots)
        // {
        //     if (slot.item != null)
        //     {
        //         if (slot.item == item && slot.count + count <= slot.item.maxCount)
        //         {
        //             slot.count += count;
        //             slot.itemCountText.text = slot.count.ToString();
        //             return true;
        //         }
        //     }
        // }
        foreach (StorageSlot slot in pages[pageId].slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = item;
                slot.count = count;
                slot.isEmpty = false;
                slot.SetIcon(item.icon);
                if (item.maxCount > 1)
                {
                    // slot.itemCountText.text = count.ToString();
                }
                else 
                {
                    slot.itemCountText.text = "";
                }
                return true;
            }
        }
        return false;
    }
    public void SaveStorage(string filePath)
    {
        StorageData storageData = new StorageData();
        for (int i = 0; i < pages.Count; i++)
        {
            StoragePageData storagePageData = new StoragePageData();
            foreach (StorageSlot slot in pages[i].slots)
            {
                SlotData slotData = new()
                {
                    itemName = slot.item != null ? slot.item.name : null,
                    isEmpty = slot.isEmpty,
                    id = slot.id
                };
                storagePageData.slots.Add(slotData);
            }
            storageData.pages.Add(storagePageData);
        }
        string json = JsonUtility.ToJson(storageData);
        File.WriteAllText(filePath, json);
        // Debug.Log("Storage saved to: " + filePath);
    }
    public async void LoadStorage(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            StorageData storageData = JsonUtility.FromJson<StorageData>(json);
            for (int i = 0; i < storageData.pages.Count; i++)
            {
                StoragePageData storagePageData = storageData.pages[i];
                for (int j = 0; j < storagePageData.slots.Count; j++)
                {
                    StorageSlot slot = pages[i].slots[j];
                    SlotData slotData = storagePageData.slots[j];
                    if (!slotData.isEmpty)
                    {
                        slot.isEmpty = slotData.isEmpty;
                        slot.isClicked = false;
                        slot.id = slotData.id;
                        slot.count = 1;
                        int level = slotData.itemName.Last() - '0';
                        List<Item> items = await ItemsLoader.Instance.LoadAllItemsFromLevel(level);
                        foreach (Item item in items)
                        {
                            if (item.name == slotData.itemName)
                            {
                                slot.item = item.itemScriptableObject;
                                slot.SetIcon(item.itemScriptableObject.icon);
                                if (item.itemScriptableObject.maxCount > 1)
                                {
                                    // slot.itemCountText.text = slot.count.ToString();
                                }
                                else
                                {
                                    slot.itemCountText.text = "";
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        slot.NullifyData();
                    }
                }
            }
            // Debug.Log("Storage loaded from: " + filePath);
        }
        else
        {
            // Debug.Log("Save file not found at: " + filePath);
        }
    }
}
