using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    public Vector2 curMovementInput;
    public LayerMask groundLayerMask;
    public LayerMask buildingLayerMask;

    [Header("Look")]
    public Transform cameraContianer;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;

    public Action inventory;
    private Rigidbody _rigidbody;

    private float originalMoveSpeed;
    private Coroutine speedBoostRoutine;

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSmoothSpeed = 10f;
    [SerializeField] private Animator animator;

    [SerializeField] private float flashDistance = 5f;
    [SerializeField] private float flashCooldown = 2f;

    private bool canFlash = true;
    private bool isBuildMode = false;

    private bool isRecovering = false;


    private bool isCrouching = false;
    private float originalHeight;
    private Vector3 originalCenter;
    private CapsuleCollider capsuleCollider;

    [SerializeField] private float crouchHeight = 1.5f;
    [SerializeField] private Vector3 crouchCenter = new Vector3(0f, 1f, 0f);

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        capsuleCollider = GetComponent<CapsuleCollider>();
        originalHeight = capsuleCollider.height;
        originalCenter = capsuleCollider.center;

        originalMoveSpeed = moveSpeed;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        canLook = true;
    }

    void Update()
    {
        if (!isBuildMode)
        {
            if (ConditionManager.Instance.curStamina == 0)
            {
                curMovementInput = Vector2.zero;
                Debug.Log("스테미나가 바닥이라 움직일 수 없습니다.");
                StartCoroutine(MoveCoroutine(3, 10));
                // 밑에 함수 2 ~ 5초 후에 실행 되게끔
                
            }
            else
            {
                Move();
            }
            UpdateAnimation();
        }
    }

    public void SetBuildMode(bool active)
    {
        isBuildMode = active;    
    }
    IEnumerator MoveCoroutine(float delay, float amount)
    {
        if (isRecovering) yield break;

        isRecovering = true;

        yield return new WaitForSeconds(delay);

        ConditionManager.Instance.Condition.DeltaStamina(amount);
        isRecovering = false;
    }
    void Move()
    {
        if (curMovementInput.sqrMagnitude < 0.01f)
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            if(IsGrounded())
            {
                ConditionManager.Instance.Condition.DepletionStamina(15f);
            }
            return;
        }

        // 카메라 기준 수평 방향 추출
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        // 입력 기준으로 이동 방향 계산
        Vector3 moveDir = camForward * curMovementInput.y + camRight * curMovementInput.x;
        moveDir.Normalize();

        // 이동 처리
        Vector3 velocity = moveDir * moveSpeed;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;

        // 회전 보간 처리 (부드럽게 방향 전환)
        if (moveDir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
            ConditionManager.Instance.Condition.DepletionStamina(-5f);
        }

    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded())
        {
            if (ConditionManager.Instance.curStamina == 0)
            {
                Debug.Log("점프할 힘이 없슈");
                _rigidbody.AddForce(Vector2.up * 0, ForceMode.Impulse);
            }
            else
            {
                ConditionManager.Instance.Condition.DeltaStamina(-5f);
                _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            }

        }
        
    }
    bool IsGrounded()
    {
        LayerMask totalMask = groundLayerMask | buildingLayerMask;

        Ray[] rays = new Ray[4]
        {
        new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        foreach (Ray ray in rays)
        {
            if (Physics.Raycast(ray, 0.1f, totalMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCousor();
        }
    }

    void ToggleCousor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = toggle;  // if toggle == true, visible == true
        canLook = !toggle;
    }
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        if (speedBoostRoutine != null)
        {
            StopCoroutine(speedBoostRoutine);
        }
        speedBoostRoutine = StartCoroutine(SpeedBoostCoroutine(multiplier, duration));
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier, float duration)
    {
        moveSpeed = originalMoveSpeed * multiplier;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalMoveSpeed;
        speedBoostRoutine = null;
    }
    public void AddJumpPower(float amount)
    {
        jumpPower += amount;
    }

    public void SetJumpPower(float value)
    {
        jumpPower = value;
    }
    void UpdateAnimation()
    {
        // 지면 속도
        Vector3 flatVelocity = _rigidbody.velocity;
        flatVelocity.y = 0f;
        float speed = flatVelocity.magnitude;

        // 지면 체크
        bool isGrounded = IsGrounded();

        // Animator 파라미터 설정
        animator.SetFloat("MovingSpeed", speed);
        animator.SetBool("IsGround", isGrounded);
    }
    public void OnFlash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Flash();
        }
    }
    void Flash()
    {
        if (!canFlash) return;

        Vector3 blinkDir = transform.forward; // 또는 moveDir, camera 기준 등 선택 가능
        blinkDir.y = 0f;
        blinkDir.Normalize();

        // 순간 이동 위치 계산
        Vector3 targetPosition = transform.position + blinkDir * flashDistance;

        // 충돌 체크 (선택)
        if (Physics.Raycast(transform.position, blinkDir, out RaycastHit hit, flashDistance))
        {
            targetPosition = hit.point - blinkDir * 1f; // 벽 앞에 멈추기
        }

        transform.position = targetPosition;

        // 쿨타임 적용
        StartCoroutine(FlashCooldownRoutine());
    }

    IEnumerator FlashCooldownRoutine()
    {
        canFlash = false;
        yield return new WaitForSeconds(flashCooldown);
        canFlash = true;
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            animator.SetBool("IsAttack", true);
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            animator.SetBool("IsAttack", false); // 좌클릭 뗌
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
{
    if (context.phase == InputActionPhase.Started)
    {
        isCrouching = true;
        capsuleCollider.height = crouchHeight;
        capsuleCollider.center = crouchCenter;
        moveSpeed *= 0.5f; // 속도 느려짐
        animator.SetBool("IsCrouch", true);
    }
    else if (context.phase == InputActionPhase.Canceled)
    {
        isCrouching = false;
        capsuleCollider.height = originalHeight;
        capsuleCollider.center = originalCenter;
        moveSpeed = originalMoveSpeed;
        animator.SetBool("IsCrouch", false);
    }
}
}
