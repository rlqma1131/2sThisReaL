using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SnowballProjectile : MonoBehaviour
{
    public GameObject SnowballPrefab;

    public float speed = 12f;
    public float lifeTime = 3f;
    public int damage = 20;

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    public void Shoot()
    {
        Vector3 spawnOffset = transform.up * 5.2f + transform.forward * 1f; // 머리 위 + 살짝 앞
        Vector3 spawnPos = transform.position + spawnOffset;

        Instantiate(SnowballPrefab, spawnPos, Quaternion.LookRotation(transform.forward));
        Debug.Log("Snowball shot!");
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
        else if (other.CompareTag("Obstacle") || other.CompareTag("Enemy") || other.CompareTag("Untagged"))
        {
            Destroy(gameObject);
            Debug.Log("Snowball hit an obstacle or enemy!");
        }
    }
}
