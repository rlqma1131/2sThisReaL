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

    [SerializeField] private ConditionManager gm;
    // Start is called before the first frame update
    void Start()
    {
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
    }
    #region HP
    public void DeltaHP(float delta) // 데미지나 아이템 상호작용으로 인한 hp변화
    {
        gm.curHp = Mathf.Clamp(gm.curHp + delta, 0, gm.maxHp);
        Debug.Log("HP: " + gm.curHp);
        UpdateHP();
    }
    private void DepletionHP(float delta) // 배고픔과 갈증이 0일 때 지속적인 감소
    {
        gm.curHp = Mathf.Clamp(gm.curHp + delta * Time.deltaTime, 0, gm.maxHp);
        UpdateHP();
    }
    private void UpdateHP()
    {
        _imageHp.fillAmount = gm.curHp / gm.maxHp;
    }
    #endregion

    #region Stamina
    public void AddStamina(float delta) // 아이템으로 인한 회복
    {
        gm.curStamina = Mathf.Clamp(gm.curStamina + delta, 0f, gm.maxStamina);
        UpdateStamina();
    }
    public void DepletionStamina(float delta) // 플레이어의 동작에 따른 지속적인 감소
    {
        gm.curStamina = Mathf.Clamp(gm.curStamina + delta * Time.deltaTime, 0f, gm.maxStamina);
        UpdateStamina();
    }
    private void UpdateStamina()
    {
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
    public void DeltaHunger(float delta)
    {
        gm.curHunger = Mathf.Clamp(gm.curHunger + delta, 0, gm.maxHunger);
        UpdateHunger();
    }
    private void UpdateHunger()
    {
        _imageHunger.fillAmount = gm.curHunger / gm.maxHunger;
    }
    #endregion

    #region Thirsty
    public void DeltaThirsty(float delta)
    {
        gm.curThirsty = Mathf.Clamp(gm.curThirsty + delta, 0, gm.maxThirsty);
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
        if (_imageHunger != null)
            _imageThirsty.fillAmount = gm.curThirsty / gm.maxThirsty;
    }
    #endregion
}
