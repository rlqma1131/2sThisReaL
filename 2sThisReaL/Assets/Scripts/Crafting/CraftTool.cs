using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTool : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject craftingUI; //제작 UI 오브젝트
    private bool craftMode;

    [Header("마우스 커서")]
    [SerializeField] private Texture2D craftingCursor; //제작커서

    private void Start()
    {        
        craftMode = false; //초기 제작 모드 비활성화
    }

    //제작대에 다가가서 상호작용을 하면 
    public string GetInteractPrompt()
    {
        return $"제작대와 상호작용 하려면 [E]를 눌러주세요."; //상호작용 프롬프트
    }

    public void OnInteract()
    {
        //제작대 상호작용> 제작 UI 띄우기
        //상호작용 시 UI를 활성화. 다시 한 번 사용 시 UI 비활성. UI가 활성화 되는 동안 캐릭터는 이동 금지
        
    }
}
