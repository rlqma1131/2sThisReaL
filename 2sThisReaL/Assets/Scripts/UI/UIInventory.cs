using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private Condition Condition;
    private GameManager gameManager;
    private Equipment equipment;

    void Start()
    {
        StartCoroutine(WaitForGameManager());
    }

    // 선택한 아이템 표시할 정보창 Clear 함수
    void ClearSelectedItemWindow()
    {
        selectedItem = null;

        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty; ;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // 인벤토리창 Open/Close 시 호출
    public void Toggle()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeInHierarchy);
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }


    public void AddItem()
    {
        List<ItemData> queue = GameManager.Instance.Player.itemQueue;

        // 여러개 가질 수 있는 아이템인 경우
        foreach (ItemData data in queue)
        {
            if (data.isStackable)
            {
                ItemSlot stackSlot = GetItemStack(data);
                if (stackSlot != null)
                {
                    stackSlot.quantity++;
                    continue;
                }
            }

            // 빈 슬롯 찾기
            ItemSlot emptySlot = GetEmptySlot();

            // 빈 슬롯이 있을 때
            if (emptySlot != null)
            {
                emptySlot.item = data;
                emptySlot.quantity = 1;
                UpdateUI();
                continue;
            }

            // 빈 슬롯 마저 없을 때
            ThrowItem(data);

        }
        queue.Clear();
        UpdateUI();
    }

    // UI 정보 새로고침
    public void UpdateUI()
    {
        foreach (var slot in slots)
        {
            if (slot.item != null) slot.Set();
            else slot.Clear();
        }
    }

    // 여러개 가질 수 있는 아이템의 정보 찾고 return
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackSize)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 슬롯의 item 정보가 비어있는 정보 return
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    // 아이템 버리기
    public void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.identity);
    }


    // 선택한 아이템 정보창에 업데이트 해주는 함수
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.itemName;
        selectedItemDescription.text = selectedItem.item.itemDescription;

        selectedItemStatName.text = "";
        selectedItemStatValue.text = "";

        if (selectedItem.item.itemType.Contains(ItemType.Consumable))
        {
            ConsumeItem item = selectedItem.item as ConsumeItem;

            foreach (var c in item.consumableData)
            {
                selectedItemStatName.text += c.consumableType + "\n";
                selectedItemStatValue.text += c.consumableAmount + "\n";
            }
        }

        bool isEquipable = selectedItem.item.itemType.Contains(ItemType.Equipable);
        useButton.SetActive(selectedItem.item.itemType.Contains(ItemType.Consumable));
        equipButton.SetActive(isEquipable && !selectedItem.equipped);
        unEquipButton.SetActive(isEquipable && selectedItem.equipped);
        dropButton.SetActive(!selectedItem.equipped);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.itemType.Contains(ItemType.Consumable))
        {
            ConsumeItem item = selectedItem.item as ConsumeItem;
            if (item == null) return;

            foreach(ItemDataConsumable c in item.consumableData)
            {
                switch (c.consumableType)
                {
                    case ConsumableType.Health:
                        Condition.HealHP(c.consumableAmount);
                        break;
                    case ConsumableType.Hunger:
                        Condition.HealHunger(c.consumableAmount);
                        break;
                    case ConsumableType.Thirst:
                        Condition.HealThirsty(c.consumableAmount);
                        break;
                }
            }

            RemoveSelectedItem();
        }
    }

    public void OnEquipButton()
    {
        selectedItem.equipped = true;
        slots[selectedItemIndex].equipped = true;

        if (selectedItem.item is EquipItem equipItem)
        {
            equipment.EquipNew(equipItem);
        }

        UpdateUI();
        SelectItem(selectedItemIndex);
    }

    public void OnUnEquipButton()
    {
        selectedItem.equipped = false;
        slots[selectedItemIndex].equipped = false;
        if (selectedItem.item is EquipItem equipItem)
        {
            equipment.UnEquip();
        }
        UpdateUI();
        SelectItem(selectedItemIndex);
    }

    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }
    public bool HasItem(ItemData item, int quantity)
    {
        return false;
    }

    void RemoveSelectedItem()
    {

        selectedItem.quantity--;

        // 아이템 수량이 0일 때 삭제
        if (selectedItem.quantity <= 0)
        {
            if (slots[selectedItemIndex].equipped)
            {
                UnEquip(selectedItemIndex);
            }

            slots[selectedItemIndex].item = null;
            slots[selectedItemIndex].quantity = 0;
            slots[selectedItemIndex].Clear();
            selectedItem = null;
            ClearSelectedItemWindow();
        }
        if (!selectedItem.item.isStackable)
        {
            // 장비는 수량 유지
            selectedItem.quantity = 1;
        }
        UpdateUI();
    }
    public void UnEquip(int index)
    {
        slots[index].equipped = false;
        UpdateUI();
    }
    IEnumerator WaitForGameManager()
    {
        while (GameManager.Instance == null || GameManager.Instance.Player == null)
        {
            yield return null;
        }

        controller = GameManager.Instance.Player.controller;
        Condition = ConditionManager.Instance.Condition;
        dropPosition = GameManager.Instance.Player.dropPosition;
        GameManager.Instance.Player.additem -= AddItem; // 중복 등록 방지
        GameManager.Instance.Player.additem += AddItem;

        equipment = GameManager.Instance.Player.GetComponent<Equipment>();

        GameManager.Instance.uiInventory = GameObject.Find("UIInventory").GetComponent<UIInventory>(); //만약 UIInventory가 할당되지 않았다면 찾아서 할당;
        inventoryWindow.SetActive(false);
        controller.inventory += Toggle;

        slots = new ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }

        ClearSelectedItemWindow();
    }
}
