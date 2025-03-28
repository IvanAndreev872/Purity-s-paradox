using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject BG;
    public Transform inventory, inventoryPanel, equipSlotsPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<InventorySlot> equipSlots = new List<InventorySlot>();
    public bool isOpened = false, isShopOpened = false, isStorageOpened = false;
    public TMP_Text itemDescriptionText, costText, statsText;
    public Button sellButton, storeButton;
    public PlayerStats playerStats;
    public UiManager uiManager;
    public int slotIdClicked = -1;
    void Awake()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        playerStats = player.GetComponent<PlayerStats>();
        uiManager = player.GetComponent<UiManager>();
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
        statsText = inventory.GetChild(6).GetComponent<TMP_Text>();
        statsText.text = "";
        statsText.gameObject.SetActive(false);
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
        string stats = "Health: " + playerStats.health.ToString() + "\n" +
                       "Max Health: " + playerStats.maxHealth.ToString() + "\n" +
                       "\n" +
                       "Recklessness: " + playerStats.money.ToString() + "\n" +
                       "\n" +
                       "Sword Damage: " +
                       (playerStats.isSwordEquipped > 0 ? playerStats.swordDamage.ToString() : "0") + "\n" +
                       ((playerStats.isSwordEquipped > 0 && playerStats.poisonSwordDamagePerSec > 0) ? 
                       ("Sword Poison Damage Per Sec: " + playerStats.poisonSwordDamagePerSec.ToString() + "\n") : "") +
                       ((playerStats.isSwordEquipped > 0 && playerStats.fireSwordDamagePerSec > 0) ? 
                       ("Sword Fire Damage Per Sec: " + playerStats.fireSwordDamagePerSec.ToString() + "\n") : "") +
                       ((playerStats.isSwordEquipped > 0 && playerStats.bleedingSwordDamagePerSec > 0) ? 
                       ("Sword Bleeding Damage Per Sec: " + playerStats.bleedingSwordDamagePerSec.ToString() + "\n") : "") +
                       ((playerStats.isSwordEquipped > 0 && playerStats.freezeSwordCoefficient > 1) ? 
                       ("Sword Freeze Power: " + ((playerStats.freezeSwordCoefficient - 1) * 100).ToString() + "%" + "\n") : "") +
                       "\n" +
                       "Staff Damage: " +
                       (playerStats.isStaffEquipped > 0 ? playerStats.staffDamage.ToString() : "0") + "\n" +
                       ((playerStats.isStaffEquipped > 0 && playerStats.poisonStaffDamagePerSec > 0) ? 
                       ("Staff Poison Damage Per Sec: " + playerStats.poisonStaffDamagePerSec.ToString() + "\n") : "") +
                       ((playerStats.isStaffEquipped > 0 && playerStats.fireStaffDamagePerSec > 0) ? 
                       ("Staff Fire Damage Per Sec: " + playerStats.fireStaffDamagePerSec.ToString() + "\n") : "") +
                       ((playerStats.isStaffEquipped > 0 && playerStats.freezeStaffCoefficient > 1) ? 
                       ("Staff Freeze Power: " + ((playerStats.freezeStaffCoefficient - 1) * 100).ToString() + "%" + "\n") : "") +
                       "\n" +
                       "Walk Speed: " + (playerStats.walkSpeed * playerStats.walkAccelerateCoefficient).ToString() + "\n" +
                       "Dash Speed: " + (playerStats.dashSpeed * playerStats.dashAccelerateCoefficient).ToString() + "\n" +
                       "Dash Duration: " + playerStats.dashDuration.ToString() + "\n" +
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
                uiManager.UpdateUI();
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
            Item item = other.GetComponent<Item>();
            if (AddItem(item.itemScriptableObject, item.count))
            {
                Destroy(other.gameObject);
            }
        }
    }
    public bool AddItem(ItemScriptableObject item, int count)
    {
        foreach (InventorySlot slot in slots)
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
                return true;
            }
        }
        return false;
    }
}
