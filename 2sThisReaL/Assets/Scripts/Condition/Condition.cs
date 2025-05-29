using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface idamagable
{
  void takephygicaldamage(int damage);
}
public class Condition : MonoBehaviour
{
    [SerializeField] private Image _imageHp;
    [SerializeField] private Image _imageStamina;
    [SerializeField] private Image _imageHunger;
    [SerializeField] private Image _imageThirsty;
    [SerializeField] private Image _imageTemprerature;

    [SerializeField] private ConditionManager gm;

    void Awake()
    {
        gm = ConditionManager.Instance;
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
    private void UpdateHP()
    {
        if (_imageHp != null)
            _imageHp.fillAmount = gm.curHp / gm.maxHp;
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
        if (_imageStamina != null)
            _imageStamina.fillAmount = gm.curStamina / gm.maxStamina;
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
            if (gm.curHp == 0)
            {
                //GameManager.player._condition.DIe();
            }
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
            if (gm.curHp == 0)
            {
                //gameManager.player._condition.DIe();
            }
        }
        UpdateThirsty();
    }
    private void UpdateThirsty()
    {
        if (_imageThirsty != null)
            _imageThirsty.fillAmount = gm.curThirsty / gm.maxThirsty;
    }
    #endregion

    #region Player Temperature
    private void DepletionTemperature() // 플레이어의 온도
    {
        Rigidbody rb = GameManager.Instance.Player.GetComponent<Rigidbody>();

        if (rb.velocity.magnitude < 0.1f)
        {
            gm.curTemperature -= gm.decreasingTemperature * Time.deltaTime;

            gm.curTemperature = Mathf.Clamp(gm.curTemperature, gm.minTemperature, gm.maxTemperature);
            UpdateTemperature();
        }
    }
    public void DeltaTemperature(float delta)
    {
        gm.curTemperature = Mathf.Clamp(gm.curTemperature + delta, 0, gm.maxTemperature);
        UpdateTemperature();
    }
    private void UpdateTemperature()
    {
        _imageTemprerature.fillAmount = (gm.curTemperature - gm.minTemperature) / (gm.maxTemperature - gm.minTemperature);
        // _imageTemprerature.fillAmount = Mathf.InverseLerp(gm.minTemperature, gm.maxTemperature, gm.curTemperature);
    }
    #endregion
}
