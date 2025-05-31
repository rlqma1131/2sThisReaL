using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public List<RecipeBase> recipes; // 크래프팅 레시피 목록 <빈 리스트에 아래에 선택된 레시피를 추가하며 사용
    public List<BuildCraftRecipe> buildCraftRecipes; // 빌드 크래프트 레시피 목록
    // public List<CookingRecipe> cookingRecipes; // 요리 레시피 목록
    // public List<ToolRecipe> toolRecipes; // 도구 레시피 목록

    [Header("크래프팅 버튼")]
    [SerializeField] private Button BuildRecipeBtn; // 크래프팅 버튼
    [SerializeField] private Button CooktRecipeBtn; // 요리 버튼
    [SerializeField] private Button ToolRecipeBtn; // 도구 제작 버튼
    [SerializeField] private Button CraftButton; // 제작하기 버튼

    [Header("현재 선택된 레시피")]
    [SerializeField] RecipeBase curRecipe; // 현재 선택된 레시피
    [SerializeField] GameObject recipeSlotPrefab; // 레시피 슬롯 프리팹
    [SerializeField] GameObject resourceSlotPrefab; // 재료 슬롯 프리팹

    [Header("UI Slot 설정")]
    //RecipeSlot을 할당할 부모 컴포넌드
    public Transform recipeSlotParent;

    //재료를 할당할 부모 컴포넌트
    public Transform ResourcesParent;

    [Header("결과물 아이콘")]
    [SerializeField] private Image resultIcon; // 결과물 아이콘

    private void Start()
    {
        // 크래프팅 시스템 
    }

    // 레시피를 선택하면 해당 레시피에 맞는 재료 슬롯을 생성하고 UI를 업데이트
    public void SelectRecipe(RecipeBase recipe)
    {
        //recipe가 null이면 return;
        if (recipe == null) return;

        // 슬롯을 누르면, 해당 슬롯에 할당된 레시피를 업데이트
        curRecipe = recipe;

        // 현재 선택된 레시피에 따라 UI 업데이트
        UpdateRecipeUI();
    }

    public void SetRecipeUI()
    {
        // recipeSlotParent의 자식들을 모두 제거
        foreach (Transform child in recipeSlotParent)
        {
            Destroy(child.gameObject);
        }

        // 레시피 목록을 순회하며 UI에 레시피 슬롯을 생성. 어떤 버튼을 눌렀느냐에 따라 나타나는 Recipe가 달라짐.
        foreach (RecipeBase recipe in recipes)
        {
            // 레시피 슬롯을 생성
            GameObject slot = Instantiate(recipeSlotPrefab, recipeSlotParent);
            // 슬롯에 RecipeBase를 할당
            CraftSlot recipeSlot = slot.GetComponent<CraftSlot>();
            recipeSlot.SetRecipe(recipe);
            // 슬롯 클릭 이벤트 등록
            recipeSlot.OnClickButton();
        }

    }


    // 현재 선택된 레시피에 따라 UI를 업데이트
    public void UpdateRecipeUI()
    {
        // 선택된 레시피가 null이면 재료 슬롯을 비우고 결과 아이콘을 초기화
        // 기존 재료의 슬롯을 정리 (이전 레시피의 재료 슬롯을 제거)
        foreach (Transform child in ResourcesParent)
        {
            Destroy(child.gameObject);
        }


        // 현재 선택된 레시피가 null이 아니면 UI 업데이트
        if (curRecipe != null)
        {
            // 결과물 아이콘 업데이트
            resultIcon.sprite = curRecipe.recipeIcon;
            resultIcon.gameObject.SetActive(true);

            // 재료 슬롯 업데이트 (재료 아이콘과 개수 표시. 나무2, 돌1 등)
            for (int i = 0; i < curRecipe.requiredItems.Length; i++)
            {
                // 재료 아이템과 개수를 가져옴
                ItemData itemData = curRecipe.requiredItems[i];
                int requiredCount = curRecipe.requiredItemAmounts[i];

                //재료 슬롯 생성(개수가 늘어날 때마다 추가 생성)
                Instantiate(resourceSlotPrefab, ResourcesParent);
                //생성된 슬롯에 지정된 재료들의 아이콘과 개수를 설정

                // 슬롯 0번에 0번 아이템 데이터의 아이콘과 개수를 설정
                ResourceSlot slot = ResourcesParent.GetChild(i).GetComponent<ResourceSlot>();
                slot.SetItem(itemData, requiredCount);

                // 재료 슬롯을 생성하거나 업데이트하는 로직
                // 예: ResourcesParent에 재료 아이콘과 개수를 표시하는 UI 요소를 추가

            }

            //CraftSlot의 maxCount가 1이상이면, 제작 버튼 활성화

        }
        else
        {
            // 결과 아이콘 초기화
            resultIcon.sprite = null;
            //resultIcon의 오브젝트를 비활성화
            resultIcon.gameObject.SetActive(false);
        }
    }

    public void OnCraft()
    {
        if (curRecipe == null) return;

        // 인벤토리에서 재료가 충분한지 확인 - CraftSlot에서 재료를 확인하는 로직이 필요        

        // 현재 레시피에 따라 크래프팅 실행
        // 결과물이 BuildItem이면 ResourceManager에 추가
        // 결과물이 ToolRecipe나 CookingRecipe이면 인벤토리에 추가

        if (curRecipe is BuildCraftRecipe buildRecipe)
        {
            // 빌드 레시피인 경우, ResourceManager에 추가
            // ResourceManager에 빌드 아이템 데이터와 개수를 추가

        }
        //else if (curRecipe is ToolRecipe toolRecipe)
        //{
        //    // 도구 레시피인 경우, 인벤토리에 추가
        //    UIInventory.Instance.AddItem(toolRecipe.resultItem, toolRecipe.resultAmount);
        //    Debug.Log($"[CraftingSystem] 도구 레시피 완료: {toolRecipe.resultItem.itemName} x{toolRecipe.resultAmount}");
        //}
        //else if (curRecipe is CookingRecipe cookingRecipe)
        //{
        //    // 요리 레시피인 경우, 인벤토리에 추가
        //    UIInventory.Instance.AddItem(cookingRecipe.resultItem, cookingRecipe.resultAmount);
        //    Debug.Log($"[CraftingSystem] 요리 레시피 완료: {cookingRecipe.resultItem.itemName} x{cookingRecipe.resultAmount}");
        //}

    }


    public void OnBuildButton()
    {
        // 빌드 레시피 버튼을 클릭할 경우, 빌드 레시피배열을 레시피에 할당하고 SetRecipeUI를 호출
        curRecipe = null; // 현재 선택된 레시피 초기화
        recipes = buildCraftRecipes.Cast<RecipeBase>().ToList(); // 빌드 레시피를 레시피 목록에 할당
        SetRecipeUI(); // 레시피 UI 업데이트
    }

    void OnCookButton()
    {
        // 요리 레시피 버튼을 클릭할 경우, 요리 레시피를 레시피에 할당하고 SetRecipeUI를 호출
    }

    void OnToolButton()
    {
        // 도구 레시피 버튼을 클릭할 경우, 도구 레시피를 레시피에 할당하고 SetRecipeUI를 호출
    }

}
