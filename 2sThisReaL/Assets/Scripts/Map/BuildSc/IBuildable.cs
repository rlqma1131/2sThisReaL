using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IBuildable // 설치 가능한 건축물
    {
        void InitializePreview();
        void SetPreviewPosition(Vector3 position);
        void RotatePreview();
        void CancelPreview();
        GameObject GetFinalPrefab();
        int GetCost();
        int GetItemID();
        Quaternion GetRotation();
    }