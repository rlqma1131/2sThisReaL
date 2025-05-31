using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject startPanel;
    public GameObject settingPanel;
    public GameObject creditPanel;
    // 전환할 씬 이름 (인스펙터에서 설정)
    public string GenderSelect;

    // 버튼에서 호출할 메서드
    public void LoadGenderSelectUI()
    {
        SceneManager.LoadScene("GenderSelect");
    }
    public void LoadSettingScene()
    {
        settingPanel.SetActive(true);
        startPanel.SetActive(false);
    }

    public void Credit()
    {
        Debug.Log("크레딧 패널 활성화 시도!");
        creditPanel.SetActive(true);
        startPanel.SetActive(false);
    }
    public void OutCredit()
    {
        Debug.Log("크레딧 패널 활성화 시도!");
        creditPanel.SetActive(false);
        startPanel.SetActive(true);
    }
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
    }
}