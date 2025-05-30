using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    // 크래프팅 시스템의 기본 골조
    // 재료 배열과 필요 개수의 배열을 매치시킴 (예: 재료[0]은 필요 개수[0]만큼 필요)
    // 레시피는 각각에 해당하는 것을 가져옴. BuiildCraftRecipe, CookingRecipe 등
    // 빌드 데이터는 데이터상으로만 추가되지만, 도구 제작 등은 완성 후 인벤토리에 추가되어야 함.
    // 외부에서 선택된 레시피를 받아와서 처리한다.

    public List<RecipeBase> recipes; // 크래프팅 레시피 목록
    public List<BuildCraftRecipe> buildCraftRecipes; // 빌드 크래프트 레시피 목록
    public RecipeBase curRecipe; // 현재 선택된 레시피


    public void MatchRecipe()
    {

    }


}
