using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();//상호작용 프롬포트 가져오기
    public void OnInteract(); //상호작용시 데이터
}

public class ItemObject : MonoBehaviour
{
    public ItemData data; //아이템 데이터
    public string GetInteractPrompt()
    {
        string prompt = $"Press E to pick up\n{data.itemName}\n{data.itemDescription}"; //상호작용 프롬프트
        // 내용은 차후 변경
        return prompt;
    }

    public void OnInteract()
    {
        GameManager.Instance.player.itemData = data; //플레이어의 아이템 데이터에 해당 아이템 데이터 저장
        GameManager.Instance.player.additem?.Invoke(); //아이템 획득 이벤트 발생
        Destroy(gameObject); //아이템 오브젝트 삭제
    }
}
