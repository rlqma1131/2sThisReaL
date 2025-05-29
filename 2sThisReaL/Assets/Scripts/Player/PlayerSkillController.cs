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

    private bool isDashing = false;
    private bool canDash = true;

    private void OnEnable()
    {
        // 모든 스킬의 InputAction에 이벤트 바인딩
        foreach (Skill skill in skills)
        {
            skill.inputAction.action.Enable();
            skill.inputAction.action.performed += ctx => TryActivateSkill(skill);
        }
    }

    private void OnDisable()
    {
        // 이벤트 제거
        foreach (Skill skill in skills)
        {
            skill.inputAction.action.performed -= ctx => TryActivateSkill(skill);
            skill.inputAction.action.Disable();
        }
    }

    private void TryActivateSkill(Skill skill)
    {
        if (skill.IsReady())
        {
            ActivateSkill(skill);
            skill.Use();
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
                    StartCoroutine(DashAttack());
                break;
        }
    }

    void CastFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, transform.rotation);
        Debug.Log("Fireball Cast!");
    }

    IEnumerator DashAttack()
    {
        canDash = false;
        isDashing = true;

        Vector3 dashDir = transform.forward;
        dashDir.y = 0;
        dashDir.Normalize();

        Vector3 startPos = transform.position;
        Vector3 targetPos = startPos + dashDir * dashDistance;

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, dashDir, dashDistance, enemyLayer);
        foreach (var hit in hits)
        {
            if (hit.collider.TryGetComponent<idamagable>(out var enemy))
            {
                enemy.takephygicaldamage(100);
            }
        }

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            transform.position = Vector3.Lerp(startPos, targetPos, elapsed / dashDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPos;
        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

