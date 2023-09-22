using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

[RequireComponent(typeof(Rigidbody2D), typeof(SpineTouchingDirections), typeof(SpineDamageable))]
public class Rabbit : MonoBehaviour
{
    private SkeletonAnimation skeletonAnimation;
    private string currentAnimation = "walk"; // 초기 애니메이션 설정

    public float walkAcceleration = 3f;
    public float maxSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone;

    private Rigidbody2D rb;
    private SpineTouchingDirections touchingDirections;
    private Animator animator;
    private SpineDamageable damageable;

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
            if (_walkDirection != value)
            {
                // 방향 뒤집기
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == WalkableDirection.Right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == WalkableDirection.Left)
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

    // CanMove 속성을 Spine 애니메이션 상태를 기반으로 설정
    public bool CanMove
    {
        get
        {
            // Spine 애니메이션의 상태를 확인하여 CanMove를 설정
            if (skeletonAnimation != null)
            {
                // "walk_animation_name"은 Spine 애니메이션의 걷기 애니메이션 이름으로 변경하세요.
                return !skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name.Equals("walk");
            }
            return false; // 예외 처리
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<SpineTouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<SpineDamageable>();

        // Spine 애니메이션 컴포넌트 초기화
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    void Update()
    {
        // 스파인 애니메이션을 여기서 제어
        // 예를 들어 HasTarget에 따라 애니메이션을 변경하려면:

        if (HasTarget)
        {
            SetAnimation("attack");
        }
        else
        {
            SetAnimation("walk");
            Debug.Log("Setting animation to: " + "walk");

        }

        // AttackCooldown 및 기타 로직을 업데이트할 수 있습니다.
    }

    private void SetAnimation(string animationName)
    {
        if (skeletonAnimation != null && !animationName.Equals(currentAnimation))
        {
            skeletonAnimation.AnimationState.SetAnimation(0, animationName, true);
            currentAnimation = animationName;
        }
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        if (!damageable.LockVelocity)
        {
                        Debug.Log("Setting animation to: " + "walk");

            if (CanMove && touchingDirections.IsGrounded)
            {
                // 최대 속도까지 가속
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
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
        else
        {
            Debug.LogError("현재 걸을 수 있는 방향이 오른쪽 또는 왼쪽으로 설정되지 않았습니다.");
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnCliffDetected()
    {
        if (touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
}
