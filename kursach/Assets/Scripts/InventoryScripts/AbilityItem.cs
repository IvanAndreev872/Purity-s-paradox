using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Food Item", menuName = "Inventory/Items/New Ability Item")]

public class FoodItem : ItemScriptableObject
{
    public float movementAccelerateCoefficient;

    void Start()
    {
        itemType = ItemType.Ability;
    }
}
