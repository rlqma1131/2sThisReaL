using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField] private int currentResources = 100;
    
    public bool HasEnoughResources(int cost)
    {
        return currentResources >= cost;
    }

    public void SpendResources(int cost)
    {
        currentResources -= cost;
    }
}
