using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class ThunderRamG : MonoBehaviour
{
    public float walkAcceleration = 75f;
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
    public GameObject bossHpbar;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerdamageable = player.GetComponent<Damageable>();
    }
    private bool hasSPAtk1 = false;
    private bool isSPATK1CorRunning = false;
    private float SPAtk1Timer = 3f;
    private bool hasSPAtk2 = false;
    private bool isSPATK2CorRunning = false;
    private bool hasGroggy = false;
    private bool hasHeal = false;
    private bool isHealCorRunning = false;
    private bool hasJump = false;

    private IEnumerator SPAtk1cor()
    {
        isSPATK1CorRunning = true;
        hasSPAtk1 = true;
        animator.SetBool("hasSPAtk1", hasSPAtk1);
        yield return new WaitForSeconds(1f);
        hasSPAtk1 = false;
        animator.SetBool("hasSPAtk1", hasSPAtk1);
        hasGroggy = true;
        animator.SetBool("hasGroggy", hasGroggy);
        yield return new WaitForSeconds(4f);
        hasGroggy = false;
        animator.SetBool("hasGroggy", hasGroggy);
        if(hasHeal == true)
        {
            StartCoroutine(SPHealcor());
        }
        SPAtk1Timer = 3f;
        isSPATK1CorRunning = false;
    }
    private IEnumerator SPAtk2cor()
    {
        isSPATK2CorRunning = true;
        float originalWalkAcceleration = walkAcceleration;
        hasSPAtk2 = true;
        animator.SetBool("hasSPAtk2", hasSPAtk2);
        yield return new WaitForSeconds(2.5f);
        hasSPAtk2 = false;
        animator.SetBool("hasSPAtk2", hasSPAtk2);
        hasGroggy = true;
        animator.SetBool("hasGroggy", hasGroggy);
        yield return new WaitForSeconds(4f);
        hasGroggy = false;
        animator.SetBool("hasGroggy", hasGroggy);
        if(hasHeal == true)
        {
            StartCoroutine(SPHealcor());
        }
        SPAtk1Timer = 3f;
        hasSPAtk1 = false;
        animator.SetBool("hasSPAtk1", hasSPAtk1);
        isSPATK2CorRunning = false;
    }

     private IEnumerator SPHealcor()
    {   
        isHealCorRunning = true;
        walkAcceleration = 0f;
        yield return new WaitForSeconds(1f);
        int healAmount = Mathf.CeilToInt(damageable.MaxHealth * 0.03f);
        damageable.Heal(healAmount);
        walkAcceleration = 75f;
        isHealCorRunning = false;
    }

    void Update()
    {
        // 번개 내려치기 로직
        if (!isSPATK1CorRunning)
        {
            SPAtk1Timer -= Time.deltaTime;
            if (SPAtk1Timer <= 0)
            {
                StartCoroutine(SPAtk1cor());
            }
        }
        //

        //번개 울음 로직
        if (!isSPATK2CorRunning)
        {   
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (walkAcceleration == 75f && distanceToPlayer >= 3f && distanceToPlayer <= 6f)
            {
                walkAcceleration = 0f;
                StartCoroutine(SPAtk2cor());
                SPAtk1Timer = 3f;
            }
        }
        if(hasSPAtk1 == true)
        {
            walkAcceleration = 0f;
        }
        else if(hasSPAtk2 == true)
        {
            walkAcceleration = 0f;
        }
        else if(hasGroggy == true)
        {
            walkAcceleration = 0f;
        }
        else
        {
            walkAcceleration = 75f;
        }
        //
        if(!isHealCorRunning && damageable.Health < damageable.MaxHealth / 2 )
        {
            hasHeal = true;
        }
        if(!isHealCorRunning && damageable.Health > damageable.MaxHealth / 2 )
        {
            hasHeal = false;
        }
        if(damageable.Health <=0 )
        {
            bossHpbar.SetActive(false);
        }

        // 플레이어가 추적로직
        HasTarget = attackZone.detectedColliders.Count > 0;
        jumpTimer -= Time.deltaTime;
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
            if (distanceToPlayer < followDistance)
            {
                if (!hasGroggy &&  !hasSPAtk1 && !hasSPAtk2)
                {
                    if (playerTransform.position.x > transform.position.x)
                    {
                        WalkDirection = WalkableDirection.Right;
                    }
                    else
                    {
                        WalkDirection = WalkableDirection.Left;
                    }
                }

                if (jumpTimer <= 0 && Mathf.Abs(playerTransform.position.y - transform.position.y) > jumpDistance)
                {
                    if (!hasGroggy && !hasSPAtk1 && !hasSPAtk2)
                    {
                        hasJump = true;
                        animator.SetBool("hasJump",hasJump);
                        Jump();
                    }
                }

                if (Mathf.Abs(playerTransform.position.y - transform.position.y) < jumpDistance)
                {
                    hasJump = false;
                    animator.SetBool("hasJump",hasJump);
                }
                if(jumpTimer > 0)
                {
                    hasJump = false;
                }
            }
        }
        //
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
        hasHeal = false;
        animator.SetBool("hasHeal", hasHeal);
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
            jumpTimer = jumpCooldown;
        }
    }
}
