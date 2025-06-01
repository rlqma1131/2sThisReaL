using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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
    public float deltaTemperature;


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
            GameObject conditionObj = GameObject.Find("Conditions");
            if (conditionObj != null)
            {
                conditionObj.SetActive(false);
            }
        }

        if (scene.name == "MainScene")
        {
            // 🔧 Conditions 오브젝트 다시 켜기
            gameObject.SetActive(true); // ← ConditionManager는 Conditions 오브젝트에 붙어있음

            GameObject conditionObj = transform.Find("Condition")?.gameObject;
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
        count = 1;
    
        curHp = maxHp;
        curStamina = maxStamina;
        curHunger = maxHunger;
        curThirsty = maxThirsty;
        curTemperature = Mathf.Clamp((maxTemperature + minTemperature) / 2f, minTemperature, maxTemperature);
    
        // UI 다시 활성화
        if (Condition != null)
        {
            Condition.gameObject.SetActive(true);
        }

        gameObject.SetActive(true); // Conditions 다시 켜기
    }
}
