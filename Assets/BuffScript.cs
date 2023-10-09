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

            // Bficon1 GameObject를 찾아서 Image 컴포넌트의 sprite 이미지를 설정
            GameObject bficon1 = GameObject.Find("Bficon_1"); // Bficon1의 GameObject 이름을 정확하게 사용해야 합니다.
            if (bficon1 != null)
            {
                Image bficonImage = bficon1.GetComponent<Image>();
                if (bficonImage != null)
                {
                    bficonImage.sprite = GetComponent<SpriteRenderer>().sprite;
                }
                else
                {
                    Debug.LogWarning("Bficon1 GameObject에 Image 컴포넌트가 없습니다.");
                }
            }
            else
            {
                Debug.LogWarning("Bficon1 GameObject를 찾을 수 없습니다.");
            }
        }
    }
}
