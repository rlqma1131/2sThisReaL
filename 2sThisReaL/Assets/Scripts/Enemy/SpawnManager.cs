using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerZone : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefabs;

    [SerializeField] private float baseSpawnInterval = 10f;
    [SerializeField] private float intervalReductionPerDay = 1f;
    [SerializeField] private float minSpawnInterval = 8f;
    [SerializeField] private int maxSpawnCount = 5;

    [SerializeField] private float spawnRadius = 30; // 스폰영역 반지름
    [SerializeField] private float obstacleCheckRadius = 2f;
    [SerializeField] private float minPlayerDistance = 10f;
    [SerializeField] private float minFOVAngle = 45f;

    private Transform player;
    private Coroutine spawnRoutine;
    
    void Start()
    {
        if(GameManager.Instance != null)
            player = GameManager.Instance.Player.transform;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawnRoutine == null)
            spawnRoutine = StartCoroutine(SpawnLoop());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            float adjustedInterval = 15f;
            // 생존날짜 구현되면 수정
            //    = Mathf.Max(minSpawnInterval,
            //baseSpawnInterval - GameManager.Instance.Day.SurvivedDays * intervalReductionPerDay);

            yield return new WaitForSeconds(adjustedInterval);
            TrySpawnMonster();
        }
    }

    void TrySpawnMonster()
    {
        for (int i = 0; i < maxSpawnCount; i++)
        {
            Vector3 spawnPoint = GetValidSpawnPoint();

            if (spawnPoint != Vector3.zero)
            {
                int index = GetMonsterIndex();
                Instantiate(monsterPrefabs[index], spawnPoint, Quaternion.identity);
            }
        }
    }

    Vector3 GetValidSpawnPoint()
    {
        for (int i = 0; i < 30; i++)
        {
            Vector3 point = transform.position + new Vector3(
                Random.Range(-spawnRadius, spawnRadius), 0f, Random.Range(-spawnRadius, spawnRadius));

            // 플레이어와의 거리
            if (Vector3.Distance(point, player.position) < minPlayerDistance)
                continue;

            // 플레이어 시야각 안에 있는지
            Vector3 dirToSpawn = (point - player.position).normalized;
            float angle = Vector3.Angle(player.forward, dirToSpawn);
            if (angle < minFOVAngle)
                continue;

            // 주변에 Obstacle 태그가 있는 오브젝트가 있는지
            Collider[] nearby = Physics.OverlapSphere(point, obstacleCheckRadius);
            bool canSpawn = true;
            foreach (var collider in nearby)
            {
                if (collider.CompareTag("Obstacle"))
                {
                    canSpawn = false;
                    break;
                }
            }
            if (!canSpawn) continue;

            
            return point;
        }

        return Vector3.zero;
    }

    int GetMonsterIndex()
    {
        float FromCenter = Vector3.Distance(Vector3.zero, transform.position);
        float maxDistance = 450f;

        int distanceLevel = Mathf.Clamp(
        (int)(FromCenter / maxDistance * monsterPrefabs.Length),
        0, monsterPrefabs.Length - 1);

        switch (distanceLevel)
        {
            case 0:
                return Random.Range(0, 2);
            case 1:
                return Random.Range(1, 3);
            case 2:
                return Random.Range(2, 4);
            case 3:
                return Random.Range(3, 5);
            case 4:
                return monsterPrefabs.Length - 1;
            default:
                return 0;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
