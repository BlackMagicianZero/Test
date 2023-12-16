using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndingDoorScript : MonoBehaviour
{
    private Damageable ThunderRamGDamageable;
    Animator animator;
    public GameObject endimage;
    private bool hasopen = false;

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
            hasopen = true;
            animator.SetBool("EndingDoorOpen",hasopen);
        }
        else
        {
            hasopen = false;
            animator.SetBool("EndingDoorOpen",hasopen);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasopen == true)
        {
            endimage.SetActive(true);
        }    
    }
}
