using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject buildPanel;
    [SerializeField] private GameObject buildItemUIPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] private List<BuildItemData> buildItems;
    [SerializeField] private BuildingSystem buildingSystem;
    [SerializeField] private ResourceManager resourceManager;
    
    private IResourceManager resourceManagers;
    
    void Start()
    {
        if (resourceManager == null)
            Debug.LogError("[BuildMenuUI] ResourceManager 연결 안 됨");
        if (buildingSystem == null)
            Debug.LogError("[BuildMenuUI] BuildingSystem 연결 안 됨");
        
        PopulateMenu();
        buildPanel.SetActive(false);
    }

    private void PopulateMenu()
    {
        foreach (Transform child in contentParent )
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < buildItems.Count; i++)
        {
            BuildItemData capturedItem = buildItems[i]; // ✅ 명확히 캡처

            GameObject go = Instantiate(buildItemUIPrefab, contentParent);
            var ui = go.GetComponent<BuildItemUI>();

            int count = resourceManager.GetResourceCount(capturedItem.itemID);
            bool isAvailable = count > 0;

            ui.Setup(capturedItem, count, isAvailable, () =>
            {
                buildPanel.SetActive(false);
                buildingSystem.CancelCurrentPreview();
                buildingSystem.SetSelectedBuildItem(capturedItem);
                Debug.Log($"Clicked item: {capturedItem.name}");
            });
        }
    }

    public void ToggleUI(bool? forceState = null)
    {
        bool isActive = buildPanel.activeSelf;
        bool newState = forceState ?? !isActive;

        buildPanel.SetActive(newState);

        if (newState)
        {
            buildingSystem.CancelCurrentPreview();
            PopulateMenu();
            Debug.Log("[BuildMenuUI] UI opened");
        }
        else
        {
            Debug.Log("[BuildMenuUI] UI closed");
        }
    }

    public void RefreshMenu()
    {
        PopulateMenu();
    }
}
