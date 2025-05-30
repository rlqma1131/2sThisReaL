﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;
 
    private PlayerController controller;
    //private Condition condition;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        //condition = GetComponent<Condition>();
    }

public void EquipNew(EquipItem data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipItem, equipParent).GetComponent<Equip>();

        //if (curEquip is EquipTool tool && tool.jumpBoostAmount != 0)
        //{
        //    controller.jumpPower += tool.jumpBoostAmount;
        //}

    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            //if (curEquip is EquipTool tool && tool.jumpBoostAmount != 0)
            //{
            //    controller.jumpPower -= tool.jumpBoostAmount;
            //}

            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            if (ConditionManager.Instance.curStamina <= 0) return;
            curEquip.OnAttackInput();
        }
    }
}
