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

    public void OnShoot()
    {
        Instantiate(SnowballPrefab, transform.forward, transform.rotation);
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
        }
    }
}
