using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField] private int currentResources;
    private Dictionary<int, int> itemCounts = new Dictionary<int, int>();

    private void Start()
    {
        AddResource(1, 5);
        AddResource(2, 5);
        AddResource(3, 5);
        AddResource(4, 5);
        AddResource(5, 5);
        AddResource(6, 5);
    }

    public bool HasEnoughResources(int itemID, int required)
    {
        return itemCounts.TryGetValue(itemID, out int count) && count >= required;
    }

    public void SpendResources(int itemID, int amount)
    {
        if (itemCounts.ContainsKey(itemID))
        {
            itemCounts[itemID] -= amount;
            itemCounts[itemID] = Mathf.Max(0, itemCounts[itemID]);
            Debug.Log($"[ResourceManager] 사용 후: {itemID} → {itemCounts[itemID]}개 남음");
        }
    }

    public int GetResourceCount(int itemID)
    {
        return itemCounts.TryGetValue(itemID, out var count) ? count : 0;
    }

    public void AddResource(int itemID, int amount)
    {
        if (!itemCounts.ContainsKey(itemID))
            itemCounts[itemID] = 0;
        
        itemCounts[itemID] += amount;
    }
}
