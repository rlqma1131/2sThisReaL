using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTool : MonoBehaviour, IInteractable
{

    //제작대에 다가가서 상호작용을 하면 
    public string GetInteractPrompt()
    {
        return $"제작대와 상호작용 하려면 [E]를 눌러주세요."; //상호작용 프롬프트
    }

    public void OnInteract()
    {
        //제작대 상호작용> 제작 UI 띄우기

    }
}
