using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public ConditionManager _conditionManager;
    public PlayerController controller;
    //public PlayerCondition condition;
   // public Equipment equip;

    public ItemData itemData;
    public Action additem;

    public Transform dropPosition;
    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        //condition = GetComponent<PlayerCondition>();
        //equip = GetComponent<Equipment>();
    }
    private void Start()
    {
        GameManager.Instance.Init(this);
    }
}
