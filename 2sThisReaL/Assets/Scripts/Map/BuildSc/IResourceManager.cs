using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceManager // 자원 관리
{
    bool HasEnoughResources(int itemID, int required);
    void SpendResources(int itemID, int amount);
    int GetResourceCount(int itemID);
}
