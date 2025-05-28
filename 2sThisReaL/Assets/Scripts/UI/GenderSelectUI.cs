using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenderSelectUI : MonoBehaviour
{
    public void OnSelectMale()
    {
        CharacterManager.Instance.SetGender(GenderType.Male);
        Debug.Log("[UI] Male selected");
        SceneManager.LoadScene("Donghyun"); // 실제 게임 씬 이름
    }

    public void OnSelectFemale()
    {
        CharacterManager.Instance.SetGender(GenderType.Female);
        Debug.Log("[UI] Female selected");
        SceneManager.LoadScene("Donghyun"); // 실제 게임 씬 이름
    }
}
