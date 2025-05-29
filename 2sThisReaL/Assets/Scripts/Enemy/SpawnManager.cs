using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerZone : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefabs;

    private float baseSpawnInterval;
    [SerializeField] private float intervalReductionPerDay = 1f;
    [SerializeField] private float minSpawnInterval = 8f;
    private int maxMonsterCount = 3;

    private float spawnRadius = 30; // 스폰영역 반지름
    private float obstacleCheckRadius = 2f;
    private float minPlayerDistance = 25f;
    private float minFOVAngle = 45f;

    private int maxTotalSpawnCount = 3;
    private int totalSpawnCount = 0;

    private Transform player;
    private Coroutine spawnRoutine;

    IEnumerator Start()
    {
        while (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            yield return null;
        }
        player = GameManager.Instance.Player.transform;
        Debug.Log("플레이어 할당됨");
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
            float adjustedInterval = 5f;
            // 생존날짜 구현되면 수정
            //    = Mathf.Max(minSpawnInterval,
            //baseSpawnInterval - GameManager.Instance.Day.SurvivedDays * intervalReductionPerDay);

            yield return new WaitForSeconds(adjustedInterval);
            TrySpawnMonster();
        }
    }

    void TrySpawnMonster()
    {
        for (int i = 0; i < maxMonsterCount; i++)
        {
            if (totalSpawnCount >= maxTotalSpawnCount)
                return;

            Vector3 spawnPoint = GetValidSpawnPoint();

            if (spawnPoint != Vector3.zero)
            {
                int index = GetMonsterIndex();
                Instantiate(monsterPrefabs[index], spawnPoint, Quaternion.identity);
                totalSpawnCount++;
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
        float maxDistance = 300f;

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
