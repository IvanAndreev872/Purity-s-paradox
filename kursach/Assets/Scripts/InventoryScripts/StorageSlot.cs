using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StorageSlot : MonoBehaviour, IPointerClickHandler
{
    public ItemScriptableObject item;
    public int count;
    public bool isEmpty = true, isClicked = false;
    public GameObject iconGameObject;
    public TMP_Text itemCountText;
    public StorageManager storage;
    public int id;
    public void Awake()
    {
        iconGameObject = transform.GetChild(0).gameObject;
        itemCountText = transform.GetChild(1).GetComponent<TMP_Text>();
        storage = FindObjectOfType<StorageManager>();
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
            if (storage.slotIdClicked == id && isClicked)
            {
                storage.itemDescriptionText.text = "";
                storage.takeButton.gameObject.SetActive(false);
                storage.costText.text = "";
                storage.costText.gameObject.SetActive(false);
                isClicked = false;
            }
            else
            {
                storage.itemDescriptionText.text = item.itemDescription;
                storage.takeButton.gameObject.SetActive(true);
                storage.costText.gameObject.SetActive(true);
                int gain = item.cost * item.sellCoefficient / 100;
                storage.costText.text = gain.ToString();
                isClicked = true;
            }
            storage.slotIdClicked = id;
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
