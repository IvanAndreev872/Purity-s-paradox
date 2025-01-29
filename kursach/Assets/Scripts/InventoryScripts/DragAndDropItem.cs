using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    public Transform player;
    public AbilityItem abilityItem;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
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
        oldSlot.item = null;
        oldSlot.count = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.iconGameObject.GetComponent<Image>().sprite = null;
        oldSlot.itemCountText.text = "";
    }
    private void ExchangeSlotData(InventorySlot newSlot)
    {
        ItemScriptableObject item = newSlot.item;
        int count = newSlot.count;
        bool isEmpty = newSlot.isEmpty;
        GameObject iconGameObject = newSlot.iconGameObject;
        TMP_Text itemCountText = newSlot.itemCountText;
        int panel = newSlot.panel;

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
            newSlot.panel = oldSlot.panel;
            if (isEmpty == false)
            {
                oldSlot.item = item;
                oldSlot.count = count;
                oldSlot.SetIcon(item.icon);
                oldSlot.isEmpty = isEmpty;
                oldSlot.panel = panel;
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
