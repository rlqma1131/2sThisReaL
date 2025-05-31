using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Day : MonoBehaviour
{
    ConditionManager _coditionManager;

    [Range(0.0f, 1.0f)]
    public float time;
    public float startTime; // 게임을 시작할 때 몇시부터 시작할지
    public float dayLength; // 현실 시간 기준으로 게임 내의 하루가 몇 초일지 설정
    private float timeRate; //시간대
    public Vector3 noon; // 정오를 정하는 값 vecor3 90,0,0

    public TextMeshProUGUI dayText;

    private float dayCount = 0;
    private float prevTime = 0f;
    private bool isTextUi = false;


    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Ligth")]
    // 조명 밝기의 강도 조절
    // ex : 1.0이라면 평상시의 낮으로 기본 밝기의 값 , 0.3이라면 밤으로 밝기가 어두워짐
    public AnimationCurve lightIntensityMultiplier;
    // 반사의 강도 조절
    // ex : 1.0 일반 실내로 현실적인 반사 , 0.3 흐린날씨로 잘 보이지않는 반사 , 2.0 광택있는 표면으로 매우 선명한 반사로 눈부심 효과
    public AnimationCurve reflectionIntensityMultiplier;

    // AnimationCurve : UnityEngine에 정의된 class로 시간에 따른 값의 변화를 다루는 구조
    //                  빛의 강도가 시간에 따라 천천히 증가했다가 줄어들도록
    //                  점프할 때 높이 변화가 자연스럽게 곡선을 그리도록

    // Gradient : UnityEngine에 정의된 class로 시간에 따른 색상의 변화를 다루는 구조
    // Intensity : 조명이나 효과의 밝기, 강도 등을 나타내는

    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1.0f / dayLength;
        time = startTime;

        _coditionManager = ConditionManager.Instance;
        _coditionManager.count = 1;

        StartCoroutine(StartDayCoroutin(3));
    }
    // Update is called once per frame
    void Update()
    {
        // time에 timeRate를 계속해서 더해주고 하루(1.0)가 끝나면 0으로 루프(time %= 1.0f )
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLight(sun, sunColor, sunIntensity);
        UpdateLight(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);

        if (time < prevTime)
        {
            Debug.Log("하루가 지남");
            _coditionManager.count++;

            UpdataDay(_coditionManager.count);

            if (isTextUi == false)
            {
                isTextUi = true;
                StartCoroutine(DayTextCoroutin(3));
            }
        }
        prevTime = time;
    }

    private void UpdateLight(Light _lightSourec , Gradient _gradient , AnimationCurve _curve)
    {
        float _intensity = _curve.Evaluate(time);

        _lightSourec.transform.eulerAngles = (time - (_lightSourec == sun ? 0.25f : 0.75f)) * noon * 4f;
        _lightSourec.color = _gradient.Evaluate(time);
        _lightSourec.intensity = _intensity;

        GameObject go = _lightSourec.gameObject;
        if (_lightSourec.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (_lightSourec.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }    
    }
    private IEnumerator StartDayCoroutin(float _tiem)
    {
        UpdataDay(_coditionManager.count);
        dayText.gameObject.SetActive(true);

        yield return new WaitForSeconds(_tiem);

        dayText.gameObject.SetActive(false);
    }
    private IEnumerator DayTextCoroutin(float tiem)
    {
        dayText.gameObject.SetActive(true);

        yield return new WaitForSeconds(tiem);

        dayText.gameObject.SetActive(false);
        isTextUi = false;
    }
    private void UpdataDay(float day)
    {
        dayText.text = $"Day : {day.ToString()}";
    }
}
