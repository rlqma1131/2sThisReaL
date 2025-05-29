using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlacementValidator // 설치 가능 여부 판별
{
    bool IsValidPosition(Vector3 position);
}
