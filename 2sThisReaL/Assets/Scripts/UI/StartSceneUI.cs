using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // 전환할 씬 이름 (인스펙터에서 설정)
    public string GenderSelect;

    // 버튼에서 호출할 메서드
    public void LoadGenderSelectUI()
    {
        SceneManager.LoadScene("GenderSelect");
    }
}