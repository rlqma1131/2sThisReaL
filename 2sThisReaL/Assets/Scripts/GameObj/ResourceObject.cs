using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceObject : MonoBehaviour
{
    //자원을 생성하는 오브젝트는 게임 플레이 중 랜덤 위치 생성
    //건물이 있는 곳에는 생성되지 않음
    public ItemData[] itemsToGive; //획득할 아이템들 배열
    public int quantityPerHit = 1; //한 번의 타격으로 획득할 아이템 수(기본값 1)
    public int capacity; //해당 오브젝트에서 획득할 수 있는 총량

    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        // 장착한 장비의 canGathering이 true일 때 획득 가능한 자원과 아닐 때의 자원 분리
        
        //만약 장착한 장비의 canGatheringd이 false라면 획득할 수 없는 아이템이 있음

        //일단 현재는 상관없이 획득한다고 함
        for (int i = 0; i < quantityPerHit; i++)
        {
            if (capacity <= 0) break; // 용량이 0 이하일 경우 더 이상 획득하지 않음
            // 아이템을 랜덤으로 선택
            ItemData itemToGive = itemsToGive[Random.Range(0, itemsToGive.Length)];

            //아이템 생성
            capacity--; // 획득할 때마다 용량 감소
            Instantiate(itemToGive.dropPrefab, hitPoint+Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }

        if(capacity <= 0)
        {
            Destroy(gameObject);
        }

    }


}
