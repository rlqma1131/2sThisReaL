using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public interface idamagable
{
  void takephygicaldamage(int damage);
}
public class Condition : MonoBehaviour
{
    [SerializeField] private Image _hpTop;
    [SerializeField] private Image _hpMiddle;
    [SerializeField] private Image _hpBottom;
    [SerializeField] private Image _staminaTop;
    [SerializeField] private Image _staminaMiddle;
    [SerializeField] private Image _staminaBottom;
    [SerializeField] private Image _imageHunger;
    [SerializeField] private Image _imageThirsty;
    [SerializeField] private Image _imageTemperature;

    [SerializeField] private ConditionManager gm;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject deathUI;
    [SerializeField] private GameObject MuzicPlayer;
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration = 2f;

    private bool isDead = false;
    void Awake()
    {
        gm = ConditionManager.Instance;
        gameManager = GameManager.Instance;
    }

    void Start()
    {
        if (gm == null)
        {
            enabled = false;
            return;
        }

        ConditionManager.Instance.Condition = this;

        gm.curHp = gm.maxHp;
        gm.curStamina = gm.maxStamina;
        gm.curHunger = gm.maxHunger;
        gm.curThirsty = gm.maxThirsty;
        gm.curTemperature = gm.maxTemperature;
    }
    void Update()
    {
        if (gm == null) return;
        DepletionHunger();
        DepletionThirsty();
        DepletionTemperature();
    }
    #region HP
    public void HealHP(float value) // 데미지나 아이템 상호작용으로 인한 hp변화
    {
        gm.curHp = Mathf.Clamp(gm.curHp + value, 0, gm.maxHp);
        UpdateHP();
    }
    private void DepletionHP(float value) // 배고픔과 갈증이 0일 때 지속적인 감소
    {
        gm.curHp = Mathf.Clamp(gm.curHp + value * Time.deltaTime, 0, gm.maxHp);
        UpdateHP();
    }
    public void IsDie()
    {
        if (isDead) return; // 중복 호출 방지

        isDead = true;

        if (MuzicPlayer != null)
        MuzicPlayer.SetActive(false);
        // 사망 UI 활성화
        StartCoroutine(FadeToBlack());
    }
    private IEnumerator FadeToBlack()
    {
        float elapsed = 0f;
        Color color = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime; // 시간 멈춰도 fade는 계속
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            fadePanel.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadePanel.color = new Color(color.r, color.g, color.b, 1f);

        yield return new WaitForSecondsRealtime(0.5f);

        SceneManager.LoadScene("EndingScene");
        // 마지막에 시간 정지
        Time.timeScale = 0f;

        // UI 등 추가 처리 가능
    }
    private void UpdateHP()
    {
        float hpPerBar = gm.maxHp / 3f;
        float curHp = gm.curHp;

        // 3분할 HP바
        if (_hpTop != null)
        {
            float topFill = Mathf.Clamp01(curHp / hpPerBar);
            _hpTop.fillAmount = topFill;
            curHp -= hpPerBar;
        }

        if (_hpMiddle != null)
        {
            float middleFill = Mathf.Clamp01(curHp / hpPerBar);
            _hpMiddle.fillAmount = middleFill;
            curHp -= hpPerBar;
        }

        if (_hpBottom != null)
        {
            float bottomFill = Mathf.Clamp01(curHp / hpPerBar);
            _hpBottom.fillAmount = bottomFill;
        }

        if(gm.curHp == 0)
        {
            IsDie();
        }
    }
    #endregion

    #region Stamina
    public void DeltaStamina(float delta) // 회복 및 감소
    {
        gm.curStamina = Mathf.Clamp(gm.curStamina + delta, 0f, gm.maxStamina);
        UpdateStamina();
    }
    public void DepletionStamina(float delta) // 플레이어의 동작에 따른 지속적인 감소 , 움직임이 없을 때 지속적인 증가
    {
        gm.curStamina = Mathf.Clamp(gm.curStamina + delta * Time.deltaTime, 0f, gm.maxStamina);
        UpdateStamina();
    }
    private void UpdateStamina()
    {
        float staminaPerBar = gm.maxStamina / 3f;
        float curStamina = gm.curStamina;

        // 3분할 Stamina 바
        if (_staminaTop != null)
        {
            float topFill = Mathf.Clamp01(curStamina / staminaPerBar);
            _staminaTop.fillAmount = topFill;
            curStamina -= staminaPerBar;
        }

        if (_staminaMiddle != null)
        {
            float middleFill = Mathf.Clamp01(curStamina / staminaPerBar);
            _staminaMiddle.fillAmount = middleFill;
            curStamina -= staminaPerBar;
        }

        if (_staminaBottom != null)
        {
            float bottomFill = Mathf.Clamp01(curStamina / staminaPerBar);
            _staminaBottom.fillAmount = bottomFill;
        }
    }
    #endregion

    #region Hunger
    private void DepletionHunger()
    {

        gm.curHunger -= gm.decreasingHunger * Time.deltaTime;
        gm.curHunger = Mathf.Clamp(gm.curHunger, 0, gm.maxHunger);

        if (gm.curHunger == 0)
        {
            DepletionHP(gm.decreasingHP);
        }
        UpdateHunger();
    }
    public void HealHunger(float value)
    {
        gm.curHunger = Mathf.Clamp(gm.curHunger + value, 0, gm.maxHunger);
        UpdateHunger();
    }
    private void UpdateHunger()
    {
        if (_imageHunger != null)
            _imageHunger.fillAmount = gm.curHunger / gm.maxHunger;
    }
    #endregion

    #region Thirsty
    public void HealThirsty(float value)
    {
        gm.curThirsty = Mathf.Clamp(gm.curThirsty + value, 0, gm.maxThirsty);
        UpdateThirsty();
    }
    private void DepletionThirsty()
    {
        gm.curThirsty -= gm.decreasingThirsty * Time.deltaTime;
        gm.curThirsty = Mathf.Clamp(gm.curThirsty, 0, gm.maxThirsty);

        if (gm.curThirsty == 0)
        {
            DepletionHP(gm.decreasingHP);

        }
        UpdateThirsty();
    }
    private void UpdateThirsty()
    {
        if (_imageThirsty != null)
            _imageThirsty.fillAmount = gm.curThirsty / gm.maxThirsty;
    }
    #endregion

    #region Temperature
    private void DepletionTemperature() // 플레이어의 온도
    {
        Rigidbody rb = gameManager.Player.GetComponent<Rigidbody>();
        if (rb.velocity.magnitude < 0.1f)
        {
            gm.curTemperature -= gm.decreasingTemperature * Time.deltaTime;
        }
        else
        {
            gm.curTemperature += gm.decreasingTemperature * Time.deltaTime;
        }
        gm.curTemperature = Mathf.Clamp(gm.curTemperature, 0, gm.maxTemperature);

        UpdateTemperature();
    }
    public void DeltaTemperature(float delta)
    {
        gm.curTemperature += (gm.decreasingTemperature + delta) * Time.deltaTime;
        gm.curTemperature = Mathf.Clamp(gm.curTemperature, 0, gm.maxTemperature);
        UpdateTemperature();
    }
    private void UpdateTemperature()
    {
        if (_imageTemperature != null)
            _imageTemperature.fillAmount = gm.curTemperature / gm.maxTemperature;
    }
    #endregion
}
