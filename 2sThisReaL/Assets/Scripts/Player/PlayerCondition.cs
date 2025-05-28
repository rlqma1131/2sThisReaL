using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IDamagable
    {
        void TakePhygicalDamage(int damage);
    }

    public class PlayerCondition : MonoBehaviour, IDamagable
    {
        private ConditionManager gm;
        public float noHungerHealthDecay; // 배고픔이 0일 때 체력 감소 속도
        public event Action onTakeDamage;

        private void Awake()
        {
            gm = ConditionManager.Instance;
            if (gm == null)
            {
                enabled = false;
                return;
            }
        }

        private void Update()
        {
            // 배고픔 지속 감소
            gm.curHunger -= gm.decreasingHunger * Time.deltaTime;
            gm.curHunger = Mathf.Clamp(gm.curHunger, 0, gm.maxHunger);
            gm.Condition?.HealHunger(0); // UI 업데이트

            // 배고픔이 0이면 체력 감소
            if (gm.curHunger <= 0f)
            {
                gm.curHp -= noHungerHealthDecay * Time.deltaTime;
                gm.curHp = Mathf.Clamp(gm.curHp, 0, gm.maxHp);
                gm.Condition?.HealHP(0);
            }

            // 체력 0이 되면 사망
            if (gm.curHp <= 0f)
            {
                Die();
            }

            if (transform.position.y < -5f)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            gm.curHp = Mathf.Clamp(gm.curHp + amount, 0, gm.maxHp);
            gm.Condition.HealHP(0);
        }

        public void HealThirsty(float value)
        {
            gm.curThirsty = Mathf.Clamp(gm.curThirsty + value, 0, gm.maxThirsty);
            gm.Condition.HealThirsty(0);
        }

        public void Eat(float amount)
        {
            gm.curHunger = Mathf.Clamp(gm.curHunger + amount, 0, gm.maxHunger);
            gm.Condition.HealHunger(0);
        }

        public void TakePhygicalDamage(int damage)
        {
            gm.curHp = Mathf.Clamp(gm.curHp - damage, 0, gm.maxHp);
            gm.Condition.HealHP(0);
            onTakeDamage?.Invoke();
        }
        public void SpeedBoost(float multiplier, float duration)
        {
            var controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.ApplySpeedBoost(multiplier, duration);
            }
        }
        public void Die()
        {
            Debug.Log("사망");
        }
    }