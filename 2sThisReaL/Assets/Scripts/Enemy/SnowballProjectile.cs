using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SnowballProjectile : MonoBehaviour
{
    public GameObject SnowballPrefab;
    public float power;
    public float lifeTime = 3f;
    public int damage = 20;
    public void Shoot(Transform EnemyTransform)
    {
        Debug.Log("Shoot Snowball Projectile");
        Vector3 spawnOffset = EnemyTransform.forward * 1f + Vector3.up * 5.2f;
        GameObject snowball = Instantiate(SnowballPrefab, EnemyTransform.position + spawnOffset, transform.rotation);
        Debug.Log("Snowball Spawned at: " + (EnemyTransform.position + spawnOffset));
        Rigidbody rb = snowball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(EnemyTransform.forward * power, ForceMode.Impulse);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (GameManager.Instance.Player != null)
            {
                ConditionManager.Instance.Condition.HealHP(-damage);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Obstacle") || other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}