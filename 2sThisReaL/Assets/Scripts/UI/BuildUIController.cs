using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildUIController : MonoBehaviour
{
    [Header("모드 안내")]
    [SerializeField] private GameObject buildControlsGroup; // N, V
    [SerializeField] private TextMeshProUGUI modeText;

    [Header("UI 버튼")]
    [SerializeField] private Button gameModeButton; // 게임 중일 때 건축 전환
    [SerializeField] private Button buildModeButton; // 건축 중일 때 게임 전환

    [Header("마우스 커서")]
    [SerializeField] private Texture2D destroyCursor;
    [SerializeField] private Texture2D defaultCursor;

    private BuildingSystem buildingSystem;

    private void Start()
    {
        buildingSystem = FindObjectOfType<BuildingSystem>();

        gameModeButton.onClick.AddListener(() =>
        {
            buildingSystem.EnterBuildMode();
        });

        buildModeButton.onClick.AddListener(() =>
        {
            buildingSystem.ExitBuildMode();
        });

        ShowGameModeUI();
    }

    public void ShowBuildModeUI()
    {
        gameModeButton.gameObject.SetActive(false);
        buildModeButton.gameObject.SetActive(true);
        buildControlsGroup.SetActive(true);
        modeText.text = "게임";
    }

    public void ShowGameModeUI()
    {
        buildModeButton.gameObject.SetActive(false);
        gameModeButton.gameObject.SetActive(true);
        buildControlsGroup.SetActive(false);
        modeText.text = "건축";
    }

    public void SetDestroyCursor()
    {
        Cursor.SetCursor(destroyCursor, Vector2.zero, CursorMode.Auto);
    }

    public void SetDefaultCursor()
    {
        Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
    }
}