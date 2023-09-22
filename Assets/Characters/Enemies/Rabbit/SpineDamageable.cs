using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Spine;
using Spine.Unity;

public class SpineDamageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    SkeletonAnimation skeletonAnimation; // SkeletonAnimation으로 변경

    [SerializeField]
    private int _maxHealth = 100;

    public int MaxHealth
    {
        get
        {
            return _maxHealth;
        }
        set
        {
            _maxHealth = value;
        }
    }

    [SerializeField]
    private int _health = 100;

    public int Health
    {
        get
        {
            return _health;
        }
        set
        {
            _health = value;
            healthChanged?.Invoke(_health, MaxHealth);

            // If health drops below 0, character is no longer alive
            if (_health <= 0)
            {
                IsAlive = false;
            }
        }
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvincible = false;

    private float timeSinceHit = 0;
    public float invincibilityTime = 0.25f;

    public bool IsAlive
    {
        get
        {
            return _isAlive;
        }
        set
        {
            _isAlive = value;
            Debug.Log("IsAlive set " + value);

            if (value == false)
            {
                damageableDeath.Invoke();
            }
        }
    }

    // The velocity should not be changed while this is true but needs to be respected by other physics components like
    // the player controller
    public bool LockVelocity
    {
        get
        {
            // Spine 애니메이션의 상태를 확인하여 LockVelocity를 설정
            if (skeletonAnimation != null)
            {
                // "lock_velocity_animation_name"은 Spine 애니메이션의 LockVelocity 상태를 나타내는 애니메이션 이름으로 변경하세요.
                return skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name.Equals("walk");
            }
            return false; // 예외 처리
        }
        set
        {
            // LockVelocity 상태를 설정하는 로직 추가
            if (skeletonAnimation != null)
            {
                if (value)
                {
                    // LockVelocity 상태를 나타내는 Spine 애니메이션을 재생
                    // "lock_velocity_animation_name"은 Spine 애니메이션의 LockVelocity 상태를 나타내는 애니메이션 이름으로 변경하세요.
                    skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
                }
                else
                {
                    // LockVelocity 상태를 해제하는 로직 추가
                    // 예를 들어, 다른 Spine 애니메이션으로 전환
                    // "idle_animation_name"은 Spine 애니메이션의 대기 애니메이션 이름으로 변경하세요.
                    skeletonAnimation.AnimationState.SetAnimation(0, "walk", true);
                }
            }
        }
    }

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    private void Update()
    {
        if (isInvincible)
        {
            if (timeSinceHit > invincibilityTime)
            {
                // Remove invincibility
                isInvincible = false;
                timeSinceHit = 0;
            }

            timeSinceHit += Time.deltaTime;
        }
    }

    // Returns whether the damageable took damage or not
    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvincible)
        {
            Health -= damage;
            isInvincible = true;

            // Notify other subscribed components that the damageable was hit to handle the knockback and such

            // 재생할 Spine 애니메이션 상태를 설정
            if (skeletonAnimation != null)
            {
                skeletonAnimation.AnimationState.SetAnimation(0, "hit_animation_name", false); // 'hit_animation_name'에 실제 Spine 애니메이션 이름을 넣으세요.
            }

            // Hit 이벤트 호출
            damageableHit?.Invoke(damage, knockback);

            // HP가 0 이하인 경우
            if (Health <= 0)
            {
                Die(); // Die 메서드 호출
            }

            return true;
        }

        // Unable to be hit
        return false;
    }
    private void Die()
    {
        IsAlive = false;

        // Spine 애니메이션으로 "DIE:re" 재생
        if (skeletonAnimation != null)
        {
            skeletonAnimation.AnimationState.SetAnimation(0, "die_animation_name", false); // 'die_animation_name'에 실제 Spine 애니메이션 이름을 넣으세요.
        }

        // Die 이벤트 호출
        damageableDeath?.Invoke();
    }

    // Returns whether the character was healed or not
    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            // CharacterEvents.characterHealed(gameObject, actualHeal); // CharacterEvents와 관련된 코드가 있다면 필요에 따라 수정
            return true;
        }

        return false;
    }
}
