using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEnums : MonoBehaviour
{
    public enum ItemType
    {
        Resource,
        Crafting,
        Equipable,
        Consumable,
        Etc
    }

    public enum ConsumableType
    {
        Health,
        Hunger,
        Thirst
    }
}
