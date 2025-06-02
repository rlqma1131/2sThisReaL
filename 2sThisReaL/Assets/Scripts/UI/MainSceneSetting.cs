using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainSceneSetting : MonoBehaviour
{
    public GameObject setingPanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Debug.Log("p키 눌림");
            OnsetingPanel(); // P 키를 누르면 함수 호출
        }
    }

    public void OnsetingPanel()
    {
        if (setingPanel != null)
        {
            setingPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void OffsetingPanel()
    {
        if (setingPanel != null)
        {
            setingPanel.SetActive(false);
        }
    }
}
