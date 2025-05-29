using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour, IDestructible
{
    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
