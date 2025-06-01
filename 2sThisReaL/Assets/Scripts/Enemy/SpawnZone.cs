using UnityEngine;

public class SpawnZone : MonoBehaviour
{
    [Header("Spawnzone")]
    public GameObject zonePrefab;

    [Header("SpawnZone Size")]
    private int gridSizeX = 10;
    private int gridSizeZ = 10;
    public float spacing;

    private Vector3 startPosition = Vector3.zero;

    [ContextMenu("Generate Zones")]
    public void GenerateZones()
    {
        if (zonePrefab == null)
        {
            Debug.LogError("Zone 프리팹이 없어!");
            return;
        }

        for (int x = 0; x < gridSizeX; x++)
        {
            for (int z = 0; z < gridSizeZ; z++)
            {
                Vector3 position = startPosition + new Vector3(x * spacing, 0, z * spacing);
                GameObject zone = Instantiate(zonePrefab, position, Quaternion.identity, transform);
                zone.name = $"Zone_{x}_{z}";
            }
        }
    }
}
