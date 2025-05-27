using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent;

    private PlayerController controller;
    private PlayerCondition condition;

    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();

        if (curEquip is EquipTool tool && tool.jumpBoostAmount != 0)
        {
            controller.jumpPower += tool.jumpBoostAmount;
        }

    }

    public void UnEquip()
    {
        if (curEquip != null)
        {
            if (curEquip is EquipTool tool && tool.jumpBoostAmount != 0)
            {
                controller.jumpPower -= tool.jumpBoostAmount;
            }

            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }
}*/
