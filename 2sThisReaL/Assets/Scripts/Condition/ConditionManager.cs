using UnityEngine;
using System.Collections;

public class ConditionManager : MonoBehaviour
{
    public static ConditionManager Instance { get; set; }
    public Condition Condition { get; set; }

    [Header("HP")]
    public float curHp;
    public float maxHp;
    public float deltaHp; // 한번에 감소하거나 상승
    public float decreasingHP; // 지속적으로 감소하거나 상승

    [Header("Stamina")]
    public float curStamina;
    public float maxStamina;
    public float decreasingStamina; // 지속적으로 감소하거나 상승

    [Header("Hunger")]
    public float curHunger;
    public float maxHunger;
    public float decreasingHunger; // 지속적으로 감소하거나 상승

    [Header("Thirsty")]
    public float curThirsty;
    public float maxThirsty;
    public float decreasingThirsty; // 지속적으로 감소하거나 상승

    [Header("Temperature")] // 플레이어의 온도
    public float curTemperature;
    public float maxTemperature;
    public float decreasingTemperature;

    [Header("Thermometer")] // 맵의 온도계
    public float curThermometer;
    public float random = Random.Range(1,2);

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
