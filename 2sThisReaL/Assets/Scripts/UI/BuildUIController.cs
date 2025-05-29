using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildUIController : MonoBehaviour
{
    [SerializeField] private GameObject gameModeIcon;
    [SerializeField] private GameObject buildModeIcon;
    [SerializeField] private Texture2D destroyCursor;
    [SerializeField] private Texture2D defaultCursor;

    public void ShowBuildModeUI()
    {
        gameModeIcon.SetActive(false);
        buildModeIcon.SetActive(true);
    }

    public void ShowGameModeUI()
    {
        buildModeIcon.SetActive(false);
        gameModeIcon.SetActive(true);
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