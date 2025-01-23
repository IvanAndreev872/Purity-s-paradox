using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Default, Ability, Weapon}

public class ItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public GameObject itemPrefab;
    public int maxCount;
    public string itemDescription;

}
