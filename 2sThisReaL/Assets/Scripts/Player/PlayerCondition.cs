//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public interface IDamagable
//{
//   void TakePhygicalDamage(int damage);
//}

//public class PlayerCondition : MonoBehaviour, IDamagable
//{
//    public UICondition uiCondition;

//    Condition health { get { return uiCondition.health; } }
//    Condition hunger { get { return uiCondition.hunger; } }

//    public float noHungerHealthDecay;

//    public event Action onTakeDamage;
//    void Update()
//    {
//        hunger.Subtract(hunger.passiveValue * Time.deltaTime * 0.1f);

//        if (hunger.curValue <= 0f)
//        {
//            health.Subtract(noHungerHealthDecay * Time.deltaTime);
//        }
//        if (health.curValue <= 0f)
//        {
//            Die();
//        }

//        if (transform.position.y < -5f)
//        {
//            Die();
//        }
//    }

//    public void Heal(float amount)
//    {
//        health.Add(amount);
//    }

//    public void eat(float amount)
//    {
//        hunger.Add(amount);
//    }
//    public void SpeedBoost(float multiplier, float duration)
//    {
//        var controller = GetComponent<PlayerController>();
//        if (controller != null)
//        {
//            controller.ApplySpeedBoost(multiplier, duration);
//        }
//    }
//    public void Die()
//    {
//        Debug.Log("사망");
//    }

//    public void TakePhygicalDamage(int damage)
//    {
//        health.Subtract(damage);
//        onTakeDamage?.Invoke();
//    }
//}
