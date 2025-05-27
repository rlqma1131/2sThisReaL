using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Condition : MonoBehaviour
{
    [SerializeField]
    private ConditionManager _conditionManager;

    [SerializeField] private Image _imageHp;
    [SerializeField] private Image _imageStamina;
    [SerializeField] private Image _imageHunger;
    [SerializeField] private Image _imageThirsty;

    // Start is called before the first frame update
    void Start()
    {
        _conditionManager.curHp = _conditionManager.maxHp;
        _conditionManager.curStamina = _conditionManager.maxStamina;
        _conditionManager.curHunger = _conditionManager.maxHunger;
        _conditionManager.curThirsty = _conditionManager.maxThirsty;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    #region HP
    public void DeltaHP(float delta)
    {

    }
    #endregion
}
