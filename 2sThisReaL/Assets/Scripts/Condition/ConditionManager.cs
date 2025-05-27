using UnityEngine;

public class ConditionManager : MonoBehaviour
{
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


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Player._ConditionManager = this;
    }
}
