using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public Equipment equip;

    public ItemData itemData;
    public Action additem;

    public Transform dropPosition;
    private void Awake()
    {
        GameManager.Instance.Player = this;
        
        controller = GetComponent<PlayerController>();
        equip = GetComponent<Equipment>();
        GameManager.Instance.Player = this;
    }
}
