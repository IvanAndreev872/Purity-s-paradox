using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using Unity.VisualScripting;

public class Chest : MonoBehaviour
{
    public GameObject BG;
    public Transform chest, chestPanel;
    private const int slotsCount = 12;
    public List<ChestSlot> slots = new List<ChestSlot>();
    public List<ItemScriptableObject> items = new List<ItemScriptableObject>();
    public TMP_Text itemDescriptionText, costText, pressButtonText;
    public Button takeButton, chestButton, inventoryButton;
    public int slotIdClicked = -1;
    public InventoryManager inventoryManager;
    public bool hasOpened = false, inTrigger = false;
    public void Update()
    {
        if (inTrigger)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (pressButtonText.gameObject.activeSelf)
                {
                    Time.timeScale = 0f;
                    OpenChest();
                }
                else
                {
                    Time.timeScale = 1f;
                    CloseChest();
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseChest();
            }
        }
    }
    public void Awake()
    {
        chest = GameObject.FindGameObjectWithTag("Chest canvas").transform.GetChild(0);
        chestPanel = chest.GetChild(0);
        for (int i = 0; i < chestPanel.childCount; i++)
        {
            if (chestPanel.GetChild(i).GetComponent<ChestSlot>() != null)
            {
                items.Add(null);
                chestPanel.GetChild(i).GetComponent<ChestSlot>().Awake();
                slots.Add(chestPanel.GetChild(i).GetComponent<ChestSlot>());
                slots[slots.Count - 1].id = i;
            }
        }
        chestPanel.gameObject.SetActive(false);
        itemDescriptionText = chest.GetChild(1).GetComponent<TMP_Text>();
        itemDescriptionText.text = "";
        takeButton = chest.GetChild(2).GetComponent<Button>();
        takeButton.gameObject.SetActive(false);
        costText = chest.GetChild(3).GetComponent<TMP_Text>();
        costText.text = "";
        costText.gameObject.SetActive(false);
        inventoryButton = chest.GetChild(4).GetComponent<Button>();
        inventoryButton.gameObject.SetActive(false);
        chestButton = chest.GetChild(5).GetComponent<Button>();
        chestButton.gameObject.SetActive(false);
        pressButtonText = chest.GetChild(6).GetComponent<TMP_Text>();
        pressButtonText.text = "";
        pressButtonText.gameObject.SetActive(false);
        Transform player = GameObject.FindGameObjectWithTag("Character").transform;
        inventoryManager = player.GetComponent<InventoryManager>();
        BG = inventoryManager.BG;
    }
    private void AddButtonsListeners()
    {
        chestButton.onClick.AddListener(() => ShowChest());
        inventoryButton.onClick.AddListener(() => ShowInventory());
        takeButton.onClick.AddListener(() => TakeItem());
    }
    private void ShowPressButtonText()
    {
        pressButtonText.text = "Press E to open the chest";
        pressButtonText.gameObject.SetActive(true);
    }
    private void FillChestSlots()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            if (items[i] != null)
            {
                slots[i].item = items[i];
                slots[i].count = 1;
                slots[i].isEmpty = false;
                slots[i].SetIcon(slots[i].item.icon);
                if (slots[i].item.maxCount > 1)
                {
                    // slot.itemCountText.text = count.ToString();
                }
                else 
                {
                    slots[i].itemCountText.text = "";
                }
            }
            else
            {
                slots[i].NullifyData();
            }
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        inTrigger = true;
        if (other.CompareTag("Character"))
        {
            AddButtonsListeners();
            ShowPressButtonText();
            FillChestSlots();
        }
    }
    private void RemoveButtonsListeners()
    {
        chestButton.onClick.RemoveAllListeners();
        inventoryButton.onClick.RemoveAllListeners();
        takeButton.onClick.RemoveAllListeners();
    }
    public void HidePressButtonText()
    {
        pressButtonText.text = "";
        pressButtonText.gameObject.SetActive(false);
    }
    private void NullifyChestSlots()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            slots[i].NullifyData();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        inTrigger = false;
        if (other.CompareTag("Character"))
        {
            RemoveButtonsListeners();
            HidePressButtonText();
            NullifyChestSlots();
        }
    }
    public async void OpenChest()
    {
        pressButtonText.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(true);
        chestButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        BG.SetActive(true);
        inventoryManager.statsText.gameObject.SetActive(true);
        inventoryManager.UpdateStatsText();
        inventoryManager.chest = this;
        inventoryManager.isChestOpened = true;
        if (!hasOpened)
        {
            hasOpened = true;
            // logic of loading items in the chest
            // string name = SceneManager.GetActiveScene().name;
            List<Item> itemsChest = await ItemsLoader.Instance.LoadAllItemsFromLevel(5);
            for (int i = 0; i < itemsChest.Count; i++)
            {
                AddItem(itemsChest[i].itemScriptableObject, 1);
            }
        }
        else
        {

        }
    }
    public void CloseChest()
    {
        pressButtonText.gameObject.SetActive(true);
        chestPanel.gameObject.SetActive(false);
        chestButton.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        BG.SetActive(false);
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        slotIdClicked = -1;
        inventoryManager.chest = null;
        inventoryManager.isOpened = false;
        inventoryManager.isChestOpened = false;
        inventoryManager.CloseInventory();
    }
    public void ShowInventory()
    {
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        slotIdClicked = -1;
        takeButton.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(false);
        inventoryManager.OpenInventory();
    }
    public void ShowChest()
    {
        inventoryManager.CloseInventory();
        OpenChest();
    }
    public void TakeItem()
    {
        for (int i = 0; i < slotsCount; i++)
        {
            ChestSlot slot = slots[i];
            if (slot.id == slotIdClicked)
            {
                bool inChest = true;
                foreach (InventorySlot inventorySlot in inventoryManager.slots)
                {
                    if (inventorySlot.isEmpty)
                    {
                        inChest = false;
                    }
                }
                if (!inChest)
                {
                    items[i] = null;
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
    public bool AddItem(ItemScriptableObject item, int count)
    {
        // foreach (chestSlot slot in pages[pageId].slots)
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
        for (int i = 0; i < slotsCount; i++)
        {
            if (slots[i].isEmpty)
            {
                items[i] = item;
                slots[i].item = item;
                slots[i].count = count;
                slots[i].isEmpty = false;
                slots[i].SetIcon(item.icon);
                if (item.maxCount > 1)
                {
                    // slot.itemCountText.text = count.ToString();
                }
                else 
                {
                    slots[i].itemCountText.text = "";
                }
                return true;
            }
        }
        return false;
    }
}
