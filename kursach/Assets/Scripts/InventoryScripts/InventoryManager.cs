using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject BG;
    public Transform inventoryPanel;
    public Transform equipSlotsPanel;
    public List<InventorySlot> slots = new List<InventorySlot>();
    public List<InventorySlot> equipSlots = new List<InventorySlot>();
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
        for (int i = 0; i < equipSlotsPanel.childCount; i++)
        {
            if (equipSlotsPanel.GetChild(i).GetComponent<InventorySlot>() != null)
            {
                equipSlots.Add(equipSlotsPanel.GetChild(i).GetComponent<InventorySlot>());
            }
        }
        BG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (isOpened)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void PauseGame()
    {
        isOpened = true;
        BG.SetActive(true);
        inventoryPanel.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        isOpened = false;
        BG.SetActive(false);
        inventoryPanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Item"))
        {
            AddItem(other.GetComponent<Item>().itemScriptableObject, other.GetComponent<Item>().count);
            Destroy(other.gameObject);
        }
    }
    private void AddItem(ItemScriptableObject _item, int _count)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item == _item && slot.count + _count <= slot.item.maxCount)
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
                if (_item.maxCount > 1)
                {
                    slot.itemCountText.text = _count.ToString();
                }
                else 
                {
                    slot.itemCountText.text = "";
                }
                return;
            }
        }
    }
}
