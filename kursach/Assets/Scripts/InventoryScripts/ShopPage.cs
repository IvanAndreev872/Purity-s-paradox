using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopPage : MonoBehaviour
{
    public List<ShopSlot> slots = new List<ShopSlot>();
    public void Awake()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<ShopSlot>() != null)
            {
                slots.Add(transform.GetChild(i).GetComponent<ShopSlot>());
                slots[slots.Count() - 1].id = i;
            }
        }
        transform.gameObject.SetActive(false);
    }
    public void ShowPage()
    {
        transform.gameObject.SetActive(true);
    }
    public void ClosePage()
    {
        transform.gameObject.SetActive(false);
    }
}
