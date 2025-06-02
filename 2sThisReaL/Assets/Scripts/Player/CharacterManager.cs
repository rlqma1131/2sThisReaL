using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GenderType
{
    Male,
    Female
}
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager Instance { get; private set; }

    public Player Player;

    public GenderType SelectedGender { get; private set; }

    [Header("Player Prefabs")]
    public GameObject malePrefab;
    public GameObject femalePrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetGender(GenderType gender)
    {
        SelectedGender = gender;
        Debug.Log($"[CharacterManager] Gender set to: {gender}");
    }

    public void SpawnPlayer(Vector3 spawnPosition)
    {
        GameObject prefab = (SelectedGender == GenderType.Male) ? malePrefab : femalePrefab;

        if (prefab == null)
        {
            //Debug.LogError("[CharacterManager] Player prefab is null!");
            return;
        }

        GameObject playerObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        //Debug.Log($"[CharacterManager] Player spawned at {spawnPosition}");

        Player = playerObj.GetComponent<Player>();

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 특정 씬에서만 파괴 (예: "MainMenu", "EndingScene" 등)
        if (scene.name == "StartScene")
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 씬 로딩 이벤트 해제 (메모리 누수 방지)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}