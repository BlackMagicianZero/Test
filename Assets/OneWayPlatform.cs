using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    PlayerController playerController;
    PlatformEffector2D platformEffector;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            playerController = collision.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(playerController == null)
            return;
        if(playerController.fallThrough == true)
        {
            platformEffector.rotationalOffset = 180;
            playerController = null;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        playerController = null;
        platformEffector.rotationalOffset = 0;
    }
}
