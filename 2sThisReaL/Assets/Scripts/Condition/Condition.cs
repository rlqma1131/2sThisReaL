using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using static UnityEditor.PlayerSettings;
using DG.Tweening;

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
    }
    void Update()
    {
        if (gm == null) return;
        DepletionHunger();
        DepletionThirsty();
        DepletionTemperature(0);
        UpdateHP();
        UpdateStamina();
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
        float remainHp = gm.curHp;

        // 3분할 HP바
        if (_hpTop != null)
        {
            float topHp = Mathf.Clamp(remainHp, 0, hpPerBar);
            _hpTop.DOFillAmount(topHp / hpPerBar, 0.3f);
            remainHp -= topHp;
        }

        if (_hpMiddle != null)
        {
            float middleHp = Mathf.Clamp(remainHp, 0, hpPerBar);
            _hpMiddle.DOFillAmount(middleHp / hpPerBar, 0.3f);
            remainHp -= middleHp;
        }

        if (_hpBottom != null)
        {
            float bottomHp = Mathf.Clamp(remainHp, 0, hpPerBar);
            _hpBottom.DOFillAmount(bottomHp / hpPerBar, 0.3f);
        }

        if (gm.curHp <= 0)
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
        float remainStamina = gm.curStamina;

        // 3분할 Stamina 바
        if (_staminaTop != null)
        {
            float topFill = Mathf.Clamp01(remainStamina / staminaPerBar);
            _staminaTop.DOFillAmount(topFill / staminaPerBar, 0.3f);
            remainStamina -= staminaPerBar;
        }

        if (_staminaMiddle != null)
        {
            float middleFill = Mathf.Clamp01(remainStamina / staminaPerBar);
            _staminaMiddle.DOFillAmount(middleFill / staminaPerBar, 0.3f);
            remainStamina -= staminaPerBar;
        }

        if (_staminaBottom != null)
        {
            float bottomFill = Mathf.Clamp01(remainStamina / staminaPerBar);
            _staminaBottom.DOFillAmount(bottomFill / staminaPerBar, 0.3f);
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
            _imageHunger.DOFillAmount(gm.curHunger / gm.maxHunger, 0.3f);
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
            _imageThirsty.DOFillAmount(gm.curThirsty / gm.maxThirsty, 0.3f);
    }
    #endregion

    #region Temperature
    private void DepletionTemperature(float delta) // 플레이어의 온도
    {
        if (gameManager == null || gameManager.Player == null)
        {
            Debug.LogWarning("GameManager 또는 Player가 null입니다.");
            return;
        }
        Rigidbody rb = gameManager.Player.GetComponent<Rigidbody>();
        if (rb.velocity.magnitude < 0.1f)
        {
            gm.curTemperature -= gm.decreasingTemperature * Time.deltaTime;
        }
        else
        {
            gm.curTemperature += (gm.decreasingTemperature - gm.limitTemperature + delta) * Time.deltaTime;
            // 움직일 때 상승하는 온도를 너무 빨리 상승하지 않게 limitTemperature변수로 제한을 해주고 불에 너무 가까이가거나 뜨거울 때 delta로 추가 상승
        }
        if (gm.curTemperature == 0)
        {
            //IsDie();
        }
        if (gm.curTemperature == gm.maxTemperature)
        {
            //IsDie();
        }
        gm.curTemperature = Mathf.Clamp(gm.curTemperature, 0, gm.maxTemperature);

        UpdateTemperature();
    }
    private void UpdateTemperature()
    {
        if (_imageTemperature != null)
            _imageTemperature.DOFillAmount(gm.curTemperature / gm.maxTemperature, 0.3f);
    }
    #endregion
}
