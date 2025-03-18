using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemScriptableObject item;
    public bool isEmpty = true;
    public GameObject iconGameObject;
    public TMP_Text itemCostText;
    public bool isClicked = false;
    public Shop shop;
    public int id;
    public void Awake()
    {
        iconGameObject = transform.GetChild(0).gameObject;
        itemCostText = transform.GetChild(1).GetComponent<TMP_Text>();
        shop = FindObjectOfType<Shop>();
    }
    public void SetIcon(Sprite icon)
    {
        iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGameObject.GetComponent<Image>().sprite = icon;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        shop.slotIdClicked = id;
        if (isClicked)
        {
            shop.itemDescriptionText.text = "";
            shop.buyButton.gameObject.SetActive(false);
        }
        else if (!isEmpty)
        {
            shop.itemDescriptionText.text = item.itemDescription;
            shop.buyButton.gameObject.SetActive(true);
        }
        isClicked = !isClicked;
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
