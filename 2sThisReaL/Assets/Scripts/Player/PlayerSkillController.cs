using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum SkillType
{
    Fireball
    
}


public class PlayerSkillController : MonoBehaviour
{
    public Skill[] skills;

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
        }
    }
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballSpawnPoint;
    void CastFireball()
    {
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, transform.rotation);
        Debug.Log("Fireball Cast!");
    }

}
