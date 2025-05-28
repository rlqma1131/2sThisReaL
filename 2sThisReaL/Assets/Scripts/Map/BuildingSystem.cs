using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSystem : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask placementLayer;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private BuildableObject[] buildablePrefabs;

    // 🔽 인터페이스는 계속 유지
    private IBuildable currentPreview;
    private IPlacementValidator placementValidator;
    private IResourceManager resourceManager;
    private IObjectPlacer objectPlacer;
    
    private bool isInBuildMode = false;
    private int selectedPrefabIndex = 0;
    
    void Start()
    {
        placementValidator = new BasicPlacementValidator();
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
        // 기본 프리팹 선택
        selectedPrefabIndex = 0;
        StartPlacing(buildablePrefabs[selectedPrefabIndex]);
        Debug.Log("건축 모드 활성화");
    }

    private void ExitBuildMode()
    {
        CancelPreview();
        Debug.Log("건축 모드 비활성화");
    }

    private void HandleBuildModeInput()
    {
        // 건축물 선택 (1, 2, 3... 숫자 키)
        for (int i = 0; i < buildablePrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedPrefabIndex = i;
                StartPlacing(buildablePrefabs[selectedPrefabIndex]);
            }
        }

        // 프리뷰 업데이트 및 상호작용
        if (currentPreview != null)
        {
            Vector3? position = GetMouseWorldPosition();
            if (position.HasValue)
            {
                currentPreview.SetPreviewPosition(position.Value);

                if (Input.GetMouseButtonDown(0) && placementValidator.IsValidPosition(position.Value))
                {
                    objectPlacer.PlaceObject(currentPreview, position.Value);
                    // 새 프리뷰 자동 생성 (계속 건축할 수 있도록)
                    StartPlacing(buildablePrefabs[selectedPrefabIndex]);
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                currentPreview.RotatePreview();
            }

            if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
            {
                ToggleBuildMode(); // 우클릭 또는 ESC로 건축 모드 종료
            }
        }
    }

    public void StartPlacing(IBuildable buildablePrefab)
    {
        CancelPreview();
        currentPreview = Instantiate((MonoBehaviour)buildablePrefab) as IBuildable;
        currentPreview.InitializePreview();
        // ToggleBuildMode(true);
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
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, placementLayer))
        {
            return SnapToGrid(hit.point);
        }
        return null;
    }

    private Vector3 SnapToGrid(Vector3 position)
    {
        float gridSize = 1f;
        return new Vector3(
            Mathf.Round(position.x / gridSize) * gridSize,
            Mathf.Round(position.y / gridSize) * gridSize,
            Mathf.Round(position.z / gridSize) * gridSize);
    }

    // public void ToggleBuildMode()
    // {
    //     
    // }
}
