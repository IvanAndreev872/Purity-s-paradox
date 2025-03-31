using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    public Transform player;
    public PlayerStats playerStats;
    public InventoryManager inventoryManager;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Character").transform;
        playerStats = player.GetComponent<PlayerStats>();
        inventoryManager = player.GetComponent<InventoryManager>();
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!oldSlot.isEmpty && inventoryManager.isOpened)
        {
            GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!oldSlot.isEmpty && inventoryManager.isOpened)
        {
            GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
            GetComponentInChildren<Image>().raycastTarget = false;
            transform.SetParent(transform.parent.parent.parent);
        }
        if (!oldSlot.isEmpty && inventoryManager.isOpened)
        {
            if (inventoryManager.slotIdClicked == oldSlot.id && oldSlot.isClicked)
            {
                inventoryManager.itemDescriptionText.text = "";
                inventoryManager.costText.text = "";
                inventoryManager.costText.gameObject.SetActive(false);
                oldSlot.isClicked = false;
                if (inventoryManager.isShopOpened)
                {
                    inventoryManager.sellButton.gameObject.SetActive(false);
                }
                if (inventoryManager.isStorageOpened)
                {
                    inventoryManager.storeButton.gameObject.SetActive(false);
                }
            }
            else
            {
                inventoryManager.itemDescriptionText.text = oldSlot.item.itemName + "\n\n" + oldSlot.item.itemDescription;
                int gain = oldSlot.item.cost * oldSlot.item.sellCoefficient / 100;
                inventoryManager.costText.text = gain.ToString();
                inventoryManager.costText.gameObject.SetActive(true);
                oldSlot.isClicked = true;
                if (inventoryManager.isShopOpened)
                {
                    if (oldSlot.id < 24)
                    {
                        inventoryManager.sellButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        inventoryManager.sellButton.gameObject.SetActive(false);
                    }
                }
                if (inventoryManager.isStorageOpened)
                {
                    if (oldSlot.id < 24)
                    {
                        inventoryManager.storeButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        inventoryManager.storeButton.gameObject.SetActive(false);
                    }
                }
            }
            inventoryManager.slotIdClicked = oldSlot.id;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!oldSlot.isEmpty && inventoryManager.isOpened)
        {
            GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
            GetComponentInChildren<Image>().raycastTarget = true;
            transform.SetParent(oldSlot.transform);
            transform.position = oldSlot.transform.position;
            if (eventData.pointerCurrentRaycast.gameObject != null)
            {
                if (eventData.pointerCurrentRaycast.gameObject.name == "BG")
                {
                    GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + 2 * Vector3.up, Quaternion.identity);
                    itemObject.GetComponent<Item>().count = oldSlot.count;
                    if (oldSlot.panel == 1)
                    {
                        if (oldSlot.item is AbilityItem abilityItem)
                        {
                            abilityItem.DiscardEffects(playerStats);
                        }
                        if (oldSlot.item is WeaponItem weaponItem)
                        {
                            weaponItem.DiscardEffects(playerStats);
                        }
                    }
                    inventoryManager.UpdateStatsText();
                    oldSlot.NullifyData();
                }
                else if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
                {
                    ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
                }
            }
        }
    }
    private void ExchangeSlotData(InventorySlot newSlot)
    {
        ItemScriptableObject item = newSlot.item;
        int count = newSlot.count;
        bool isEmpty = newSlot.isEmpty;

        bool isAllowed = true;
        if (oldSlot.panel == 0 && newSlot.panel == 1)
        {
            foreach (InventorySlot slot in inventoryManager.equipSlots)
            {
                if (slot.item != null && slot.id != newSlot.id)
                {
                    if (slot.item.itemClass == oldSlot.item.itemClass)
                    {
                        isAllowed = false;
                    }
                }
            }
        }
        if (oldSlot.panel == 1 && newSlot.panel == 0)
        {
            foreach (InventorySlot slot in inventoryManager.equipSlots)
            {
                if (slot.item != null && newSlot.item != null && oldSlot.id != slot.id)
                {
                    if (slot.item.itemClass == newSlot.item.itemClass)
                    {
                        isAllowed = false;
                    }
                }
            }
        }
        if (isAllowed)
        {
            if (newSlot.panel == 1)
            {
                if (inventoryManager.isShopOpened)
                {
                    inventoryManager.sellButton.gameObject.SetActive(false);
                }
                if (inventoryManager.isStorageOpened)
                {
                    inventoryManager.storeButton.gameObject.SetActive(false);
                }
            }
            if (oldSlot.panel == 0 && newSlot.panel == 1)
            {
                if (newSlot.item is AbilityItem abilityItem)
                {
                    abilityItem.DiscardEffects(playerStats);
                }
                if (newSlot.item is WeaponItem weaponItem)
                {
                    weaponItem.DiscardEffects(playerStats);
                }
                if (oldSlot.item is AbilityItem abilityItem2)
                {
                    abilityItem2.ApplyEffects(playerStats);
                }
                if (oldSlot.item is WeaponItem weaponItem2)
                {
                    weaponItem2.ApplyEffects(playerStats);
                }
            }
            if (oldSlot.panel == 1 && newSlot.panel == 0)
            {
                if (oldSlot.item is AbilityItem abilityItem)
                {
                    abilityItem.DiscardEffects(playerStats);
                }
                if (oldSlot.item is WeaponItem weaponItem)
                {
                    weaponItem.DiscardEffects(playerStats);
                }
                if (newSlot.item is AbilityItem abilityItem2)
                {
                    abilityItem2.ApplyEffects(playerStats);
                }
                if (newSlot.item is WeaponItem weaponItem2)
                {
                    weaponItem2.ApplyEffects(playerStats);
                }
            }
            inventoryManager.UpdateStatsText();
            newSlot.item = oldSlot.item;
            newSlot.count = oldSlot.count;
            newSlot.SetIcon(oldSlot.iconGameObject.GetComponent<Image>().sprite);
            if (oldSlot.item.maxCount > 1)
            {
                // newSlot.itemCountText.text = oldSlot.count.ToString();
            }
            else
            {
                newSlot.itemCountText.text = "";
            }
            newSlot.isEmpty = oldSlot.isEmpty;
            newSlot.isClicked = oldSlot.isClicked;
            inventoryManager.slotIdClicked = newSlot.id;
            if (isEmpty == false)
            {
                oldSlot.item = item;
                oldSlot.count = count;
                oldSlot.SetIcon(item.icon);
                oldSlot.isEmpty = isEmpty;
                oldSlot.isClicked = false;
                if (item.maxCount > 1)
                {
                    // oldSlot.itemCountText.text = count.ToString();
                }
                else
                {
                    oldSlot.itemCountText.text = "";
                }
            }
            else
            {
                oldSlot.NullifyData();
            }
        }
    }
}
