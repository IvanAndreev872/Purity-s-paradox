using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StorageManager : MonoBehaviour
{
    public GameObject BG;
    public Transform storage;
    public int indexOfCurrentPage = 0;
    public List<StoragePage> pages = new List<StoragePage>();
    public TMP_Text itemDescriptionText, costText;
    public Button takeButton, nextButton, prevButton, storageButton, inventoryButton;
    public int slotIdClicked = -1;
    public Transform player;
    public InventoryManager inventoryManager;
    public void Awake()
    {
        for (int i = 0; i < storage.childCount; i++)
        {
            if (storage.GetChild(i).GetComponent<StoragePage>() != null)
            {
                pages.Add(storage.GetChild(i).GetComponent<StoragePage>());
            }
        }
        BG.SetActive(false);
        storage.gameObject.SetActive(false);
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventoryManager = player.GetComponent<InventoryManager>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenStorage();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
        BG.SetActive(true);
        pages[indexOfCurrentPage].gameObject.SetActive(true);
        inventoryManager.isStorageOpened = true;
    }
    public void CloseStorage()
    {
        storage.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        storageButton.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        BG.SetActive(false);
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
        CloseStorage();
        storage.gameObject.SetActive(true);
        storageButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        inventoryManager.isStorageOpened = true;
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
                    Debug.Log("UNLUCK");
                }
                return;
            }
        }
    }
    public bool AddItem(ItemScriptableObject item, int count, int pageId)
    {
        Debug.Log(pageId);
        foreach (StorageSlot slot in pages[pageId].slots)
        {
            if (slot.item != null)
            {
                if (slot.item == item && slot.count + count <= slot.item.maxCount)
                {
                    slot.count += count;
                    slot.itemCountText.text = slot.count.ToString();
                    return true;
                }
            }
        }
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
                    slot.itemCountText.text = count.ToString();
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
}
