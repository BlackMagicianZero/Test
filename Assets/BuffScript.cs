using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffScript : MonoBehaviour
{
    // 버프 목록 enum 화
    public enum PowerUp { DBJump, BlinkRange, BlinkCoolTime }
    public PowerUp powerups;

    // 버프 시스템 수치 조정하는 곳 변수만들어서 switch문에 꽂아넣을 것.
    public float JumpToGive;
    public float blinkRGToGive;
    public float blinkClToGive;
    //-------------------------------------------

    private PlayerController playercontroller;

    private void Awake()
    {
        playercontroller = FindObjectOfType<PlayerController>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            switch (powerups)
            {
                case PowerUp.DBJump:
                    playercontroller.hasDBJumpBuff = true;
                    break;
                case PowerUp.BlinkRange:
                    playercontroller.blinkDistance += blinkRGToGive;
                    break;
                case PowerUp.BlinkCoolTime:
                    playercontroller.blinkCooldown -= blinkClToGive;
                    break;
                default:
                    break;
            }
            Destroy(gameObject);
        }
    }
}
