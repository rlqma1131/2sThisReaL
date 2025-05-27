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
        if (agent.isOnNavMesh)
            SetState(AIState.Wandering);
    }

    void Update()
    {
        playerDistance = Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
                animator.SetBool("Moving", false);
                PassiveUpdate();
                break;
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
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (playerDistance < detectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit; // 최단거리 경로로 이동할 위치를 담는 변수

        NavMesh.SamplePosition(transform.position +
            (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < detectDistance) // 플레이어와의 거리가 detectDistance보다 가까운 경우
        {
            NavMesh.SamplePosition(transform.position +
            (Random.onUnitSphere * Random.Range(minWanderDistance, maxWanderDistance)), out hit, maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                //GameManager.Instance.player.controller.GetComponent<IDamagalbe>().TakePhygicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            if (playerDistance < detectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(GameManager.Instance.player.transform.position, path))
                {
                    agent.SetDestination(GameManager.Instance.player.transform.position);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = (GameManager.Instance.player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0 && aiState != AIState.Dead)
        {
            SetState(AIState.Dead);
        }
    }

    void Dead()
    {
        agent.isStopped = true;
        animator.SetTrigger("Dead");
        StartCoroutine(SinkIntoGround());   // 스르륵 땅 속으로 가라앉기
    }

    IEnumerator SinkIntoGround()
    {
        yield return new WaitForSeconds(1f);

        float sinkDuration = 1.5f;
        float elapsedTime = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos - new Vector3(0, 1.5f, 0);

        while (elapsedTime < sinkDuration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / sinkDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;

        Destroy(gameObject);
    }

}
