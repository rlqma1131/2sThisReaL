using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [System.Serializable]
    public class WaypointData
    {
        public Transform waypoint;
        public float stopDistance;
        public bool waitForPlayer = false;
        public bool returnOnExit = false;
    }
    
    public WaypointData[] waypoints;
    [SerializeField] private int speed;
    
    [Header("Player Acivation Settings")]
    public bool waitForPlayer = false;

    public float delayAfterPlayerOn = 3f;
    
    private int currentIndex = 0;
    private bool isPaused = false;
    private float pauseTimer = 0f;
    private bool isPlayerOnPlatform = false;
    private bool isMovementAciving = false;
    
    void Update()
    {
        // // 플레잉어 대기모드, 활성화 x
        // if(waitForPlayer && !isMovementAciving) return;
        //
        // // 일시정지 상태
        // if (isPaused)
        // {
        //     pauseTimer -= Time.deltaTime;
        //     if(pauseTimer <= 0f) isPaused = false;
        //     return;
        // }
        //
        // //이동
        // if(waypoints.Length == 0 || isPaused) return;
        //
        // Transform target = waypoints[currentIndex].waypoint;
        // transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        //
        // //도착 확인
        // if (Vector3.Distance(transform.position, target.position) < 0.05f)
        // {
        //     WaypointData currentData = waypoints[currentIndex];
        //     if (currentData.pauseAtPoint)
        //     {
        //         PauseAtCurrentWaypoint(currentData.pauseDistance);
        //     }
        //     
        //     currentIndex = (currentIndex + 1) % waypoints.Length;
        // }
    }

    private void PauseAtCurrentWaypoint(float duration)
    {
        isPaused = true;
        pauseTimer = duration;
        // Invoke("ResumeMovement", duration);
    }

    // private void ResumeMovement()
    // {
    //     isPaused = false;
    // }
    

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
            isPlayerOnPlatform = true;
            
            // 플레이어 대기모드, 처음 올라타면 타이머 시작
            if (waitForPlayer && !isMovementAciving)
            {
                Invoke("ActivateMovement", delayAfterPlayerOn);
            }
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

    private void ActivateMovement()
    {
        if(isPlayerOnPlatform) // 플레이어가 올라와 있을때만
            isMovementAciving = true;
    }
}
