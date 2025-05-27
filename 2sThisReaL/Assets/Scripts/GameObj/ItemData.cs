using UnityEngine;

public enum ItemType
{
    // 자원(재료), 장비(장비-도구, 무기), 소모품, 자재. 기타
    Resource,
    Material,
    Equipable,
    Consumable,
    Etc
}


[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("기본 정보")]
    // 고유 ID를 추가하면 아이템 관리 및 저장/불러오기에 유용 (확장성 고려)
    public string id; // 아이템의 고유 식별자
    public string itemName; // 아이템 이름
    public Sprite itemIcon; // 아이템 아이콘
    [TextArea(3, 5)] // 인스펙터에서 여러 줄로 입력 가능
    public string itemDescription; // 아이템 설명
    public ItemType itemType; // 아이템 종류 (필수)

    [Header("인벤토리 설정")]
    public bool isStackable = true; // 겹치기 가능 여부 (기본값 true), 도구나 무기에서는 false로 변경
    public int maxStackSize = 20; // 최대 겹침 개수 (기본값 20)
    public GameObject dropPrefab; // 월드에 떨어뜨렸을 때 생성될 프리팹 (선택 사항)

    //[Header("제작품 설정")]
    //public bool isCraftable; // 제작 가능한 아이템인지 확인
    //public int craftAmount; // 제작 시 생성되는 개수
    //public ItemData[] craftResources; // 제작에 필요한 재료 아이템들 (배열로 설정 가능) - 도구도 해당 내용에 포함. 도구 Amounts는 반드시 1
    //public int[] craftResourceAmounts; // 각 재료 아이템의 필요 개수 (배열로 설정 가능, craftResources와 길이가 같아야 함)

    //활용 예시 : craftResources = [Wood, Stone], craftResourceAmounts = [2, 1] -> 2개의 나무와 1개의 돌이 필요
    //제작 스크립트 예시 : craftResources[i]인 개수가 craftResourceAmounts[i] 이상이라면 (currentAmount >= craftResourceAmounts[i]) 제작 가능

    //public ItemData craftResult; // 제작 결과 아이템 (필수)
    //public GameObject craftPrefab; // 제작 완료 시 생성될 프리팹

    //[Header("제작 도구 설정")]
    //public bool requiresTool; // 제작 시 도구가 필요한지 여부 (기본값 false)
    //public ItemData requiredTool; // 필요한 도구 아이템 (requiresTool이 true일 때만 사용됨)
    // 제작품 중 일부 아이템은 도구를 '장착'하고 있어야 제작 가능 (도끼 equipped = true 상태일 때 통나무>합판 제작가능)



    //[TextArea(3, 5)] // 인스펙터에서 여러 줄로 입력 가능
    //public string craftDescription; // 제작 설명 (선택 사항)

}

