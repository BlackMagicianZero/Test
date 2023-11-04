using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject past_save_point;
    public GameObject pre_save_point;
    void Start()
    {

    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            past_save_point.SetActive(false);
            pre_save_point.SetActive(true);
        }
    }
}
