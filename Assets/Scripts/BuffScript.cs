using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public GameObject BfIcon;

    private void Awake()
    {
        playercontroller = FindObjectOfType<PlayerController>();
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            BuffSlot bfslot = collision.GetComponent<BuffSlot>();
            for(int i = 0; i < bfslot.slots.Count; i++)
            {
                if(bfslot.slots[i].isEmpty)
                {
                    Instantiate(BfIcon,bfslot.slots[i].slotObj.transform);
                    bfslot.slots[i].isEmpty = false;
                    Destroy(this.gameObject);
                    break;
                }
            }
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
        }
    }
}
// 아이콘 기초 로직
// GameCanvas - UI - Image 해서 이미지 입히고 프리팹화 시킨뒤에 
// Buffscript 적용 하고 bficon에 프리팹 넣으면 끝.