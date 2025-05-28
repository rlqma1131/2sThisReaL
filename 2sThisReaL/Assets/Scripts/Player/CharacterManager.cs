using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Debug.LogError("[CharacterManager] Player prefab is null!");
            return;
        }

        GameObject playerObj = Instantiate(prefab, spawnPosition, Quaternion.identity);
        Debug.Log($"[CharacterManager] Player spawned at {spawnPosition}");

        Player player = playerObj.GetComponent<Player>();
    }
}