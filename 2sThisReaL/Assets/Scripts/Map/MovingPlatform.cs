using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [System.Serializable]
    public class WaypointData
    {
        public Transform waypoint;
        public float stopDuration;
        public bool waitForPlayer = false;
    }

    public WaypointData[] waypoints;
    [SerializeField] private int speed;

    private int currentIndex = 0;
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isPlayerOnPlatform = false;
    private bool isMovingForward = true;

    void Update()
    {
        if (waypoints.Length == 0) return;

        WaypointData currentData = waypoints[currentIndex];

        // 플레이어 대기 모드: A 지점에서만 적용
        if (currentData.waitForPlayer && !isPlayerOnPlatform && currentIndex == 0 && isMovingForward)
        {
            return;
        }

        // 대기 중이면 타이머 작동
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f) isWaiting = false;
            return;
        }

        // 이동
        Transform target = currentData.waypoint;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 도착 확인
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            HandleWaypointArrival(currentData);
        }
    }

    private void HandleWaypointArrival(WaypointData data)
    {
        // 정지 시간 적용
        if (data.stopDuration > 0f)
        {
            isWaiting = true;
            waitTimer = data.stopDuration;
        }

        // 다음 웨이포인트 계산
        if (isMovingForward)
        {
            if (currentIndex < waypoints.Length - 1)
            {
                currentIndex++; // A → B → C
            }
            else
            {
                // C 도착 → 역방향 전환
                isMovingForward = false;
                currentIndex = waypoints.Length - 2; // 다음은 B
            }
        }
        else
        {
            if (currentIndex > 0)
            {
                currentIndex--; // C → B → A
            }
            else
            {
                // A 도착 → 순방향 전환
                isMovingForward = true;

                if (data.waitForPlayer)
                {
                    isWaiting = true;
                }
                else
                {
                    currentIndex = 1; // 다음은 B
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
            isPlayerOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
            isPlayerOnPlatform = false;
        }
    }
}
