using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatfromEffector2DCheck : MonoBehaviour
{
    public bool playerCheck;
    PlatformEffector2D platformObject;
    PlayerController playerController;

    void Start()
    {
        playerCheck = false;
        platformObject = GetComponent<PlatformEffector2D>();
        playerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.S)&& playerCheck)
        {
            platformObject.rotationalOffset = 180f;
        }
        if(Input.GetKey(KeyCode.W)&& playerCheck)
        {
            platformObject.rotationalOffset = 0f;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerCheck = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        playerCheck = false;
    }
}
