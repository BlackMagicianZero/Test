using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Rabbit : MonoBehaviour
{
    private SkeletonAnimation spineAnimation;
    private Rigidbody2D rb;

    [SerializeField] private float walkSpeed = 2.0f;

    private void Start()
    {
        spineAnimation = GetComponent<SkeletonAnimation>();
        rb = GetComponent<Rigidbody2D>();
        
        // 시작 시 walk 애니메이션 재생
        PlayWalkAnimation();
    }

    private void Update()
    {
        // Walk 애니메이션을 플레이하며 양 옆으로 움직임
        Move(walkSpeed);

        // 움직임 로직을 추가하여 원하는 동작을 구현할 수 있습니다.
        // 예를 들어, 공격 범위에 플레이어가 있는지 확인하고 공격 로직을 추가할 수 있습니다.
        if (IsPlayerInAttackRange())
        {
            AttackPlayer();
        }
    }

    // 플레이어에게 피격될 때 호출하는 함수
    public void TakeDamage()
    {
        // Hit 애니메이션 재생
        PlayHitAnimation();
    }

    // 플레이어를 공격할 때 호출하는 함수
    public void AttackPlayer()
    {
        // Attack 애니메이션 재생
        PlayAttackAnimation();
    }

    // Walk 애니메이션 재생
    private void PlayWalkAnimation()
    {
        if (spineAnimation != null)
        {
            spineAnimation.AnimationState.SetAnimation(0, "walk", true);
        }
    }

    // Attack 애니메이션 재생
    private void PlayAttackAnimation()
    {
        if (spineAnimation != null)
        {
            spineAnimation.AnimationState.SetAnimation(0, "attack", false);
        }
    }

    // Hit 애니메이션 재생
    private void PlayHitAnimation()
    {
        if (spineAnimation != null)
        {
            spineAnimation.AnimationState.SetAnimation(0, "hit", false);
        }
    }

    // 움직임 함수
    private void Move(float speed)
    {
        // 이동 로직을 Rigidbody2D를 사용하여 구현
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    // 플레이어가 공격 범위에 있는지 확인하는 함수
    private bool IsPlayerInAttackRange()
    {
        // 원하는 공격 범위 확인 로직 추가
        // 예: 플레이어의 위치와 Rabbit의 위치를 비교하여 공격 범위에 있는지 확인
        // 만약 공격 범위 내에 플레이어가 있다면 true 반환, 그렇지 않으면 false 반환
        return false;
    }
}
