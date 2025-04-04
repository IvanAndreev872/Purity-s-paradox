using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Threading.Tasks;

public class Chest : MonoBehaviour
{
    public Transform chest, chestPanel;
    private const int slotsCount = 12;
    public List<ChestSlot> slots = new List<ChestSlot>();
    public List<ItemScriptableObject> items = new List<ItemScriptableObject>();
    public TMP_Text itemDescriptionText, costText, pressButtonText;
    public Button takeButton, chestButton, inventoryButton;
    public int slotIdClicked = -1;
    public InventoryManager inventoryManager;
    public bool hasOpened = false, inTrigger = false, isFarm = false;
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
    }
    private void AddButtonsListeners()
    {
        chestButton.onClick.AddListener(() => ShowChest());
        inventoryButton.onClick.AddListener(() => ShowInventory());
        takeButton.onClick.AddListener(() => TakeItem());
    }
    private void ShowPressButtonText()
    {
        pressButtonText.text = "Press E To Open The Chest";
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
        if (other.CompareTag("Character"))
        {
            inTrigger = true;
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
        if (other.CompareTag("Character"))
        {
            inTrigger = false;
            RemoveButtonsListeners();
            HidePressButtonText();
            NullifyChestSlots();
        }
    }
    private async Task<List<Item>> GenerateItems(int level)
    {
        List<Item> res = new List<Item>();
        if (level == 1)
        {
            int countOfItems = Random.Range(1, 3);
            List<Item> list1 = await ItemsLoader.Instance.LoadAllItemsFromLevel(1);
            for (int i = 0; i < countOfItems; i++)
            {
                int index = Random.Range(0, list1.Count - 1);
                res.Add(list1[index]);
            }
        }
        else if (level == 2)
        {
            int countOfItems = Random.Range(1, 3);
            List<Item> list1 = await ItemsLoader.Instance.LoadAllItemsFromLevel(1);
            List<Item> list2 = await ItemsLoader.Instance.LoadAllItemsFromLevel(2);
            for (int i = 0; i < countOfItems; i++)
            {
                int rnd = Random.Range(1, 100);
                if (rnd <= 20)
                {
                    int index = Random.Range(0, list1.Count - 1);
                    res.Add(list1[index]);
                }
                else
                {
                    int index = Random.Range(0, list2.Count - 1);
                    res.Add(list2[index]);
                }
            }
        }
        else if (level == 3)
        {
            int countOfItems = Random.Range(1, 3);
            List<Item> list1 = await ItemsLoader.Instance.LoadAllItemsFromLevel(1);
            List<Item> list2 = await ItemsLoader.Instance.LoadAllItemsFromLevel(2);
            List<Item> list3 = await ItemsLoader.Instance.LoadAllItemsFromLevel(3);
            for (int i = 0; i < countOfItems; i++)
            {
                int rnd = Random.Range(1, 100);
                if (rnd <= 10)
                {
                    int index = Random.Range(0, list1.Count - 1);
                    res.Add(list1[index]);
                }
                else if (rnd <= 50)
                {
                    int index = Random.Range(0, list2.Count - 1);
                    res.Add(list2[index]);
                }
                else
                {
                    int index = Random.Range(0, list3.Count - 1);
                    res.Add(list3[index]);
                }
            }
        }
        else if (level == 4)
        {
            int countOfItems = Random.Range(1, 3);
            List<Item> list1 = await ItemsLoader.Instance.LoadAllItemsFromLevel(1);
            List<Item> list2 = await ItemsLoader.Instance.LoadAllItemsFromLevel(2);
            List<Item> list3 = await ItemsLoader.Instance.LoadAllItemsFromLevel(3);
            List<Item> list4 = await ItemsLoader.Instance.LoadAllItemsFromLevel(4);
            for (int i = 0; i < countOfItems; i++)
            {
                int rnd = Random.Range(1, 100);
                if (rnd <= 5)
                {
                    int index = Random.Range(0, list1.Count - 1);
                    res.Add(list1[index]);
                }
                else if (rnd <= 25)
                {
                    int index = Random.Range(0, list2.Count - 1);
                    res.Add(list2[index]);
                }
                else if (rnd <= 70)
                {
                    int index = Random.Range(0, list3.Count - 1);
                    res.Add(list3[index]);
                }
                else 
                {
                    int index = Random.Range(0, list4.Count - 1);
                    res.Add(list4[index]);
                }
            }
        }
        else if (level == 5)
        {
            int countOfItems = Random.Range(1, 3);
            List<Item> list1 = await ItemsLoader.Instance.LoadAllItemsFromLevel(1);
            List<Item> list2 = await ItemsLoader.Instance.LoadAllItemsFromLevel(2);
            List<Item> list3 = await ItemsLoader.Instance.LoadAllItemsFromLevel(3);
            List<Item> list4 = await ItemsLoader.Instance.LoadAllItemsFromLevel(4);
            List<Item> list5 = await ItemsLoader.Instance.LoadAllItemsFromLevel(5);
            for (int i = 0; i < countOfItems; i++)
            {
                int rnd = Random.Range(1, 100);
                if (rnd <= 3)
                {
                    int index = Random.Range(0, list1.Count - 1);
                    res.Add(list1[index]);
                }
                else if (rnd <= 10)
                {
                    int index = Random.Range(0, list2.Count - 1);
                    res.Add(list2[index]);
                }
                else if (rnd <= 30)
                {
                    int index = Random.Range(0, list3.Count - 1);
                    res.Add(list3[index]);
                }
                else if (rnd <= 85)
                {
                    int index = Random.Range(0, list4.Count - 1);
                    res.Add(list4[index]);
                }
                else
                {
                    int index = Random.Range(0, list5.Count - 1);
                    res.Add(list5[index]);
                }
            }
        }
        return res;
    }
    public async void OpenChest()
    {
        pressButtonText.gameObject.SetActive(false);
        chestPanel.gameObject.SetActive(true);
        chestButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        inventoryManager.statsText.gameObject.SetActive(true);
        inventoryManager.UpdateStatsText();
        inventoryManager.chest = this;
        inventoryManager.isChestOpened = true;
        if (!hasOpened)
        {
            hasOpened = true;
            // logic of loading items in the chest
            // level 1 - 100% drop 1st level item
            // level 2 - 80% drop 2nd level and 20% 1st
            // level 3 - 50% drop 3rd level and 40% 2nd, 10% for 1st
            // level 4 - 30% drop 4th level and 45% 3rd, 20% 2nd, 5% 1st
            // level 5 - 15% drop 5th level and 55% 4th, 20% 3rd, 7% 2nd, 3% 1st
            List<Item> itemsChest;
            if (isFarm)
            {
                PlayerStats playerStats = GameObject.FindGameObjectWithTag("Character").transform.GetComponent<PlayerStats>();
                itemsChest = await GenerateItems(playerStats.levelCompleted);
            }
            else
            {
                string name = SceneManager.GetActiveScene().name;
                itemsChest = await GenerateItems(name.Last() - '0');
            }
            for (int i = 0; i < itemsChest.Count; i++)
            {
                AddItem(itemsChest[i].itemScriptableObject, 1);
            }
        }
    }
    public void CloseChest()
    {
        pressButtonText.gameObject.SetActive(true);
        chestPanel.gameObject.SetActive(false);
        chestButton.gameObject.SetActive(false);
        takeButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
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
