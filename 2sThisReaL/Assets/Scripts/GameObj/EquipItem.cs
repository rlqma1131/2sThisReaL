using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EquipableItem", menuName = "New Item/Equipable")]
public class EquipItem : ItemData
{
    public float attackRate; // 공격 속도
    private bool attacking; // 공격 중인지 여부
    public float attackDistance; // 공격 거리

    [Header("장비 설정")]
    public bool isEquipable; // 장비 여부 (기본값 false)
    // 장비 아이템은 도구와 무기로 분류, 도구라도 일단 '공격'은 가능함
    public GameObject equipItem; // 장착할 때 사용할 프리팹 (선택 사항, 도구나 무기 아이템에 사용)
    public bool canGathering; // 자원 채집 가능 여부 (true면 해당하는 아이템을 채집 가능)
    public int attackPower; // 무기가 아니라도 공격력이 있는 아이템은 공격력 설정 가능 (기본값 0)
}
