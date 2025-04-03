using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject BG;
    public Transform inventory, inventoryPanel, equipSlotsPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<InventorySlot> equipSlots = new List<InventorySlot>();
    public bool isOpened = false, isShopOpened = false, isStorageOpened = false, isChestOpened = false;
    public TMP_Text itemDescriptionText, costText, statsText;
    public Button sellButton, storeButton;
    public PlayerStats playerStats;
    public int slotIdClicked = -1;
    public Chest chest;
    public void Awake()
    {
        Transform player = GameObject.FindGameObjectWithTag("Character").transform;
        playerStats = player.GetComponent<PlayerStats>();
        BG = inventory.GetChild(0).gameObject;
        inventoryPanel = inventory.GetChild(1).transform;
        equipSlotsPanel = inventory.GetChild(2).transform;
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
        itemDescriptionText = inventory.GetChild(3).GetComponent<TMP_Text>();
        itemDescriptionText.text = "";
        costText = inventory.GetChild(4).GetComponent<TMP_Text>();
        costText.text = "";
        costText.gameObject.SetActive(false);
        sellButton = inventory.GetChild(5).GetComponent<Button>();
        sellButton.gameObject.SetActive(false);
        storeButton = inventory.GetChild(6).GetComponent<Button>();
        storeButton.gameObject.SetActive(false);
        statsText = inventory.GetChild(7).GetComponent<TMP_Text>();
        statsText.text = "";
        statsText.gameObject.SetActive(false);
        BG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
    }
    private void Start()
    {
        string filePath = Application.streamingAssetsPath + "/inventory.json";
        LoadInventory(filePath);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && !isShopOpened && !isStorageOpened && !isChestOpened)
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
        statsText.gameObject.SetActive(true);
        UpdateStatsText();
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
        statsText.text = "";
        statsText.gameObject.SetActive(false);
    }
    public void UpdateStatsText()
    {
        string stats = "Health: " + Math.Round(playerStats.health, 1) + "\n" +
                       "Max Health: " + Math.Round(playerStats.maxHealth, 1) + "\n" +
                       "\n" +
                       "Recklessness: " + playerStats.money + "\n" +
                       "\n" +
                       "Sword Damage: " +
                       (playerStats.isSwordEquipped > 0 ? playerStats.swordDamage : "0") + "\n" +
                       "Sword Radius: " + 
                       (playerStats.isSwordEquipped > 0 ? playerStats.swordRadius : "0") + "\n" +
                       ((playerStats.isSwordPoisoned > 0) ? "Poison After Sword Attack: ON\n" : "") +
                       ((playerStats.isSwordBleeding > 0) ? "Bleeding After Sword Attack: ON\n" : "") +
                       ((playerStats.isSwordFired > 0) ? "Fire After Sword Attack: ON\n" : "") +
                       ((playerStats.isSwordFreezed > 0) ?
                       "Sword Freeze Power: " + Math.Round(playerStats.swordSlowdownCoeffitient - 1, 2) * 100 : "") + "\n" +

                       "Staff Damage: " +
                       (playerStats.isStaffEquipped > 0 ? playerStats.staffDamage : "0") + "\n" +
                       "Staff Range: " + 
                       (playerStats.isStaffEquipped > 0 ? playerStats.staffRange : "0") + "\n" +
                       ((playerStats.isStaffPoisoned > 0) ? "Poison After Staff Attack: ON\n" : "") +
                       ((playerStats.isStaffFired > 0) ? "Fire After Staff Attack: ON\n" : "") +
                       ((playerStats.isStaffFreezed > 0) ?
                       "Staff Freeze Power: " + Math.Round(playerStats.staffSlowdownCoeffitient - 1, 2) * 100 : "") + "\n" +
                       "Walk Speed: " + Math.Round(playerStats.walkSpeed, 1) + "\n" +
                       "Dash Speed: " + Math.Round(playerStats.dashSpeed, 1) + "\n" +
                       "Dash Duration: " + Math.Round(playerStats.dashDuration, 1) + "\n" +
                       ((playerStats.isDashPoisoned > 0) ? "Poisoned Dash: ON" + "\n" : "") +
                       ((playerStats.isDashInvulnerable > 0) ? "Invulnerable Dash: ON" + "\n" : "")
                       ;
        statsText.text = stats;
    }
    public void SellItem()
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.id == slotIdClicked)
            {
                int gain = slot.item.cost * slot.item.sellCoefficient / 100;
                playerStats.money += gain;
                playerStats.UpdateUI();
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
        if (isStorageOpened) {
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
        else
        {
            foreach (InventorySlot slot in slots)
            {
                if (slot.id == slotIdClicked)
                {
                    bool ok = chest.AddItem(slot.item, 1);
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
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Item item = other.GetComponent<Item>();
            if (AddItem(item.itemScriptableObject, item.count))
            {
                Destroy(other.gameObject);
            }
        }
    }
    public bool AddItem(ItemScriptableObject item, int count)
    {
        // foreach (InventorySlot slot in slots)
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
    public void SaveInventory(string filePath)
    {
        InventoryData inventoryData = new InventoryData();
        foreach (InventorySlot slot in slots)
        {
            SlotData slotData = new()
            {
                itemName = slot.item != null ? slot.item.name : null,
                isEmpty = slot.isEmpty,
                id = slot.id
            };
            inventoryData.slots.Add(slotData);
        }
        foreach (InventorySlot slot in equipSlots)
        {
            SlotData slotData = new()
            {
                itemName = slot.item != null ? slot.item.name : null,
                isEmpty = slot.isEmpty,
                id = slot.id
            };
            inventoryData.slots.Add(slotData);
        }
        string json = JsonUtility.ToJson(inventoryData);
        File.WriteAllText(filePath, json);
        Debug.Log("Inventory saved to: " + filePath);
    }
    public async void LoadInventory(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);
            for (int i = 0; i < inventoryData.slots.Count; i++)
            {
                InventorySlot slot;
                if (i >= 24)
                {
                    slot = equipSlots[i - 24];
                }
                else
                {
                    slot = slots[i];
                }
                SlotData slotData = inventoryData.slots[i];
                if (!slotData.isEmpty)
                {
                    slot.isEmpty = slotData.isEmpty;
                    slot.isClicked = false;
                    slot.id = slotData.id;
                    slot.panel = slot.id >= 24 ? 1 : 0;
                    slot.count = 1;
                    int level = slotData.itemName.Last() - '0';
                    List<Item> items = await ItemsLoader.Instance.LoadAllItemsFromLevel(level);
                    foreach (Item item in items)
                    {
                        if (item.name == slotData.itemName)
                        {
                            slot.item = item.itemScriptableObject;
                            if (slot.panel == 1)
                            {
                                if (slot.item is AbilityItem abilityItem && slot.item is not ArmorItem armorItem)
                                {
                                    abilityItem.DiscardEffects(playerStats);
                                    abilityItem.ApplyEffects(playerStats);
                                }
                                if (slot.item is WeaponItem weaponItem)
                                {
                                    weaponItem.DiscardEffects(playerStats);
                                    weaponItem.ApplyEffects(playerStats);
                                }
                            }
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
            Debug.Log("Inventory loaded from: " + filePath);
        }
        else
        {
            Debug.Log("Save file not found at: " + filePath);
        }
    }
}
