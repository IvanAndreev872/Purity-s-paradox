using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject BG;
    public Transform inventory, inventoryPanel, equipSlotsPanel, player;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<InventorySlot> equipSlots = new List<InventorySlot>();
    public bool isOpened = false, isShopOpened = false, isStorageOpened = false;
    public TMP_Text itemDescriptionText, costText;
    public Button sellButton, storeButton;
    public PlayerStats playerStats;
    public int slotIdClicked = -1;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        inventoryPanel = inventory.GetChild(0).transform;
        equipSlotsPanel = inventory.GetChild(1).transform;
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                inventoryPanel.GetChild(i).GetComponent<InventorySlot>().Awake();
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
                slots[slots.Count - 1].id = i;
            }
        }
        for (int i = 0; i < equipSlotsPanel.childCount; i++)
        {
            if (equipSlotsPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                equipSlots.Add(equipSlotsPanel.GetChild(i).GetComponent<InventorySlot>());
                equipSlots[equipSlots.Count - 1].id = i + slots.Count;
            }
        }
        itemDescriptionText = inventory.GetChild(2).GetComponent<TMP_Text>();
        itemDescriptionText.text = "";
        costText = inventory.GetChild(3).GetComponent<TMP_Text>();
        costText.text = "";
        costText.gameObject.SetActive(false);
        sellButton = inventory.GetChild(4).GetComponent<Button>();
        sellButton.gameObject.SetActive(false);
        storeButton = inventory.GetChild(5).GetComponent<Button>();
        storeButton.gameObject.SetActive(false);
        BG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isShopOpened && !isStorageOpened)
        {
            if (isOpened)
            {
                Time.timeScale = 1f;
                CloseInventory();
            }
            else
            {
                Time.timeScale = 0f;
                OpenInventory();
            }
        }
    }
    public void OpenInventory()
    {
        isOpened = true;
        BG.SetActive(true);
        inventoryPanel.gameObject.SetActive(true);
    }
    public void CloseInventory()
    {
        isOpened = false;
        BG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        slotIdClicked = -1;
        sellButton.gameObject.SetActive(false);
        storeButton.gameObject.SetActive(false);
    }
    public void SellItem()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.id == slotIdClicked)
            {
                int gain = slot.item.cost * slot.item.sellCoefficient / 100;
                playerStats.money += gain;
                slot.NullifyData();
                itemDescriptionText.text = "";
                costText.text = "";
                costText.gameObject.SetActive(false);
                sellButton.gameObject.SetActive(false);
                slotIdClicked = -1;
                return;
            }
        }
    }
    public void StoreItem()
    {
        StorageManager storage = FindObjectOfType<StorageManager>();
        foreach (InventorySlot slot in slots)
        {
            if (slot.id == slotIdClicked)
            {
                bool ok = true;
                int pageId = 0;
                while (pageId < storage.pages.Count && !storage.AddItem(slot.item, 1, pageId))
                {
                    pageId++;
                }
                if (pageId == storage.pages.Count)
                {
                    ok = false;
                }
                if (!ok)
                {
                    return;
                }
                slot.NullifyData();
                itemDescriptionText.text = "";
                costText.text = "";
                costText.gameObject.SetActive(false);
                storeButton.gameObject.SetActive(false);
                slotIdClicked = -1;
                return;
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            AddItem(other.GetComponent<Item>().itemScriptableObject, other.GetComponent<Item>().count);
            Destroy(other.gameObject);
        }
    }
    public void AddItem(ItemScriptableObject item, int count)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item != null)
            {
                if (slot.item == item && slot.count + count <= slot.item.maxCount)
                {
                    slot.count += count;
                    slot.itemCountText.text = slot.count.ToString();
                    return;
                }
            }
        }
        foreach (InventorySlot slot in slots)
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
                return;
            }
        }
    }
}
