using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlacementValidator : IPlacementValidator
{
    public bool IsValidPosition(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapBox(position, Vector3.one * 0.5f);
        foreach (var col in colliders)
        {
            if(!col.isTrigger)
                return false;
        }
        return true;
    }
}
