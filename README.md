# 😎 팀 명
### 2ㄱㅈㅉㅇㅇ? (이거 진짜예요?)
### 팀원
> 김동현(팀장): Player, Skill, Interaction   
> 박도운: Map Design, Build Mode   
> 전기연: GameObject, Crafting System, Resource Manage   
> 차우진: Condition, Environment, Skybox(Day/Night)   
> 최다혜: Enemy, Enemy Spawn(AI Navigation), NPC   
> 한예림: UI, UI Design, Inventory, NPC   


# 🖥️ 프로젝트 명
### TrueManShow
#### 영화 '트루먼쇼' 모티브, 설원 속에서 살아남아 세상의 끝을 찾아가는 이야기.
![StartImg](https://github.com/user-attachments/assets/12a464f7-5fbf-4a1a-aa04-a82345808fb7)

> 선택 사유   
> 만든다면 Survival 장르가 가장 끌린다.   
> 지금까지 배운 다양한 기능을 구현할 수 있을 것 같다.   


# 프로젝트 소개
## 게임 시작
![image](https://github.com/user-attachments/assets/6112c705-e8da-44cd-9840-7a2acf14f26e)

## 캐릭터 선택   
https://github.com/user-attachments/assets/1f1a0a1a-5dba-4715-9026-0db4464be2fd



https://github.com/user-attachments/assets/55c5ff58-cf9c-4d06-818c-bd8349ac9346



> 남성/여성 중 원하는 캐릭터를 선택할 수 있습니다.   
> 선택한 캐릭터는 서로 다른 모습을 가집니다.   

## 상호작용

### 아이템 줍기



https://github.com/user-attachments/assets/d9262789-1723-4993-bc69-aab46cfb3d1e


### NPC와 대화



https://github.com/user-attachments/assets/62694dc6-4d1d-4f77-a31a-f04f01054304



### 제작 시스템 활성화



https://github.com/user-attachments/assets/7310d081-27c8-4fdc-ad26-89d84420a959



> IInteractable 인터페이스와, Interactable Layer, Interaction.cs를 이용하여 상호작용을 구현했습니다.   
> [E] 키를 눌러 아이템을 줍거나, 제작을 하거나, NPC와 대화를 하는 등 상호작용이 가능합니다.   
> NPC와의 대화에서 어떤 대답을 하느냐에 따라 얻을 수 있는 아이템이 달라집니다.   

## Condition

![image](https://github.com/user-attachments/assets/f0f5b71f-08c5-4033-973d-13022669c441)




https://github.com/user-attachments/assets/895203f1-2fe8-4eae-9412-dec9d281ee8e




> 붉은색 상태바: Health (HP)   
> 파란색 상태바: Stamina (SP)   
> 고기 아이콘: Hungry (허기)   
> 물 아이콘: Thirsty (갈증)   
> 눈송이 아이콘: Temperature (기온)   
> 불꽃 아이콘: FireBall (스킬-화염구)   
> 회오리 아이콘: DashAttack (스킬 - 대쉬어택<질풍참>)   
   
>> HP가 0이 되면 사망합니다.   
>> SP가 0이 되면 잠시 움직일 수 없게 됩니다.   
>> Hunger/Thirsty가 0이 되면 HP가 크게 깎입니다.   
>> Temperature가 너무 낮거나 높아지면 HP가 깎입니다.

# 스킬



https://github.com/user-attachments/assets/2e44bbd3-790b-4bfe-a30c-39f2c8c16052



> [Q], 그리고 [F] 키로 스킬을 사용할 수 있습니다.
> 긱 스킬은 공격력과 쿨타임을 가지고 있습니다.
> 스킬은 스태미나를 소모합니다.

## 자원 채집



https://github.com/user-attachments/assets/547a8cd8-93af-462c-bfdc-7e34955e4718



> 도끼, 혹은 곡괭이를 들고 나무 앞에서 마우스 왼쪽 클릭을 하면 자원을 채집할 수 있습니다.   
> 자원이 채집되면, 일정 시간동안 해당 위치에 생성되지 않습니다. (Coroutine 사용)   
> 자원이 생성될 위치(영상 좌측, 녹색 BoxCollider)에 Player나 Enemy가 존재할 경우, 자원의 생성 시간이 지연됩니다.   
>> 해당 범위에 Player나 Enemy가 없을 때까지 지속적으로 지연시킵니다.

## 인벤토리



https://github.com/user-attachments/assets/58a4ccdf-772d-47fe-acea-0e5969168be1



> [I] 키를 이용해 열고 닫을 수 있습니다.
> 자원 채집, 제작대에서 제작한 아이템들을 보관합니다.
> 장비 아이템의 장비/해제와 소비 아이템를 사용하는 기능을 수행합니다.

## 제작

https://github.com/user-attachments/assets/3a2ae859-ac83-4c60-8f6d-59c3f2bf767d

> 제작대에서 부를 수 있는 제작 UI에서 아이템을 제작할 수 있습니다.   
> 우측 상단에는 1회 제작에 만들어낼 수 있는 개수, 좌측 하단에는 제작 가능한 횟수가 표시됩니다.   
> 각 레시피는 RecipeBase를 기반으로, Build/Cook/Tool(건축/요리/도구)의 세 분야로 나누어 상속받습니다.
> 레시피 슬롯은 원하는 분야를 선택할 때마다 바뀝니다.
> 사용한 아이템은 소모되며, 그에 따라 다른 레시피들도 제작 가능 여부가 업데이트 됩니다.


## 적 Enemy

https://github.com/user-attachments/assets/3c90388e-bf3b-4164-8443-0e631bfabe7a


> 플레이어의 위치의 일정 범위 내에서 적 개체를 생성합니다.   
>> Enemy 종류: 게, 해골, 꿀벌, 예티 등   
> 베이스 캠프에서 멀어질 수록 더 강한 에너미가 스폰됩니다.  
> 에너미가 사망할 경우 몇 가지의 아이템을 드랍합니다.   
> AI Navigation을 이용하여, Enemy는 Player를 추적하고 갈 수 있는 길에서만 생성 및 이동합니다.   

## 건축모드



https://github.com/user-attachments/assets/5d7f2b97-8062-442b-bd36-dc1a46ef01ef



https://github.com/user-attachments/assets/42c5f0ad-9d8a-433c-a28d-c0dada20c74f




> [B]버튼을 누르면 건축모드로 변합니다.   
> 건축 모드 상태에서 [N]을 누르면 카탈로그가 열립니다.   
> 제작대에서 제작한 건축 아이템들은 전부 건축모드의 카탈로그에 저장됩니다.   
> 건축 아이템들의 배치를 조합하여 나만의 집을 만들 수 있습니다.   




# CommitConvention

### feat
새로운 기능 추가
### fix
버그 수정
### refactor
코드 리팩토링 ( 기능 변화 x )
### style
코드 스타일 변경 ( 공백, 세미콜론 등 )
### docs
문서 내용 수정 (README(내용), 주석 등)
### test
테스크 코드 추가/수정
### chore
기능, 버그 수정과 무관한 기타 잡일
 .gitignore, README(위치), Notion 등
### perf
성능 개선
### ci
CI 설정 관련 변경
### build
빌드 시스템 관련 변경
### revert
이전 커밋 되돌리기
### add
오브젝트 혹은 파일 추가
### remove
기존 오브젝트 혹은 파일 삭제
