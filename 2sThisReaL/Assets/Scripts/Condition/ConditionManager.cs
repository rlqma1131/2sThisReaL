using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ConditionManager : MonoBehaviour
{
    public static ConditionManager Instance { get; set; }
    public Condition Condition { get; set; }
    public float count;
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
    public float minTemperature;
    public float decreasingTemperature;
    public float limitTemperature;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }

    void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "EndingScene")
        {
            GameObject conditionObj = GameObject.Find("Condition");
            if (conditionObj != null)
            {
                conditionObj.SetActive(false);
            }
        }

        if (scene.name == "MainScene")
        {
            GameObject conditionObj = GameObject.Find("Condition");
            if (conditionObj != null)
            {
                conditionObj.SetActive(true);
            }

            // 초기화도 여기서 안전하게 가능
            ResetCondition();
        }
    }
    public void ResetCondition()
    {
        count = 0;

        curHp = maxHp;
        curStamina = maxStamina;
        curHunger = maxHunger;
        curThirsty = maxThirsty;
        curTemperature = Mathf.Clamp((maxTemperature + minTemperature) / 2f, minTemperature, maxTemperature);

        if (Condition != null)
        {
            Condition.gameObject.SetActive(true); // UI가 꺼져있다면 켬
        }

        GameObject conditionUI = GameObject.Find("Condition");
        if (conditionUI != null)
        {
            conditionUI.SetActive(true); // UI 트리 복구
        }
    }
}
