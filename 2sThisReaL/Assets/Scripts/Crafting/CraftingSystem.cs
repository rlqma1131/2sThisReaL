using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    // 크래프팅 시스템의 기본 골조
    // 재료 배열과 필요 개수의 배열을 매치시킴 (예: 재료[0]은 필요 개수[0]만큼 필요)
    // 레시피는 각각에 해당하는 것을 가져옴. BuiildCraftRecipe, CookingRecipe 등
    // 빌드 데이터는 데이터상으로만 추가되지만, 도구 제작 등은 완성 후 인벤토리에 추가되어야 함.
    // 외부에서 선택된 레시피를 받아와서 처리한다.
    // 제작하기 버튼을 누르면 결과물을 정해진 곳에 추가한다. (BuildCraftRecipe는 ResourceManager에 데이터를 추가, 나머지는 인벤토리에 추가)
    // 레시피를 받으면 그만큼 UI에 아이템 슬롯을 만들어서 보여준다.

    [Header("제작 레시피 설정")]
    public List<RecipeBase> recipes; // 크래프팅 레시피 목록
    public List<BuildCraftRecipe> buildCraftRecipes; // 빌드 크래프트 레시피 목록
    public RecipeBase curRecipe; // 현재 선택된 레시피

    [Header("UI Slot 설정")]
    //RecipeSlot을 할당할 부모 컴포넌드
    public Transform recipeSlotParent;

    //재료를 할당할 부모 컴포넌트
    public Transform ResourcesParent;

    [Header("결과물 아이콘")]
    [SerializeField] private Image resultIcon; // 결과물 아이콘


    public void MatchRecipe()
    {

    }


}
