using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "SwordAttack1")
        {
            animator.SetTrigger("hit");
        }
        if(collision.gameObject.name == "SwordAttack2")
        {
            animator.SetTrigger("hit");
        }
        if(collision.gameObject.name == "SwordAttack3")
        {
            animator.SetTrigger("hit");
        }
        if(collision.gameObject.name == "DashAttack")
        {
            animator.SetTrigger("hit");
        }
        if(collision.gameObject.name == "Sword_Air_Attack1")
        {
            animator.SetTrigger("hit");
        }
        if(collision.gameObject.name == "Sword_Air_Attack2")
        {
            animator.SetTrigger("hit");
        }
    }
}
