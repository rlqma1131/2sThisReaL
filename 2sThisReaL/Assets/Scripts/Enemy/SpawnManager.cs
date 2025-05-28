using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerZone : MonoBehaviour
{
    [SerializeField] private GameObject[] monsterPrefabs; // (강함순)
    [SerializeField] private float baseSpawnInterval = 20f;
    [SerializeField] private float intervalReductionPerDay = 0.5f;
    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private int maxSpawnCount = 3;
    [SerializeField] private float spawnRadius = 30f; // 스폰 
    [SerializeField] private float obstacleCheckRadius = 2f;
    [SerializeField] private float minPlayerDistance = 10f;
    [SerializeField] private float minFOVAngle = 45f;

    private Transform player;
    private Coroutine spawnRoutine;

    void Start()
    {
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
            float adjustedInterval = 20f;
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
                Random.Range(-spawnRadius, spawnRadius),
                0f,
                Random.Range(-spawnRadius, spawnRadius)
            );

            // 2. 플레이어와 너무 가까우면 패스
            if (Vector3.Distance(point, player.position) < minPlayerDistance)
                continue;

            // 3. 플레이어의 시야각 안에 있으면 패스
            Vector3 dirToSpawn = (point - player.position).normalized;
            float angle = Vector3.Angle(player.forward, dirToSpawn);
            if (angle < minFOVAngle)
                continue;

            // 4. 주변에 Obstacle 태그가 있는 오브젝트가 있으면 패스
            Collider[] nearby = Physics.OverlapSphere(point, obstacleCheckRadius);
            bool hasObstacle = false;
            foreach (var col in nearby)
            {
                if (col.CompareTag("Obstacle"))
                {
                    hasObstacle = true;
                    break;
                }
            }
            if (hasObstacle) continue;

            // 5. 유효한 위치라면 반환
            return point;
        }

        return Vector3.zero; // 실패
    }

    int GetMonsterIndex()
    {
        // 예: 스폰 구역의 반지름에 따라 강한 몬스터 선택
        float distanceFromCenter = Vector3.Distance(Vector3.zero, transform.position); // 월드 중심 기준
        float maxDistance = 100f; // 예시
        int index = Mathf.Clamp((int)(distanceFromCenter / maxDistance * monsterPrefabs.Length), 0, monsterPrefabs.Length - 1);
        return index;
    }
}
