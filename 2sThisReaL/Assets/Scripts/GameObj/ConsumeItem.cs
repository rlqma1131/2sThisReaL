using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConsumableType
{
    // 허기, 갈증, 체력
    Hunger,
    Thirst,
    Health
}
[Serializable]
public class ItemDataConsumable
{
    public ConsumableType consumableType; // 소모품 종류 (허기, 갈증 등)
    public float consumableAmount; // 소모 시 회복되는 양 (허기, 갈증 등)
    public float consumableDuration; // 소모품 효과 지속시간 (초 단위로 회복량 지속 회복. 5초 동안 허기를 2%씩 회복 등)
    //Duration 0이면 즉시회복
}

[CreateAssetMenu(fileName = "ConsumableItem", menuName = "New Item/Consumable")]
public class ConsumeItem : ItemData
{
    [Header("소모품 설정")] //소모품 - 허기, 갈증. 회복량, 

    public ItemDataConsumable[] consumableData; // 소모품 데이터 배열 (허기, 갈증 등)


}
