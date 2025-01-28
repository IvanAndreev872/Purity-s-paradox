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
            transform.SetParent(transform.parent.parent);
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
            if (eventData.pointerCurrentRaycast.gameObject.name == "UIPanel")
            {
                GameObject itemObject = Instantiate(oldSlot.item.itemPrefab, player.position + 2 * Vector3.up, Quaternion.identity);
                itemObject.GetComponent<Item>().count = oldSlot.count;
                NullifySlotData();
            }
            else if(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
            {
                ExchangeSlotData(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>());
            }
        }
    }
    void NullifySlotData()
    {
        oldSlot.item = null;
        oldSlot.count = 0;
        oldSlot.isEmpty = true;
        oldSlot.iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.iconGameObject.GetComponent<Image>().sprite = null;
        oldSlot.itemCountText.text = "";
    }
    void ExchangeSlotData(InventorySlot newSlot)
    {
        ItemScriptableObject item = newSlot.item;
        int count = newSlot.count;
        bool isEmpty = newSlot.isEmpty;
        GameObject iconGameObject = newSlot.iconGameObject;
        TMP_Text itemCountText = newSlot.itemCountText;

        newSlot.item = oldSlot.item;
        newSlot.count = oldSlot.count;
        if (oldSlot.isEmpty == false)
        {
            newSlot.SetIcon(oldSlot.iconGameObject.GetComponent<Image>().sprite);
            newSlot.itemCountText.text = oldSlot.count.ToString();
        }
        else
        {
            newSlot.iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            newSlot.iconGameObject.GetComponent<Image>().sprite = null;
            newSlot.itemCountText.text = "";
        } 
        newSlot.isEmpty = oldSlot.isEmpty;

        oldSlot.item = item;
        oldSlot.count = count;
        if (isEmpty == false)
        {
            oldSlot.SetIcon(iconGameObject.GetComponent<Image>().sprite);
            oldSlot.itemCountText.text = count.ToString();
        }
        else
        {
            oldSlot.iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            oldSlot.iconGameObject.GetComponent<Image>().sprite = null;
            oldSlot.itemCountText.text = "";
        }
        oldSlot.isEmpty = isEmpty;
    }
}
