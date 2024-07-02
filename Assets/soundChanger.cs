using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundChanger : MonoBehaviour
{
    public GameObject past_sound;
    public GameObject next_sound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            past_sound.SetActive(false);
            next_sound.SetActive(true);
        }
    }
}
