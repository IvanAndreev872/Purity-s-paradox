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
    public Transform shop;
    public int id;
    public void Awake()
    {
        iconGameObject = transform.GetChild(0).gameObject;
        itemCostText = transform.GetChild(1).GetComponent<TMP_Text>();
        shop = GameObject.FindObjectOfType<Shop>().transform;
    }
    public void SetIcon(Sprite icon)
    {
        iconGameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        iconGameObject.GetComponent<Image>().sprite = icon;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        shop.gameObject.GetComponent<Shop>().slotIdClicked = id;
        if (isClicked)
        {
            shop.gameObject.GetComponent<Shop>().itemDescriptionText.text = "";
            shop.gameObject.GetComponent<Shop>().buyButton.gameObject.SetActive(false);
            shop.gameObject.GetComponent<Shop>().costText.text = "";
            shop.gameObject.GetComponent<Shop>().costText.gameObject.SetActive(false);
        }
        else if (!isEmpty)
        {
            shop.gameObject.GetComponent<Shop>().itemDescriptionText.text = item.itemDescription;
            shop.gameObject.GetComponent<Shop>().buyButton.gameObject.SetActive(true);
            shop.gameObject.GetComponent<Shop>().costText.gameObject.SetActive(true);
            shop.gameObject.GetComponent<Shop>().costText.text = item.cost.ToString();
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
