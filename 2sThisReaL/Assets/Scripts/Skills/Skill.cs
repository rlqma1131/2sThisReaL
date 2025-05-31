using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Skill
{
    public SkillType type;
    public float cooldown;
    public KeyCode activationKey;

    [HideInInspector] public float lastUsedTime;

    [Header("Optional UI")]
    public Image cooldownImage;
    public bool IsReady()
    {
        return Time.time >= lastUsedTime + cooldown;
    }

    public void Use()
    {
        lastUsedTime = Time.time;
    }
    public void UpdateCooldownUI()
    {
        if (cooldownImage == null) return;

        float elapsed = Time.time - lastUsedTime;
        float ratio = Mathf.Clamp01(elapsed / cooldown);
        cooldownImage.fillAmount = 0f + ratio;
    }
}