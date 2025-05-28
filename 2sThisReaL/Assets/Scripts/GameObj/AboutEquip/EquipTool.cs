using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipTool : Equip
{
    // 캐릭터가 도구를 사용할 때 액션 등의 처리
    // 도구에는 Collider를 이용, IsTrigger Enter가 되면 공격 혹은 자원 채집이 됨 (Layer = Resorce면 자원이, Layer = Enemy면 공격이 됨)
    // 도구에 대한 내용은 EquipItem에서 설정하고 이곳에 받아옴

    public EquipItem equipData; // 도구 아이템 데이터
    private bool attacking; // 공격 중인지 여부. true면 공격이 실행되지 않음.
    private PlayerController controller; // 플레이어 컨트롤러 참조, 컨트롤러의 애니메이션이 진행되는 것= 공격임


    public override void OnAttackInput()
    {
        // 도구 사용 로직 처리
        if (!attacking)
        {
            attacking = true;           
            // 공격이 시작되면 애니메이션을 실행-플레이어의 공격로직
            controller.OnAttack(default);
            //공격이 끝나면 attacking을 false로 변경
            OncanAttack(); 

        }

    }

    void OncanAttack()
    {
        attacking = false; // 공격이 끝나면 attacking을 false로 설정
    }


}
