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
            return;
        }

        resourceManager.SpendResources(cost);

        GameObject prefab = buildable.GetFinalPrefab();
        Quaternion rotation = buildable.GetRotation(); //  회전 정보 받아오기

        GameObject final = GameObject.Instantiate(prefab, position, rotation); //  회전 적용
    }
}
