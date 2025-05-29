using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountText : MonoBehaviour
{
    [SerializeField] private ConditionManager conditionManager;

    public TextMeshProUGUI dayText;

    // Start is called before the first frame update
    void Start()
    {
        UpdataDay(conditionManager.count);
    }
    private void UpdataDay(float day)
    {
        dayText.text = $"Day : {day.ToString()}";
    }
}
