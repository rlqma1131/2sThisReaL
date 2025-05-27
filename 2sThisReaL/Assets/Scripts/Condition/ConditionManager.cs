using UnityEngine;

public class ConditionManager : MonoBehaviour
{
    [Header("HP")]
    public float curHp;
    public float maxHp;
    public float deltaHp; // �ѹ��� �����ϰų� ���
    public float decreasingHP; // ���������� �����ϰų� ���

    [Header("Stamina")]
    public float curStamina;
    public float maxStamina;
    public float decreasingStamina; // ���������� �����ϰų� ���

    [Header("Hunger")]
    public float curHunger;
    public float maxHunger;
    public float decreasingHunger; // ���������� �����ϰų� ���

    [Header("Thirsty")]
    public float curThirsty;
    public float maxThirsty;
    public float decreasingThirsty; // ���������� �����ϰų� ���


    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.Player._ConditionManager = this;
    }
}
