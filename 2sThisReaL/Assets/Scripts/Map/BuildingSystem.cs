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
    
    void Start()
    {
        placementValidator = new BasicPlacementValidator();
        resourceManager = FindObjectOfType<ResourceManager>();
        objectPlacer = new ObjectPlacer(resourceManager);
    }
    
    void Update()
    {
        if (currentPreview != null)
        {
            Vector3? position = GetMouseWorldPosition();
            if (position.HasValue)
            {
                currentPreview.SetPreviewPosition(position.Value);

                if (Input.GetMouseButtonDown(0) && placementValidator.IsValidPosition(position.Value))
                {
                    objectPlacer.PlaceObject(currentPreview, position.Value);
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                currentPreview.RotatePreview();
            }

            if (Input.GetMouseButtonDown(1))
            {
                CancelPreview();
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
