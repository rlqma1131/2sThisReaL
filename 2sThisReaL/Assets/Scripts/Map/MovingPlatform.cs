using System;
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
        // public bool returnOnExit = false; // 플레이어 하차시 복귀여부
    }
    
    public WaypointData[] waypoints;
    [SerializeField] private int speed;
    // public bool isRoundTrip = true; // 왕복모드
    
    private int currentIndex = 0;
    // private int direction = 1; // 이동방향 1 or -1
    private bool isWaiting = false;
    private float waitTimer = 0f;
    private bool isPlayerOnPlatform = false;
    // private bool isActive = false; // 플랫폼 활성화 여부
    // private Vector3[] originalWaypoints; // 복귀용 초기 위치
    private bool isMovingForward = true; // 순방향인지 여부

    // private void Start()
    // {
    //     // 초기 웨이포인트 위치 백업(복귀용)
    //     originalWaypoints = new Vector3[waypoints.Length];
    //     for (int i = 0; i < waypoints.Length; i++)
    //     {
    //         if (waypoints[i].waypoint != null)
    //             originalWaypoints[i] = waypoints[i].waypoint.position;
    //     }
    // }

    void Update()
    {
        if (waypoints.Length == 0) return;

        WaypointData currentData = waypoints[currentIndex];

        // 1. 플레이어 대기 모드 + 플레이어 없음 = 정지 (A 지점에서만 적용)
        if (currentData.waitForPlayer && !isPlayerOnPlatform && currentIndex == 0 && isMovingForward)
        {
            return;
        }

        // 2. 대기 상태 처리
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0f) isWaiting = false;
            return;
        }

        // 3. 이동 로직
        Transform target = currentData.waypoint;
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // 4. 도착 처리
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

        // 다음 웨이포인트 결정
        if (isMovingForward)
        {
            if (currentIndex < waypoints.Length - 1)
            {
                currentIndex++; // A → B → C
            }
            else
            {
                // C 도착 시 역방향 모드 전환
                isMovingForward = false;
                currentIndex = waypoints.Length - 2; // C 다음은 B로
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
                // A 도착 시 순방향 모드 전환
                isMovingForward = true;

                if (data.waitForPlayer)
                {
                    isWaiting = true;
                    // currentIndex 유지 (플레이어 기다리기)
                }
                else
                {
                    currentIndex = 1; // A 다음은 B로
                }
            }
        }
    }
    // private void ArriveAtWaypoint(WaypointData data)
    // {
    //     // 정지 시간 적용
    //     if (data.stopDuration > 0f)
    //     {
    //         isWaiting = true;
    //         waitTimer = data.stopDuration;
    //     }
    //
    //     // 다음 웨이포인트 결정
    //     if (isMovingForward)
    //     {
    //         if (currentIndex < waypoints.Length - 1)
    //         {
    //             currentIndex++; // B → C
    //         }
    //         else
    //         {
    //             // C에 도착하면 역방향 모드 활성화 (C → B → A)
    //             isMovingForward = false;
    //             if (currentIndex > 0) currentIndex--;
    //         }
    //     }
    //     else
    //     {
    //         if (currentIndex > 0)
    //         {
    //             currentIndex--; // B → A
    //         }
    //         else
    //         {
    //             // A에 도착하면 순방향 모드로 전환 + 플레이어 대기
    //             isMovingForward = true;
    //             isWaiting = true; // 플레이어 탑승 대기
    //         }
    //     }
    // }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
            isPlayerOnPlatform = true;
            // isActive = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
            isPlayerOnPlatform = false;
            
            // // 복귀모드 체크
            // WaypointData currentData = waypoints[currentIndex];
            // if(currentData.returnOnExit && !isRoundTrip)
            //     ReturnToOriginalPosition();
        }
    }

    // private void ReturnToOriginalPosition()
    // {
    //     currentIndex = 0; // 첫번째 웨이 복귀
    //     direction = 1; // 방향 초기화
    //     isActive = false; //
    // }
    // 이동 시나리오
    // 기본 - 모든 지점 stopDuration = 0 isRoundTrip = false
    // 일시정디 - 특정지점 ex)B stopDuration = ex) 3 B도착시 3초 정지 후 이동
    // 플레이어 탑승 대기 - 출발 지점 waitForPlayer = true stopDuration = ex)2 플레이어가 올라타야 출발, 2초 후 이동
    // 복귀 모드 - 최종 지점 returnOnExit = true isRoundTrip = false 플레이어 하차 시 첫번째 지점으로 복귀
    // 왕복 엘베모드 - isRoundTrip = true 중간지점 stopDuration = ex)1 A - B - c 자동 왕복, 각 지점에서 1초 정지
    // 하이브리드 모드 - A : waitForPlayer = true C: returnOnExit = true isRoundTrip = false
    // 플레이어 하차시 항상 A로 복귀 A - B - C(하차) - B - A 경로
    // 플레이어 유지 - 모든 지점 waitForPlayer = false isRoundTrip = true 플레이어 하차해도 플랫폼은 계속 왕복
}
