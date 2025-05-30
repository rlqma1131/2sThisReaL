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
        ItemData data = GameManager.Instance.Player.itemData;

        // 여러개 가질 수 있는 아이템인 경우
        if (data.isStackable)
        {
            ItemSlot stackSlot = GetItemStack(data);
            if (stackSlot != null)
            {
                stackSlot.quantity++;
                UpdateUI();
                GameManager.Instance.Player.itemData = null;
                return;
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
            GameManager.Instance.Player.itemData = null;
            return;
        }

        // 빈 슬롯 마저 없을 때
        ThrowItem(data);
        GameManager.Instance.Player.itemData = null;
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
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
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

        foreach (var c in selectedItem.item.consumables)
        {
            selectedItemStatName.text += c.type + "\n";
            selectedItemStatValue.text += c.value + "\n";
        }

        useButton.SetActive(selectedItem.item.ItemType.Contains(ItemType.Consumable));
        equipButton.SetActive(selectedItem.item.ItemType.Contains(ItemType.Equipable) && !slots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.ItemType.Contains(ItemType.Equipable) && slots[index].equipped);
        dropButton.SetActive(true);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.ItemType.Contains(ItemType.Consumable))
        {
            foreach (ItemDataConsumable c in selectedItem.item.consumables)
            {
                switch (c.type)
                {
                    case ConsumableType.Health:
                        Condition.HealHP(c.value);
                        break;
                    case ConsumableType.Hunger:
                        Condition.HealHunger(c.value);
                        break;
                    case ConsumableType.Thirst:
                        Condition.HealThirsty(c.value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    public void OnEquipButton()
    {
        selectedItem.equipped = true;
        UpdateUI();
    }

    public void OnUnEquipButton()
    {
        selectedItem.equipped = false;
        UpdateUI();
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
        else
            {
                // 장비는 수량이 0이 되어도 삭제하지 않음
                selectedItem.quantity = 1;
            }
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
        GameManager.Instance.Player.additem += AddItem;

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
