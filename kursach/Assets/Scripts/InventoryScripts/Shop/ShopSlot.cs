using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemScriptableObject item;
    public bool isEmpty = true, isClicked = false;
    public GameObject iconGameObject;
    public TMP_Text itemCostText;
    public int id;
    public void Awake()
    {
        iconGameObject = transform.GetChild(0).gameObject;
        itemCostText = transform.GetChild(1).GetComponent<TMP_Text>();
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
            Shop shop = FindObjectOfType<Shop>();
            if (shop.slotIdClicked == id && isClicked)
            {
                shop.itemDescriptionText.text = "";
                shop.buyButton.gameObject.SetActive(false);
                shop.costText.text = "";
                shop.costText.gameObject.SetActive(false);
                isClicked = false;
            }
            else
            {
                shop.itemDescriptionText.text = item.itemName + "\n\n" + item.itemDescription;
                shop.buyButton.gameObject.SetActive(true);
                shop.costText.gameObject.SetActive(true);
                shop.costText.text = item.cost.ToString();
                isClicked = true;
            }
            shop.slotIdClicked = id;
        }
    }
    public void NullifyData()
    {
        iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        iconGameObject.GetComponent<Image>().sprite = null;
        isEmpty = true;
        itemCostText.text = "";
        item = null;
        isClicked = false;
    }
}
