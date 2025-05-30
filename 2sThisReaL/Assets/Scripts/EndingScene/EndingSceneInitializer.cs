using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EndingSceneInitializer : MonoBehaviour
{
    void Start()
    {
        // 시간 재개
        Time.timeScale = 1f;
        //커서 활성화
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
