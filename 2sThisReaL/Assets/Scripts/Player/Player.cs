using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform DropPosition;
    public Transform dropPosition => DropPosition;
    public PlayerController controller { get; private set; }
    public Equipment Equip { get; private set; }

    public Action additem;

    public ItemData itemData;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        Equip = GetComponent<Equipment>();
    }

    IEnumerator Start()
    {
        while (GameManager.Instance == null)
        {
            yield return null;
        }
        GameManager.Instance.Init(this);
        Debug.Log("Player에서 인스턴스 할당됨");
    }
}
