﻿using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;


public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Dead
}

public class Enemy : MonoBehaviour, IDamageable
{

    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath;

    [Header("AI")]
    private NavMeshAgent agent;
    public float detectDistance = 20;
    private AIState aiState;

    [Header("Wandering")]
    public float minWanderDistance;
    public float maxWanderDistance;
    public float minWanderWaitTime;
    public float maxWanderWaitTime;

    [Header("Attack Setting")]
    public int damage;
    public float attackRate;
    private float lastAttackTime;
    public float attackDistance;
    public SnowballProjectile snowballProjectile;

    [Header("Spawn Effect")]
    [SerializeField] private GameObject spawnEffectPrefab;

    private float playerDistance;
    private float fieldOfView = 120f; // 시야각
    private bool isDead = false;

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
        animator.SetTrigger("Spawn");
        if (agent.isOnNavMesh)
            SetState(AIState.Wandering);
    }

    void Update()
    {
        if (GameManager.Instance.Player == null) return;
        playerDistance = Vector3.Distance(GameManager.Instance.Player.transform.position, transform.position);

        if(playerDistance > 500f)
        {
            Destroy(gameObject);
        }

        animator.SetBool("Moving", aiState != AIState.Idle && playerDistance > attackDistance);

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
        if (playerDistance < attackDistance && CanSeePlayer())
        {
            agent.isStopped = true;

            Vector3 dir = GameManager.Instance.Player.transform.position - transform.position;
            dir.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);

            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                animator.speed = 1;
                animator.SetTrigger("Attack");
                
                if (attackDistance <= 5f)
                    ConditionManager.Instance.Condition.HealHP(-damage);
            }
        }
        else if (playerDistance < detectDistance)
        {
            agent.isStopped = false;
            agent.SetDestination(GameManager.Instance.Player.transform.position);
        }
        else
        {
            agent.isStopped = true;
            SetState(AIState.Wandering);
        }
    }


    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageFlash());

        if (health <= 0 && aiState != AIState.Dead)
        {
            SetState(AIState.Dead);
        }
    }
    public void TakePhygicalDamage(int damage)
    {
        health -= damage;
        StartCoroutine(DamageFlash());
        if (health <= 0 && aiState != AIState.Dead)
        {
            SetState(AIState.Dead);
        }
    }
    // 몬스터가 데미지 입을 시 반짝임
    IEnumerator DamageFlash()
    {
        foreach (var renderer in meshRenderers)
        {
            foreach (var mat in renderer.materials)
            {
                mat.EnableKeyword("_EMISSION");
                mat.SetColor("_EmissionColor", UnityEngine.Color.red * 2f);
            }
        }

        yield return new WaitForSeconds(0.1f);

        foreach (var renderer in meshRenderers)
        {
            foreach (var mat in renderer.materials)
            {
                mat.SetColor("_EmissionColor", UnityEngine.Color.black);
            }
        }
    }
    void Dead()
    {
        if (isDead) return;
        isDead = true;

        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            ItemData item = dropOnDeath[i];
            if (dropOnDeath[i] != null)
            {
                Instantiate(item.dropPrefab, transform.position + Vector3.up * 0.5f, Quaternion.identity);
                //Debug.Log($"아이템 {item.itemName} 드랍됨");
            }
        }
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

    public void OnAttack()
    {
        if (snowballProjectile != null)
            snowballProjectile.Shoot(transform);
    }

    public void OnSpawn()
    {
        if (spawnEffectPrefab != null)
        {
            Instantiate(spawnEffectPrefab, transform.position, Quaternion.identity, transform);
        }
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
