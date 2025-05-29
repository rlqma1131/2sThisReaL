using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    // 외부에서 설정할 필요 없음 → 읽기 전용 프로퍼티로 변경
    public PlayerController controller { get; private set; }
    public Equipment Equip { get; private set; }

    // 이벤트로 사용하므로 외부에서 += 가능하도록 유지
    public Action additem;

    // ItemData는 인벤토리 아이템 정보 등으로 쓰는 경우가 많아 public 유지 (필요 시 캡슐화 가능)
    public ItemData itemData;

    // dropPosition은 외부에서 참조는 필요하지만 수정할 필요 없음
    [SerializeField] private Transform DropPosition;
    public Transform dropPosition => DropPosition;

    private void Awake()
    {
        GameManager.Instance.Player = this;

        controller = GetComponent<PlayerController>();
        Equip = GetComponent<Equipment>();
    }
}
