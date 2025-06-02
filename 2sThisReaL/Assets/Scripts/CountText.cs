using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountText : MonoBehaviour
{
    public TextMeshProUGUI dayText;

    // Start is called before the first frame update
    void Start()
    {
        if (ConditionManager.Instance != null)
        {
            UpdataDay(ConditionManager.Instance.count);
        }
        else
        {
            //Debug.LogWarning("ConditionManager.Instance is null");
        }
    }
    private void UpdataDay(float day)
    {
        dayText.text = $"Day : {day.ToString()}";
    }
}
