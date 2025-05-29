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
        controller = GetComponent<PlayerController>();
        equip = GetComponent<Equipment>();
    }

    IEnumerator Start()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }
        GameManager.Instance.Init(this);
    }
}
