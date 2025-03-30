using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoragePage : MonoBehaviour
{
    public List<StorageSlot> slots = new List<StorageSlot>();
    public void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<StorageSlot>() != null)
            {
                transform.GetChild(i).GetComponent<StorageSlot>().Awake();
                slots.Add(transform.GetChild(i).GetComponent<StorageSlot>());
                slots[slots.Count - 1].id = i;
            }
        }
        transform.gameObject.SetActive(false);
    }
    public void OpenPage()
    {
        transform.gameObject.SetActive(true);
    }
    public void ClosePage()
    {
        transform.gameObject.SetActive(false);
        foreach (StorageSlot slot in slots)
        {
            slot.isClicked = false;
        }
    }
}
