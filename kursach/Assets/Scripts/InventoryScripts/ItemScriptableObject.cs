using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {Default, Ability, Weapon}

public class ItemScriptableObject : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public GameObject itemPrefab;
    public Sprite icon;
    public int maxCount = 1;
    public int cost;
    public int sellCoefficient = 10;
    public string itemDescription;
}
