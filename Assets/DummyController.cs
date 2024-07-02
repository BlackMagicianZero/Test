using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    private Animator animator;

    public GameObject theplayer;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "SwordAttack1" || collision.gameObject.name == "SwordAttack2" ||
           collision.gameObject.name == "SwordAttack3" || collision.gameObject.name == "DashAttack" ||
           collision.gameObject.name == "Sword_Air_Attack1" || collision.gameObject.name == "Sword_Air_Attack2")
        {
            // 플레이어의 scale.x 값이 -1인 경우 "hit2" 트리거를 설정.
            if(theplayer.transform.localScale.x < 0)
            {
                animator.SetTrigger("hit2");
            }
            else
            {
                animator.SetTrigger("hit");
            }
        }
    }
}
