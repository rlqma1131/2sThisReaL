using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller { get; private set; }
    public Equipment Equip { get; private set; }

    public Action additem;

    public ItemData itemData;

    [SerializeField] private Transform DropPosition;
    public Transform dropPosition => DropPosition;

    private void Awake()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Init(this);

        controller = GetComponent<PlayerController>();
        Equip = GetComponent<Equipment>();
    }
}
