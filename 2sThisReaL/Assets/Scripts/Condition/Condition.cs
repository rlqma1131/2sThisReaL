using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Condition : MonoBehaviour

{

    [SerializeField] private ConditionManager _conditionManager;
    [SerializeField] private Image _imageHp;
    [SerializeField] private Image _imageStamina;
    [SerializeField] private Image _imageHunger;
    [SerializeField] private Image _imageThirsty;


    private ConditionManager gm;

    void Start()
    {
        gm = GameManager.Instance.player._conditionManager;

        gm.curHp = gm.maxHp;
        gm.curStamina = gm.maxStamina;
        gm.curHunger = gm.maxHunger;
        gm.curThirsty = gm.maxThirsty;
    }
    void Update()
    {
        DepletionHunger();
        DepletionThirsty();
    }

    #region HP
    public void DeltaHP(float delta)
    {
        gm.curHp = Mathf.Clamp(gm.curHp + delta, 0, gm.maxHp);
        Update();
    }
    private void UpdateHP()
    {
        _imageHp.fillAmount = gm.curHp / gm.maxHp;
    }
    #endregion

    #region Stamina
    public void DeltaStamina(float delta)
    {

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
            DeltaHP(gm.decreasingHP);
            if (gm.curHp == 0)
            {
                //gameManager.player._condition.DIe();
            }
        }
        UpdateHunger();
    }
    public void DeltaHunger(float delta)
    {
        gm.curHunger = Mathf.Clamp(gm.curHunger + delta, 0, gm.maxHunger);
        UpdateStamina();
    }
    private void UpdateHunger()
    {
        _imageStamina.fillAmount = gm.curHunger / gm.maxHunger;
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
            DeltaHP(gm.decreasingHP);
            if (gm.curHp == 0)
            {
                //gameManager.player._condition.DIe();
            }
        }
        UpdateThirsty();
    }
    private void UpdateThirsty()
    {
        _imageThirsty.fillAmount = gm.curThirsty / gm.maxThirsty;
    }
    #endregion
}
