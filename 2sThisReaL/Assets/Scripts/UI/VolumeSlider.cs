using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SettingSceneUI : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_InputField volumeInputField;
    public GameObject SettingPanel;
    public GameObject StartPanel;

    private bool isUpdating = false;

    void Start()
    {
        float currentVolume = BGMManager.Instance.GetVolume();
        volumeSlider.value = currentVolume;
        volumeInputField.text = Mathf.RoundToInt(currentVolume * 100).ToString();

        volumeSlider.onValueChanged.AddListener(OnSliderChanged);
        volumeInputField.onEndEdit.AddListener(OnInputChanged);
    }

    void OnSliderChanged(float value)
    {
        if (isUpdating) return;
        isUpdating = true;

        int intValue = Mathf.RoundToInt(value * 100);
        volumeInputField.text = intValue.ToString();

        BGMManager.Instance.SetVolume(value);
        isUpdating = false;
    }

    void OnInputChanged(string input)
    {
        if (isUpdating) return;
        isUpdating = true;

        if (int.TryParse(input, out int intValue))
        {
            float sliderValue = Mathf.Clamp01(intValue / 100f);
            volumeSlider.value = sliderValue;
            BGMManager.Instance.SetVolume(sliderValue);
        }

        isUpdating = false;
    }

    public void BacktoNormalState()
    {
        SettingPanel.SetActive(false);
        if(StartPanel != null)
        {
            StartPanel.SetActive(true);
        }
    }
}
