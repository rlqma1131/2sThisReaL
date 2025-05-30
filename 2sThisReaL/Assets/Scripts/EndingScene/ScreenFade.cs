using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 2f;

    public void StartFadeToWhite(string sceneName)
    {
        StartCoroutine(FadeToWhiteCoroutine(sceneName));
    }

    private IEnumerator FadeToWhiteCoroutine(string sceneName)
    {
        float timer = 0f;
        Color startColor = fadeImage.color;
        Color targetColor = new Color(1f, 1f, 1f, 1f); // 완전한 흰색

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, targetColor, timer / fadeDuration);
            yield return null;
        }

        SceneManager.LoadScene(sceneName);
    }
}
