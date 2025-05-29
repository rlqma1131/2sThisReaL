using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class Skill
{
    public SkillType type;
    public InputActionReference inputAction;
    public float cooldown;

    private float lastUsedTime;

    public bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void Use()
    {
        lastUsedTime = Time.time;
    }
}