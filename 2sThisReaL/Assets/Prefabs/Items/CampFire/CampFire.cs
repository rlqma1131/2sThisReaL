using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    private void OnTriggerEnter(Collider other)
    {
        ConditionManager.Instance.Condition.HealHP(-damage);
    }

}
