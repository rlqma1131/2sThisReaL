using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public GameObject slotPanel;

    [Header("Select Item")]
    public TextMeshProUGUI selectdItemName;
    public TextMeshProUGUI selectdItemDescription;
    public TextMeshProUGUI selectdSetName;
    public TextMeshProUGUI selectdStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipeButton;
    public GameObject dropButton;

    private PlayerController controller;
    //private PlayerCondition condition;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
}
