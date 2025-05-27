using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpPower;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

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

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        originalMoveSpeed = moveSpeed;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        UpdateAnimation();
    }

    private void LateUpdate()
    {

    }
    void Move()
    {
        if (curMovementInput.sqrMagnitude < 0.01f)
        {
            _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
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
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
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
        if(context.phase == InputActionPhase.Started && IsGrounded())
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
        }
    }
    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
        };

        for(int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
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
        Cursor.lockState = toggle?CursorLockMode.None : CursorLockMode.Locked;
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
        // 속도의 크기를 기준으로 MovingSpeed 파라미터 업데이트
        Vector3 flatVelocity = _rigidbody.velocity;
        flatVelocity.y = 0f; // 수직 속도 제외

        float speed = flatVelocity.magnitude; // 지면 기준 속도 크기
        animator.SetFloat("MovingSpeed", speed);
    }
}
