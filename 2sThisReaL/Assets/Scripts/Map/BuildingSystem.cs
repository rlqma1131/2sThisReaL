using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BuildableObject[] buildablePrefabs;

    [Header("Layer Setting")]
    [SerializeField] private LayerMask _placementLayer;
    [SerializeField] private LayerMask _obstacleLayers;
    
    // 인터페이스는 계속 유지
    private IBuildable currentPreview;
    private IPlacementValidator _placementValidator;
    private IResourceManager resourceManager;
    private IObjectPlacer objectPlacer;
    
    private bool isInBuildMode = false;
    private int selectedPrefabIndex = 0;
    
    void Start()
    {
        _placementValidator = new BasicPlacementValidator(
            groundLayer: _placementLayer,
            obstacleLayer: _obstacleLayers,
            verticalCheckDistance: 1f);
        resourceManager = FindObjectOfType<ResourceManager>();
        objectPlacer = new ObjectPlacer(resourceManager);
    }
    
    void Update()
    {
        // B키로 건축 모드 토글
        if (Input.GetKeyDown(KeyCode.B))
        {
            ToggleBuildMode();
        }

        // 건축 모드에서만 작동하는 로직
        if (isInBuildMode)
        {
            HandleBuildModeInput();
        }
    }

    private void ToggleBuildMode()
    {
        isInBuildMode = !isInBuildMode;

        if (isInBuildMode)
        {
            EnterBuildMode();
        }
        else
        {
            ExitBuildMode();
        }
    }

    private void EnterBuildMode()
    {
        isInBuildMode = true;
        playerController?.SetBuildMode(true);
        Cursor.lockState = CursorLockMode.None; // 커서 자유 이동
        // UI 활성화
    }
    
    private void ExitBuildMode()
    {
        isInBuildMode = false;
        playerController?.SetBuildMode(false);
        Cursor.lockState = CursorLockMode.Locked; // FPS 게임인 경우
        // UI 비활성화
        CancelPreview();
    }

    private void HandleBuildModeInput()
    {
        // 건축물 선택 (1, 2, 3...)
        for (int i = 0; i < buildablePrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedPrefabIndex = i;
                StartPlacing(buildablePrefabs[selectedPrefabIndex]);
            }
        }

        if (currentPreview != null)
        {
            Vector3? position = GetMouseWorldPosition();
            if (position.HasValue)
            {
                currentPreview.SetPreviewPosition(position.Value);

                
                if (Input.GetKeyDown(KeyCode.R))
                {
                    currentPreview.RotatePreview();
                    return; // 회전했을 경우 그 프레임에서는 배치하지 않음
                }

                
                if (Input.GetMouseButtonDown(0) && _placementValidator.IsValidPosition(position.Value))
                {
                    var temp = currentPreview;
                    CancelPreview();
                    objectPlacer.PlaceObject(temp, position.Value);
                    StartPlacing(buildablePrefabs[selectedPrefabIndex]);
                }
            }

            // 건축 모드 취소
            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                CancelPreview();
                ToggleBuildMode();
            }
        }
    }

    public void StartPlacing(IBuildable buildablePrefab)
    {
        CancelPreview();
        currentPreview = Instantiate((MonoBehaviour)buildablePrefab) as IBuildable;
        currentPreview.InitializePreview();
    }

    public void CancelPreview()
    {
        if (currentPreview != null)
        {
            currentPreview.CancelPreview();
            currentPreview = null;
        }
    }

    private Vector3? GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, _placementLayer | _obstacleLayers))
        {
            // 건물의 맨 위에 맞은 경우
            if (hit.collider != null)
            {
                return SnapToGrid(hit.point, hit.collider);
            }
        }
        return null;
    }

    private Vector3 SnapToGrid(Vector3 hitPoint, Collider baseCollider)
    {
        float gridSize = 1f;

        // 현재 충돌 지점이 건물이라면 위로 스냅
        if (((1 << baseCollider.gameObject.layer) & _obstacleLayers) != 0)
        {
            float topY = baseCollider.bounds.max.y;

            return new Vector3(
                Mathf.Round(hitPoint.x / gridSize) * gridSize,
                Mathf.Round(topY / gridSize) * gridSize,
                Mathf.Round(hitPoint.z / gridSize) * gridSize);
        }

        // 일반 바닥에 대한 스냅 처리
        return new Vector3(
            Mathf.Round(hitPoint.x / gridSize) * gridSize,
            Mathf.Round(hitPoint.y / gridSize) * gridSize,
            Mathf.Round(hitPoint.z / gridSize) * gridSize);
    }

}
