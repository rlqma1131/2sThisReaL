using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IResourceManager // 자원 관리
{
    bool HasEnoughResources(int cost);
    void SpendResources(int cost);
}
