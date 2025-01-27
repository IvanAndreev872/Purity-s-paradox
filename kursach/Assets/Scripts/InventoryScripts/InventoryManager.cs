using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject UIPanel;
    public Transform inventoryPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public bool isOpened = false;
    void Start()
    {
        for (int i = 0; i < inventoryPanel.childCount; i++)
        {
            if (inventoryPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                slots.Add(inventoryPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        UIPanel.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpened)
            {
                UIPanel.SetActive(false);
            }
            else
            {
                UIPanel.SetActive(true);
            }
            isOpened = !isOpened;
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            Debug.Log("LOL\n");
            AddItem(other.GetComponent<Item>().itemScriptableObject, other.GetComponent<Item>().count);
            Destroy(other.gameObject);
            Debug.Log("KEK\n");
        }
    }
    private void AddItem(ItemScriptableObject _item, int _count)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                slot.count += _count;
                slot.itemCountText.text = slot.count.ToString();
                return;
            }
        }
        foreach (InventorySlot slot in slots)
        {
            if (slot.isEmpty == true)
            {
                slot.item = _item;
                slot.count = _count;
                slot.isEmpty = false;
                slot.SetIcon(_item.icon);
                slot.itemCountText.text = _count.ToString();
                return;
            }
        }
    }
}
