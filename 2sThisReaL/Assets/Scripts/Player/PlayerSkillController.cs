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
    void Update()
    {
        foreach (Skill skill in skills)
        {
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
        Debug.Log($"Dashing from {startPos} to {targetPos} (dir: {dashDir}, dist: {dashDistance})");

        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 1f, dashDir, dashDistance);
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
