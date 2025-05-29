using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BuildableObject[] buildablePrefabs;
    [SerializeField] private BuildMenuUI buildMenuUI;
    [SerializeField] private BuildUIController buildUIController;
    [SerializeField] private MouseCursor mouseCursor;

    [Header("Layer Setting")]
    [SerializeField] private LayerMask _placementLayer;
    [SerializeField] private LayerMask _obstacleLayers;
    
    // 인터페이스는 계속 유지
    private IBuildable currentPreview;
    private IPlacementValidator _placementValidator;
    private IResourceManager resourceManager;
    private IObjectPlacer objectPlacer;
    
    private int selectedPrefabIndex = 0;
    private BuildItemData selectedBuildItem;
    private bool isInBuildMode = false;
    private GameObject lastHoveredObject;
    private enum BuildSubMode {None, Placing, Destroying}
    private BuildSubMode currentSubMode = BuildSubMode.None;
    
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
            if (Input.GetKeyDown(KeyCode.N))
            {
                currentSubMode = BuildSubMode.Placing;
                buildMenuUI.ToggleUI();
            }

            if (Input.GetKeyDown(KeyCode.V))
            {
                currentSubMode = BuildSubMode.Destroying;
                buildMenuUI.ToggleUI(false);
                CancelPreview();

                buildUIController.SetDestroyCursor();
            }
        }
        
        HandleBuildModeInput();
    }

    private void ToggleBuildMode()
    {
        isInBuildMode = !isInBuildMode;
        playerController?.SetBuildMode(isInBuildMode);

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
        Cursor.visible = true;

        // UI 활성화
        buildUIController.ShowBuildModeUI();
        buildUIController.SetDefaultCursor();
    }
    
    private void ExitBuildMode()
    {
        isInBuildMode = false;
        playerController?.SetBuildMode(false);
        Cursor.lockState = CursorLockMode.Locked; // FPS 게임인 경우
        Cursor.visible = false;
        // UI 비활성화
        buildUIController.ShowGameModeUI();
        buildUIController.SetDefaultCursor();

        CancelPreview();
    }

    private void HandleBuildModeInput()
    {
        switch (currentSubMode)
        {
            case BuildSubMode.Placing:
                HandlePlacementMode();
                break;
            case BuildSubMode.Destroying:
                HandleDestroyMode();
                break;
        }
    }

    private void HandlePlacementMode()
    {
        if(currentPreview == null) return;
        
        Vector3? position = GetMouseWorldPosition();
        if(!position.HasValue) return;
        
        currentPreview.SetPreviewPosition(position.Value);

        if (Input.GetKeyDown(KeyCode.R))
        {
            currentPreview.RotatePreview();
            return;
        }

        if (Input.GetMouseButtonDown(0) && _placementValidator.IsValidPosition(position.Value))
        {
            var temp = currentPreview;
            CancelPreview();
            objectPlacer.PlaceObject(temp, position.Value);
            StartPlacing(selectedBuildItem.previewPrefab.GetComponent<IBuildable>());
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            CancelPreview();
            currentSubMode = BuildSubMode.None;
        }
    }

    private void HandleDestroyMode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            GameObject target = hit.collider.gameObject;
            if (lastHoveredObject != target)
            {
                ToggleOutline(lastHoveredObject, false);
                lastHoveredObject = target;
                ToggleOutline(target, true);
            }

            if (Input.GetMouseButtonDown(0))
            {
                var destructible = target.GetComponent<IDestructible>();
                if (destructible != null)
                {
                    destructible.DestroySelf();
                    Debug.Log("[DestroyMode] 파괴됨: " + target.name);
                }
            }
        }
        else if (lastHoveredObject != null)
        {
            ToggleOutline(lastHoveredObject, false);
            lastHoveredObject = null;
        }

        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOutline(lastHoveredObject, false);
            lastHoveredObject = null;
            currentSubMode = BuildSubMode.None;

            mouseCursor.SetDestroyCursor();
        }
    }

    private void ToggleOutline(GameObject obj, bool enabled)
    {
        if (obj == null) return;
        
        var outline = obj.GetComponent<Outline>();
        if(outline != null)
            outline.enabled = enabled;
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

    public void CancelCurrentPreview()
    {
        currentPreview?.CancelPreview();
        currentPreview = null;
    }

    public void OnObjectPlaced()
    {
        buildMenuUI.RefreshMenu();
    }

    public void SetSelectedBuildItem(BuildItemData item)
    {
        selectedBuildItem = item;
        StartPlacing(item.previewPrefab.GetComponent<IBuildable>());
    }

    public void SetDefaultCursor()
    {
        mouseCursor.SetDefaultCursor();
    }

    public void SetDestroyCursor()
    {
        mouseCursor.SetDestroyCursor();
    }
}
