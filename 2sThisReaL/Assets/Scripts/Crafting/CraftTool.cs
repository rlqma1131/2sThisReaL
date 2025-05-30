using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftTool : MonoBehaviour, IInteractable
{
    //제작대에 다가가서 상호작용을 하면 
    public string GetInteractPrompt()
    {
        throw new System.NotImplementedException();
    }

    public void OnInteract()
    {
        //제작대 상호작용> 제작 UI 띄우기

    }
}
