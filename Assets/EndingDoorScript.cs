using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingDoorScript : MonoBehaviour
{
    private Damageable ThunderRamGDamageable;
    Animator animator;
    public GameObject endimage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        endimage.SetActive(false);
    }

    private void Update()
    {
        GameObject ThunderRamG = GameObject.FindGameObjectWithTag("BOSS");
        if(!ThunderRamG)
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
