using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : IObjectPlacer
{
    private readonly IResourceManager resourceManager;

    public ObjectPlacer(IResourceManager resourceManager)
    {
        this.resourceManager = resourceManager;
    }
    
    public void PlaceObject(IBuildable buildable, Vector3 position)
    {
        int cost = buildable.GetCost();
        if (!resourceManager.HasEnoughResources(cost))
        {
            Debug.Log("Not enough resources to build");
            return;
        }
        
        resourceManager.SpendResources(cost);
        GameObject final = GameObject.Instantiate(buildable.GetFinalPrefab(), position, Quaternion.identity);
    }
}
