using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 10f;
    public float remainingJumps = 0;
    private bool isOnWallJumpCooldown = false;
    public float dashDistance = 5f; // 대쉬 거리
    public float dashDuration = 0.2f; // 대쉬 지속 시간
    private bool isDashing;
    public float dashCooldown = 3f; // 대쉬 쿨타임 (3초)
    private bool canDash = true; // 대쉬 사용 가능 여부를 나타내는 변수
    public float dashingPower = 0f;
    public float dashingTime = 0f;
    Vector2 moveInput;
    TouchingDirections touchingDirections;
    Damageable damageable;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    //임시방편 가이드
    public GameObject explainimage;
    //
    public GhostTrailEffect ghostTrailEffect;

    private GameObject currentOneWayPlatform;
    [SerializeField] private BoxCollider2D playerCollider;
    [SerializeField] private Collider2D wallCollider;

    private bool isJumpingOffWall = false;
    private bool ignoreCollisionsDuringJump = false;

    public bool hasDBJumpBuff = false;
    private IEnumerator DisableCollision()
    {
        BoxCollider2D platformCollider = currentOneWayPlatform.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCollider, platformCollider);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreCollision(playerCollider, platformCollider, false);
    }

    public float CurrentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }

    [SerializeField]
    private bool _isMoving = false;

    public bool IsMoving
    {
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }

    [SerializeField]
    private bool _isRunning = false;

    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if (_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
        wallCollider = GetComponentInChildren<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator PerformDashing()
    {
        damageable.isInvincible = true;
        canDash = false;
        animator.SetBool("dashing", true);
        isDashing =true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        animator.SetBool("dashing", false);
        yield return new WaitForSeconds(dashCooldown);
        canDash =true;
    }
    private void Update()
    {
        if(isDashing)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(PerformDashing());
        }
        //임시코드
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // 이미지의 활성/비활성을 토글합니다.
            if (explainimage != null)
            {
                bool isImageActive = !explainimage.activeSelf;
                explainimage.SetActive(isImageActive);

                if (isImageActive)
                {
                    Time.timeScale = 0f;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
        //
    }
   private void FixedUpdate()
    {
        if(isDashing)
        {
            return;
        }
        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (IsAlive)
        {
            ghostTrailEffect.makeGhost = true;
            IsMoving = moveInput != Vector2.zero;
            SetFacingDirection(moveInput);
        }
        else
        {
            ghostTrailEffect.makeGhost = false;
            IsMoving = false;
        }
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if (moveInput.x > 0 && !IsFacingRight)
        {
            IsFacingRight = true;
        }
        else if (moveInput.x < 0 && IsFacingRight)
        {
            IsFacingRight = false;
        }
    }
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationStrings.rangedAttackTrigger);
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsRunning = true;
        }
        else if (context.canceled)
        {
            IsRunning = false;
        }
    }
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started && CanMove)
        {
            if (touchingDirections.IsGrounded)
            {
                remainingJumps = 0;
                // DBJump 버프 아이템을 획득한 경우
                if (hasDBJumpBuff) 
                {
                    remainingJumps = 1; // DBJump 버프로 인해 2번 점프 가능
                }
                animator.SetTrigger(AnimationStrings.jump);
                Jump(jumpImpulse);
            }
            else if (remainingJumps > 0)
            {
                remainingJumps--;
                Jump(jumpImpulse);
            }
        }
    }
    private void Jump(float jumpForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
    public void DownJump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (currentOneWayPlatform != null && !currentOneWayPlatform.CompareTag("UPOnly") && !ignoreCollisionsDuringJump)
            {
                StartCoroutine(DisableCollision());
                animator.SetTrigger(AnimationStrings.jump);
            }
            else
            {
                Debug.Log("다운점프가 금지된 타일 위에 있거나 점프 중입니다.");
            }
        }
    }
    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fallen"))
        {
            // "fallen" 영역에 닿았을 때, 최대 체력의 10%를 감소.
            int maxHealth = damageable.MaxHealth;
            int healthToReduce = maxHealth / 10; // 최대 체력의 10%를 계산
            bool damageApplied = damageable.ApplyDamage(healthToReduce, Vector2.zero); // 체력 감소

            if (damageApplied)
            {
                // "respawn" 영역으로 이동
                MoveToRespawnZone();
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = collision.gameObject;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("OneWayPlatform"))
        {
            currentOneWayPlatform = null;
        }
    }

    private void MoveToRespawnZone()
    {
        // Respawn 영역을 찾기
        GameObject[] respawnAreas = GameObject.FindGameObjectsWithTag("RespawnArea");

        if (respawnAreas.Length > 0)
        {
            Vector3 respawnPosition = respawnAreas[0].transform.position;

            // 플레이어를 respawn 위치로 이동
            transform.position = respawnPosition;
        }
        else
        {
            // Respawn 영역을 찾지 못한 경우에 대한 예외 처리
            Debug.LogWarning("Respawn 영역을 찾을 수 없습니다.");
        }
    }
}