using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartAndEnd : MonoBehaviour
{
    public void OnRestartButtonClicked()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ExitButtonClicked()
    {
        SceneManager.LoadScene("StartScene");
    }
}
