using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingDoorScript : MonoBehaviour
{
    private Damageable ThunderRamGDamageable;
    Animator animator;
    public GameObject endimage;
    public GameObject bossHpbar;

    private void Awake()
    {
        GameObject ThunderRamG = GameObject.FindGameObjectWithTag("BOSS");
        ThunderRamGDamageable = ThunderRamG.GetComponent<Damageable>();
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        endimage.SetActive(false);
    }

    private void Update()
    {
        if(bossHpbar.activeSelf == false)
        {
            animator.SetBool("EndingDoorOpen",true);
        }
        else
        {
            animator.SetBool("EndingDoorOpen",false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ThunderRamGDamageable.Health <= 0 && collision.CompareTag("Player"))
        {
            endimage.SetActive(true);
        }    
    }
}
