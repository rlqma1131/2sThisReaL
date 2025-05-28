using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    // 캐릭터가 도구를 사용할 때 액션 등의 처리
    // 도구에는 Collider를 이용, IsTrigger Enter가 되면 공격 혹은 자원 채집이 됨 (Layer = Resorce면 자원이, Layer = Enemy면 공격이 됨)
    // 도구에 대한 내용은 EquipItem에서 설정하고 이곳에 받아옴

    public EquipItem equipData; // 도구 아이템 데이터
    private bool attacking; // 공격 중인지 여부. true면 공격이 실행되지 않음. - 애니메이션 쿨이 다 돌 때까지는 attcking 상태.
    private PlayerController controller; // 플레이어 컨트롤러 참조, 컨트롤러의 애니메이션이 진행되는 것= 공격임
    private Player player; //플레이어 참조. 플레이어의 현재 장비 확인

    private void Awake()
    {
        controller = GetComponentInParent<PlayerController>();
    }
    public override void OnAttackInput()
    {
        // 도구 사용 로직 처리
        if (!attacking)
        {
            attacking = true;           
            // 공격이 시작되면 애니메이션을 실행-플레이어의 공격로직
            controller.OnAttack(default);
        }
    }

    //콜라이더랑 부딪히면 - 자원 채집 혹은 공격
    //Layer = Resource면 자원 채집, Layer = Enemy면 공격

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Resource") && equipData.canGathering)
        {
            // 자원 채집 로직 처리
            Debug.Log("자원 채집: " + other.name);
            ResourceObject resource = other.GetComponent<ResourceObject>();
            if (resource != null)
            {
                // 자원 획득
                resource.Gather(transform.position, transform.forward);
            }
            // 자원 채집 관련 코드 추가
        }
        else if (other.CompareTag("Enemy") && equipData.attackPower > 0)
        {
            //에너미는 layer가 아니라 tag로 구분
            // 공격 로직 처리
            Debug.Log("공격: " + other.name);
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                // 적에게 피해를 입힘
                enemy.TakeDamage(equipData.attackPower);
            }
            // 공격 관련 코드 추가
        }

        //공격이 끝나면 attacking을 false로 변경
        OncanAttack();
    }


    void OncanAttack()
    {
        attacking = false; // 공격이 끝나면 attacking을 false로 설정
    }


}
