using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject BG;
    public Transform shop;
    public int indexOfCurrentPage = 0;
    public List<ShopPage> pages = new List<ShopPage>();
    public TMP_Text itemDescriptionText, costText;
    public Button buyButton, nextButton, prevButton, shopButton, inventoryButton;
    public int slotIdClicked = -1;
    public Transform player;
    public InventoryManager inventoryManager;
    public PlayerStats playerStats;
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
        shop.gameObject.SetActive(false);
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
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventoryManager = player.GetComponent<InventoryManager>();
        playerStats = player.GetComponent<PlayerStats>();
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
    }
    public void CloseShop()
    {
        shop.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
        prevButton.gameObject.SetActive(false);
        shopButton.gameObject.SetActive(false);
        inventoryButton.gameObject.SetActive(false);
        BG.SetActive(false);
        pages[indexOfCurrentPage].gameObject.SetActive(false);
        itemDescriptionText.text = "";
        costText.text = "";
        costText.gameObject.SetActive(false);
        indexOfCurrentPage = 0;
        slotIdClicked = -1;
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
        indexOfCurrentPage = (indexOfCurrentPage + 1) % pages.Count;
        pages[indexOfCurrentPage].ShowPage();
    }
    public void ShowPrevPage()
    {
        pages[indexOfCurrentPage].ClosePage();
        indexOfCurrentPage = (indexOfCurrentPage - 1 + pages.Count) % pages.Count;
        pages[indexOfCurrentPage].ShowPage();
    }
    public void BuyItem()
    {
        foreach (ShopSlot slot in pages[indexOfCurrentPage].slots)
        {
            if (slot.id == slotIdClicked)
            {
                if (slot.item.cost <= playerStats.money)
                {
                    playerStats.money -= slot.item.cost;
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
                    }
                    else
                    {
                        Debug.Log("UNLUCK");
                    }
                    itemDescriptionText.text = "";
                    costText.text = "";
                    costText.gameObject.SetActive(false);
                    buyButton.gameObject.SetActive(false);
                    slot.NullifyData();
                }
                return;
            }
        }
    }
    public void AddItem(ItemScriptableObject item)
    {
        foreach (ShopSlot slot in pages[indexOfCurrentPage].slots)
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
