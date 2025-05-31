using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum SkillType
{
    Fireball,
    DashAttack
}


public class PlayerSkillController : MonoBehaviour
{
    public Skill[] skills;

    [Header("Fireball")]
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballSpawnPoint;

    [Header("Dash Attack")]
    [SerializeField] private float dashDistance = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 3f;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fireballClip;
    [SerializeField] private AudioClip dashClip;

    private bool isDashing = false;
    private bool canDash = true;
    void Update()
    {
        foreach (Skill skill in skills)
        {
            skill.UpdateCooldownUI();
            if (Input.GetKeyDown(skill.activationKey) && skill.IsReady())
            {
                ActivateSkill(skill);
                skill.Use();
            }
        }
    }

    void ActivateSkill(Skill skill)
    {
        switch (skill.type)
        {
            case SkillType.Fireball:
                CastFireball();
                break;

            case SkillType.DashAttack:
                if (canDash)
                {
                    Debug.Log("DashAttack Activated!");
                    StartCoroutine(DashAttack());
                }
                break;
        }
    }
    void CastFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, transform.rotation);
        audioSource.PlayOneShot(fireballClip);
        Debug.Log("Fireball Cast!");
    }
    IEnumerator DashAttack()
    {
        canDash = false;
        isDashing = true;

        audioSource.PlayOneShot(dashClip);

        Vector3 dashDir = Camera.main.transform.forward.normalized;
        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + dashDir * dashDistance;

        // 대시 시작 시 적 탐지
        RaycastHit[] hits = Physics.SphereCastAll(startPos, 1f, dashDir, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                if (hit.collider.TryGetComponent<idamagable>(out var enemy))
                {
                    enemy.takephygicaldamage(100);
                }
            }
        }

        float elapsed = 0f;
        float checkRadius = 0.5f;
        float capsuleHeight = 2.0f;
        float offsetY = 3.0f; // 캡슐 중심이 캐릭터 중앙쯤 되도록

        Vector3 lastValidPosition = transform.position;

        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;
            Vector3 nextPos = Vector3.Lerp(startPos, targetPos, t);

            // CheckCapsule 충돌 검사
            Vector3 capsuleBottom = nextPos + Vector3.up * (offsetY - capsuleHeight / 2f);
            Vector3 capsuleTop = nextPos + Vector3.up * (offsetY + capsuleHeight / 2f);

            bool hitWall = Physics.CheckCapsule(capsuleBottom, capsuleTop, checkRadius, LayerMask.GetMask("Ground", "Building"));

            if (hitWall)
            {
                Debug.Log("벽에 닿아서 대시 중단됨");
                break;
            }

            lastValidPosition = nextPos;
            transform.position = nextPos;

            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = lastValidPosition;

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
