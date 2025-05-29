using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectPlacer // 설치 담당
{
    void PlaceObject(IBuildable buildable, Vector3 position);
}
