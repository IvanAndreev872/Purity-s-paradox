using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int count;
    public bool isEmpty = true;
    public GameObject iconGameObject;
    public TMP_Text itemCountText;
    public int panel; // 0 - inventory, 1 - equip
    public void Start()
    {
        iconGameObject = transform.GetChild(0).GetChild(0).gameObject;
        itemCountText = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
        if (transform.parent.gameObject.name == "InventoryPanel")
        {
            panel = 0;
        }
        else
        {
            panel = 1;
        }
    }
    public void SetIcon(Sprite icon)
    {
        iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGameObject.GetComponent<Image>().sprite = icon;
    }
}
