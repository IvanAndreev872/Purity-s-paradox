using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    public Transform player;
    public AbilityItem abilityItem;
    public InventoryManager inventoryManager;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        inventoryManager = player.GetComponent<InventoryManager>();
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (!oldSlot.isEmpty)
        {
            GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!oldSlot.isEmpty)
        {
            GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.75f);
            GetComponentInChildren<Image>().raycastTarget = false;
            transform.SetParent(transform.parent.parent.parent);
        }
        if (!oldSlot.isEmpty)
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
                inventoryManager.itemDescriptionText.text = oldSlot.item.itemDescription;
                int gain = oldSlot.item.cost * oldSlot.item.sellCoefficient / 100;
                inventoryManager.costText.text = gain.ToString();
                inventoryManager.costText.gameObject.SetActive(true);
                oldSlot.isClicked = true;
                if (inventoryManager.isShopOpened)
                {
                    inventoryManager.sellButton.gameObject.SetActive(true);
                }
                if (inventoryManager.isStorageOpened)
                {
                    inventoryManager.storeButton.gameObject.SetActive(true);
                }
            }
            inventoryManager.slotIdClicked = oldSlot.id;
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!oldSlot.isEmpty)
        {
            GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1f);
            GetComponentInChildren<Image>().raycastTarget = true;
            transform.SetParent(oldSlot.transform);
            transform.position = oldSlot.transform.position;
            if (eventData.pointerCurrentRaycast.gameObject.name == "BG")
            {
                GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + 2 * Vector3.up, Quaternion.identity);
                itemObject.GetComponent<Item>().count = oldSlot.count;
                NullifySlotData();
            }
            else if (eventData.pointerCurrentRaycast.gameObject != null &&
                    eventData.pointerCurrentRaycast.gameObject.transform.parent != null &&
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent != null &&
                    eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
            {
                ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
            }
        }
    }
    private void NullifySlotData()
    {
        oldSlot.NullifyData();
    }
    private void ExchangeSlotData(InventorySlot newSlot)
    {
        if (oldSlot.gameObject.name == newSlot.gameObject.name) 
        {
            return;
        }
        ItemScriptableObject item = newSlot.item;
        int count = newSlot.count;
        bool isEmpty = newSlot.isEmpty;
        bool isClicked = newSlot.isClicked;
        GameObject iconGameObject = newSlot.iconGameObject;
        TMP_Text itemCountText = newSlot.itemCountText;
        int panel = newSlot.panel;
        int id = newSlot.id;

        bool isAllowed = true;
        if (oldSlot.panel == 0 && newSlot.panel == 1)
        {
            for (int i = 0; i < newSlot.transform.parent.childCount; i++)
            {
                if (newSlot.transform.parent.GetChild(i).GetComponent<InventorySlot>() != null)
                {
                    InventorySlot slot = newSlot.transform.parent.GetChild(i).GetComponent<InventorySlot>();
                    if (slot.item == oldSlot.item)
                    {
                        isAllowed = false;
                    }
                }
            }
        }
        if (isAllowed)
        {
            if (oldSlot.panel == 0 && newSlot.panel == 1)
            {
                if (newSlot.item is AbilityItem abilityItem)
                {
                    abilityItem.DiscardEffects(player.GetComponent<PlayerStats>());
                }
                if (oldSlot.item is AbilityItem abilityItem2)
                {
                    abilityItem2.ApplyEffects(player.GetComponent<PlayerStats>());
                }
            }
            if (oldSlot.panel == 1 && newSlot.panel == 0)
            {
                if (oldSlot.item is AbilityItem abilityItem3)
                {
                    abilityItem3.DiscardEffects(player.GetComponent<PlayerStats>());
                }
                if (newSlot.item is AbilityItem abilityItem4)
                {
                    abilityItem4.ApplyEffects(player.GetComponent<PlayerStats>());
                }
            }
            newSlot.item = oldSlot.item;
            newSlot.count = oldSlot.count;
            newSlot.SetIcon(oldSlot.iconGameObject.GetComponent<Image>().sprite);
            if (oldSlot.item.maxCount > 1)
            {
                newSlot.itemCountText.text = oldSlot.count.ToString();
            }
            else
            {
                newSlot.itemCountText.text = "";
            }
            newSlot.isEmpty = oldSlot.isEmpty;
            newSlot.isClicked = oldSlot.isClicked;
            newSlot.panel = oldSlot.panel;
            newSlot.id = oldSlot.id;
            if (isEmpty == false)
            {
                oldSlot.item = item;
                oldSlot.count = count;
                oldSlot.SetIcon(item.icon);
                oldSlot.isEmpty = isEmpty;
                oldSlot.isClicked = isClicked;
                oldSlot.panel = panel;
                oldSlot.id = id;
                if (item.maxCount > 1)
                {
                    oldSlot.itemCountText.text = count.ToString();
                }
                else
                {
                    oldSlot.itemCountText.text = "";
                }
            }
            else
            {
                NullifySlotData();
            }
        }
    }
}
