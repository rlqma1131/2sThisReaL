using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftSlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private Button selectButton;

    private RecipeBase recipeData;
    private int slotIndex; // 슬롯의 순서
    private bool isAvailable; // 제작 가능한 아이템이면 Alpha 1, 제작 불가능한 아이템이면 Alpha 0.4로 표시



    //Slot을 선택하면 다른 슬롯에 필요한 아이템 슬롯을 만들어 인스턴스
    //각 슬롯에는 아이템의 Icon과 개수를 보여주며, 버튼을 눌러 선택
    //선택된 아이템 유형 메뉴에 따라 나오는 슬롯이 바뀌어야함(BuileItem은 BuildItem들 제작법만, CookRecipe는 요리법만 등)

    public void SelectRecipe()
    {

    }

}
