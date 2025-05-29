using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildCraftRecipe", menuName = "Crafting/BuildCraftRecipe")]
public class BuildCraftRecipe : RecipeBase
{
    [Header("BuildCraftResult")]
    public BuildItemData buildItemData; // 결과로 나올 빌드 아이템 데이터
    public GameObject buildPrefab; // 빌드 프리팹, 빌드할 때 사용되는 오브젝트
}
