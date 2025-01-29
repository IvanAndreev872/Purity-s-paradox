using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability Item", menuName = "Inventory/Items/New Ability Item")]

public class AbilityItem : ItemScriptableObject
{
    public float movementAccelerateCoefficient;

    void Start()
    {
        itemType = ItemType.Ability;
    }
}
