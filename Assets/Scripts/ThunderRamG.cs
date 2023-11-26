using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class ThunderRamG : MonoBehaviour
{
    public float walkAcceleration = 50f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    Damageable damageable;
    Damageable playerdamageable;
    public Transform playerTransform;
    public float followDistance = 15f;
    public float jumpDistance = 2f;
    public float jumpForce = 5f;
    public float jumpCooldown = 3f;
    public float jumpTimer = 0f;
    private float SP1targetDetectionTimer = 0f;
    private float SP2targetDetectionTimer = 0f;
    

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        { 
            if(_walkDirection != value)
            {
                // Direction flipped
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if(value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if(value == WalkableDirection.Left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    { 
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public float AttackCooldown 
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        } 
    }
    public bool _SP_Atk1 = false;
    public bool SP_Atk1
    {
        get
        {
            return _SP_Atk1;
        }
        private set
        {
            _SP_Atk1 = value;
            animator.SetBool(AnimationStrings.hasSPAtk1, value);
        }
    }
    public bool _SP_Atk2 = false;
    public bool SP_Atk2
    {
        get
        {
            return _SP_Atk2;
        }
        private set
        {
            _SP_Atk2 = value;
            animator.SetBool(AnimationStrings.hasSPAtk2, value);
        }
    }
    public bool _Groggy = false;
    public bool Groggy
    {
        get
        {
            return _Groggy;
        }
        private set
        {
            _Groggy = value;
            animator.SetBool(AnimationStrings.hasGroggy, value);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerdamageable = player.GetComponent<Damageable>();
    }

    private IEnumerator SPATK1()
    {
        SP_Atk1 = true;
        yield return new WaitForSeconds(0.5f);
        SP_Atk1 = false;
        Groggy = true;
        yield return new WaitForSeconds(3f);
        Groggy = false;
        walkAcceleration = 50f;
    }
    private IEnumerator SPATK2()
    {
        SP_Atk2 = true;
        yield return new WaitForSeconds(0.5f);
        SP_Atk2 = false;
        Groggy = true;
        yield return new WaitForSeconds(3f);
        Groggy = false;
        walkAcceleration = 50f;
    }
    void Update()
    {
        // 번개 내려치기 로직
        if (!HasTarget)
        {
            SP1targetDetectionTimer += Time.deltaTime;
            if (SP1targetDetectionTimer >= 3f && SP_Atk2 == false)
            {
                SP1targetDetectionTimer = 0f;
                walkAcceleration = 0f;
                StartCoroutine(SPATK1());
            }
        }
        //

        // 번개 울음 로직
        if(!HasTarget)
        {
            float distanceToPlayerForLightning = Vector2.Distance(transform.position, playerTransform.position);
            if(distanceToPlayerForLightning < 7f && distanceToPlayerForLightning > 3f && SP_Atk1 == false)
            {
                walkAcceleration = 0f;
                StartCoroutine(SPATK2());
            }
        }
        //
        HasTarget = attackZone.detectedColliders.Count > 0;
        // 점프 쿨다운 타이머 갱신
        jumpTimer -= Time.deltaTime;

        // 플레이어가 추적 거리 안에 있는지 확인
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer < followDistance)
            {
                // 플레이어를 따라가도록 walkDirection 업데이트
                if (playerTransform.position.x > transform.position.x)
                {
                    WalkDirection = WalkableDirection.Right;
                }
                else
                {
                    WalkDirection = WalkableDirection.Left;
                }

                // 점프 쿨다운이 끝났고, 플레이어와의 거리가 일정 값 이상이면 점프
                if (!SP_Atk1 && !SP_Atk2 &&!Groggy && jumpTimer <= 0 && Mathf.Abs(playerTransform.position.y - transform.position.y) > jumpDistance)
                {
                    Jump();

                    // 점프 후 쿨다운을 설정한 값으로 초기화
                    jumpTimer = jumpCooldown;
                }
            }
        }
    }
    private void FixedUpdate()
    {

        if(touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();    
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove && touchingDirections.IsGrounded)
            {
                // 최대 속도 방향으로 가속
                rb.velocity = new Vector2(
                    Mathf.Clamp(rb.velocity.x + (walkAcceleration * walkDirectionVector.x * Time.fixedDeltaTime), -maxSpeed, maxSpeed),
                    rb.velocity.y);
            }
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
        // 넉백 후에 위치를 검사하여 타일 끝에 도달하면 위치를 조정
        CheckTileEdge();
    }

    private void CheckTileEdge()
    {
        float tileSize = 1.0f;

        // 현재 위치의 x 좌표를 기준으로 타일 끝에 도달했는지 검사
        float currentPositionX = transform.position.x;
        float remainder = Mathf.Abs(currentPositionX) % tileSize;

        if (remainder > tileSize / 2)
        {
            // 타일의 끝에 가까운 경우 위치를 조정
            float adjustment = tileSize - remainder;
            transform.position = new Vector2(currentPositionX + Mathf.Sign(currentPositionX) * adjustment, transform.position.y);
        }
    }
    public void OnCliffDetected()
    {
        if(touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
    private void Jump()
    {
        if (touchingDirections.IsGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
