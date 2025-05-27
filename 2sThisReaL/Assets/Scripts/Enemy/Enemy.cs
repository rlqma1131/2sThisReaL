using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Dead
}
public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance;
    private AIState aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;

    private float playerDistance;

    public float fieldOfView = 120f; // 시야각

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    } 
    void Start()
    {
        SetState(AIState.Wandering);
    }

    void Update()
    {
        playerDistance = Vector3.Distance(GameManager.Instance.player.transform.position, transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);

        switch(aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Dead:
                Dead();
                break;
        }
    }

    public void SetState(AIState state)
    {
        aiState = state;
        switch (state)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Dead:
                agent.isStopped = true;
                break;
        }

        animator.speed = agent.speed / walkSpeed; // 애니메이션 속도 조정
    }

    void PassiveUpdate()
    {
        if(aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }
    }

    //void WanderToNewLocation()
    //{
    //    if (aiState != AIState.Idle) return;

    //    SetState(AIState.Wandering);
    //    agent.SetDestination(GetWanderLocation());
    //}

    //Vector3 GetWanderLocation()
    //{
    //    NavMeshHit hit;

    //    NavMesh.SamplePosition(transform.position +
    //        (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

    //    int i = 0;

    //}

    void AttackingUpdate()
    {
        
    }

    void Dead()
    {
        agent.isStopped = true;
        animator.SetTrigger("Dead");
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.enabled = false; // 죽으면 메쉬 비활성화
        }
        Destroy(gameObject, 3f); // 3초 후에 오브젝트 제거
    }

    void OnMouseEnter()
    {
        FindObjectOfType<MouseCursor>().SetAttackCursor();
    }

    void OnMouseExit()
    {
        FindObjectOfType<MouseCursor>().SetDefaultCursor();
    }
}
