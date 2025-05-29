using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlacementValidator : IPlacementValidator
{
    private readonly LayerMask _groundLayer;
    private readonly LayerMask _obstacleLayers;
    private readonly float _verticalCheckDistance;

    public BasicPlacementValidator(LayerMask groundLayer, LayerMask obstacleLayer, float verticalCheckDistance = 1f)
    {
        _groundLayer = groundLayer;
        _obstacleLayers = obstacleLayer;
        _verticalCheckDistance = verticalCheckDistance;
    }
    
    public bool IsValidPosition(Vector3 position)
    {
        // 아래 방향으로 바닥 검사 (Ground 레이어)
        bool hasGroundBelow = Physics.Raycast(
            position + Vector3.up * 0.5f, 
            Vector3.down, 
            _verticalCheckDistance, 
            _groundLayer);

        bool hasBuildingBelow = Physics.Raycast(
            position + Vector3.up * 0.5f,
            Vector3.down,
            _verticalCheckDistance,
            _obstacleLayers);

        // 바닥이나 기존 건축물 위가 아닌 경우
        if (!hasGroundBelow && !hasBuildingBelow)
        {
            Debug.Log("No ground or building below");
            return false;
        }

        // 주변 장애물 검사
        Collider[] colliders = Physics.OverlapBox(
            position, 
            Vector3.one * 0.5f, 
            Quaternion.identity, 
            _obstacleLayers);
        
        foreach (var col in colliders)
        {
            if(!col.isTrigger)
            {
                Debug.Log($"Obstacle detected: {col.name}");
                return false;
            }
        }
        return true;
    }
}
