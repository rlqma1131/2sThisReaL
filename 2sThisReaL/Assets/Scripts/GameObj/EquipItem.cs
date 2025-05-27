using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EquipableItem", menuName = "New Item/Equipable")]
public class EquipItem : ItemData
{
    [Header("장비 설정")]
    public bool isEquipable; // 장비 여부 (기본값 false)
    // 장비 아이템은 도구와 무기로 분류, 도구라도 일단 '공격'은 가능함
    public bool canGathering; // 자원 채집 가능 여부 (true면 해당하는 아이템을 채집 가능)
    public ItemData canGet; // 해당 도구로 채집 가능한 자원 아이템 (예: 도끼로 나무를 베는 경우, canGet = Wood)
    public int attackPower; // 무기가 아니라도 공격력이 있는 아이템은 공격력 설정 가능 (기본값 0)
}
