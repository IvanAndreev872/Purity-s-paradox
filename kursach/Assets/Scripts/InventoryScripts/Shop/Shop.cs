using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Linq;
using System.Threading.Tasks;

public class Shop : MonoBehaviour
{
    public GameObject BG;
    public Transform shop;
    public int indexOfCurrentPage = 0;
    public List<ShopPage> pages = new List<ShopPage>();
    public TMP_Text itemDescriptionText, costText;
    public Button buyButton, nextButton, prevButton, shopButton, inventoryButton;
    public int slotIdClicked = -1;
    public InventoryManager inventoryManager;
    public PlayerStats playerStats;
    public UiManager uiManager;
    public StorageManager storage;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CloseShop();
        }
    }
    public async void Start()
    {
        for (int level = 1; level <= 5; level++)
        {
            List<Item> items = await ItemsLoader.Instance.LoadAllItemsFromLevel(level);
            for (int i = 0; i < items.Count; i++)
            {
                AddItem(items[i].itemScriptableObject,  items[i].name.Last() - '1');
            }
        }
    }
    public void Awake()
    {
        for (int i = 0; i < shop.childCount; i++)
        {
            if (shop.GetChild(i).GetComponent<ShopPage>() != null)
            {
                pages.Add(shop.GetChild(i).GetComponent<ShopPage>());
            }
        }
        BG.SetActive(false);
        shop.gameObject.SetActive(true);
        itemDescriptionText = shop.GetChild(5).GetComponent<TMP_Text>();
        itemDescriptionText.text = "";
        buyButton = shop.GetChild(6).GetComponent<Button>();
        buyButton.gameObject.SetActive(false);
        prevButton = shop.GetChild(7).GetComponent<Button>();
        prevButton.gameObject.SetActive(false);
        nextButton = shop.GetChild(8).GetComponent<Button>();
        nextButton.gameObject.SetActive(false);
        costText = shop.GetChild(9).GetComponent<TMP_Text>();
        costText.text = "";
        costText.gameObject.SetActive(false);
        shopButton = shop.GetChild(10).GetComponent<Button>();
        shopButton.gameObject.SetActive(false);
        inventoryButton = shop.GetChild(11).GetComponent<Button>();
        inventoryButton.gameObject.SetActive(false);
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;
        inventoryManager = player.GetComponent<InventoryManager>();
        playerStats = player.GetComponent<PlayerStats>();
        uiManager = player.GetComponent<UiManager>();
        storage = FindObjectOfType<StorageManager>();
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OpenShop();
        }
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CloseShop();
        }
    }
    public void OpenShop()
    {
        shop.gameObject.SetActive(true);
        nextButton.gameObject.SetActive(true);
        prevButton.gameObject.SetActive(true);
        shopButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        BG.SetActive(true);
        pages[indexOfCurrentPage].gameObject.SetActive(true);
        inventoryManager.isShopOpened = true;
        inventoryManager.statsText.gameObject.SetActive(true);
        inventoryManager.UpdateStatsText();
    }
    public void CloseShop()
    {
        shop.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        BG.SetActive(false);
        pages[indexOfCurrentPage].ClosePage();
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        indexOfCurrentPage = 0;
        slotIdClicked = -1;
        inventoryManager.isShopOpened = false;
        inventoryManager.CloseInventory();
    }
    public void ShowInventory()
    {
        CloseShop();
        shop.gameObject.SetActive(true);
        shopButton.gameObject.SetActive(true);
        inventoryButton.gameObject.SetActive(true);
        inventoryManager.isShopOpened = true;
        inventoryManager.OpenInventory();
    }
    public void ShowShop()
    {
        inventoryManager.CloseInventory();
        OpenShop();
    }
    public void ShowNextPage()
    {
        pages[indexOfCurrentPage].ClosePage();
        slotIdClicked = -1;
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        buyButton.gameObject.SetActive(false);
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
        buyButton.gameObject.SetActive(false);
        indexOfCurrentPage = (indexOfCurrentPage - 1 + pages.Count) % pages.Count;
        pages[indexOfCurrentPage].OpenPage();
    }
    public void BuyItem()
    {
        foreach (ShopSlot slot in pages[indexOfCurrentPage].slots)
        {
            if (slot.id == slotIdClicked)
            {
                if (slot.item.cost <= playerStats.money)
                {
                    bool inStorage = true, ok = true;
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
                    }
                    else
                    {
                        int pageId = 0;
                        while (pageId < storage.pages.Count && !storage.AddItem(slot.item, 1, pageId))
                        {
                            pageId++;
                        }
                        if (pageId == storage.pages.Count)
                        {
                            ok = false;
                        }
                    }
                    if (!ok)
                    {
                        return;
                    }
                    playerStats.money -= slot.item.cost;
                    uiManager.UpdateUI();
                    itemDescriptionText.text = "";
                    costText.text = "";
                    costText.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);
                    slotIdClicked = -1;
                }
                return;
            }
        }
    }
    private void AddItem(ItemScriptableObject item, int pageId)
    {
        foreach (ShopSlot slot in pages[pageId].slots)
        {
            if (slot.isEmpty)
            {
                slot.item = item;
                slot.SetIcon(item.icon);
                slot.itemCostText.text = item.cost.ToString();
                slot.isEmpty = false;
                return;
            }
        }
    }
}
