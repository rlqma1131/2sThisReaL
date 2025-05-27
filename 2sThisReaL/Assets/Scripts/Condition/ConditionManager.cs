using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    [Header("HP")]
    public float curHp;
    public float maxHp;
    public float decreasingHP;

    [Header("Stamina")]
    public float curStamina;
    public float maxStamina;
    public float decreasingStamina;

    [Header("Hunger")]
    public float curHunger;
    public float maxHunger;
    public float decreasingHunger;

    [Header("Thirsty")]
    public float curThirsty;
    public float maxThirsty;
    public float decreasingThirsty;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
