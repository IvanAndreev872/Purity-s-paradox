using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemScriptableObject item;
    public int count;
    public bool isEmpty = true, isClicked = false;
    public GameObject iconGameObject;
    public TMP_Text itemCountText;
    public int id;
    public ChestSlot()
    {
        iconGameObject = null;
        item = null;
        itemCountText = null;
        id = 0;
        count = 0;
    }
    public void Awake()
    {
        iconGameObject = transform.GetChild(0).gameObject;
        itemCountText = transform.GetChild(1).GetComponent<TMP_Text>();
    }
    public void SetIcon(Sprite icon)
    {
        iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGameObject.GetComponent<Image>().sprite = icon;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isEmpty)
        {
            Chest chest = FindObjectOfType<InventoryManager>().chest;
            if (chest.slotIdClicked == id && isClicked)
            {
                chest.itemDescriptionText.text = "";
                chest.takeButton.gameObject.SetActive(false);
                chest.costText.text = "";
                chest.costText.gameObject.SetActive(false);
                isClicked = false;
            }
            else
            {
                chest.itemDescriptionText.text = item.itemName + "\n\n" + item.itemDescription;
                chest.takeButton.gameObject.SetActive(true);
                chest.costText.gameObject.SetActive(true);
                int gain = item.cost * item.sellCoefficient / 100;
                chest.costText.text = gain.ToString();
                isClicked = true;
            }
            chest.slotIdClicked = id;
        }
    }
    public void NullifyData()
    {
        iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        iconGameObject.GetComponent<Image>().sprite = null;
        isEmpty = true;
        itemCountText.text = "";
        item = null;
        isClicked = false;
        count = 0;
    }
}
