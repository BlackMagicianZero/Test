using System.Collections;
using System.Collections.Generic;
using Spine;
using UnityEngine;
using UnityEngine.UI;

public class BuffScript : MonoBehaviour
{
    // 버프 목록 enum 화
    public enum PowerUp { DBJump, DashRangeUp, ATKDamageUp, DashATKDamageUp, HPUp }
    public PowerUp powerups;

    // 버프 시스템 수치 조정하는 곳 변수만들어서 switch문에 꽂아넣을 것.
    public float JumpToGive;
    public float dashRGToGive;
    public int AtkDamageToGive;
    public int DashATKDamageToGive;
    public int HPToGive;
    //-------------------------------------------

    private GameObject player;
    private PlayerController playercontroller;
    private Damageable damageable;
    public GameObject BfIcon;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playercontroller = FindObjectOfType<PlayerController>();
        damageable = player.GetComponent<Damageable>();
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
                case PowerUp.HPUp:
                    damageable.MaxHealth = damageable.MaxHealth + HPToGive;
                    break;
                case PowerUp.ATKDamageUp:
                    GameObject swordAttack1 = player.transform.Find("SwordAttack1").gameObject;
                    if (swordAttack1 != null)
                    {
                        // SwordAttack1 오브젝트에서 Attack 스크립트를 찾기
                        Attack attack1 = swordAttack1.GetComponent<Attack>();
                        if (attack1 != null)
                        {
                            // Attack 스크립트의 AttackDamage 값을 증가
                            attack1.attackDamage += AtkDamageToGive;
                        }
                    }
                    GameObject swordAttack2 = player.transform.Find("SwordAttack2").gameObject;
                    if (swordAttack2 != null)
                    {
                        // SwordAttack1 오브젝트에서 Attack 스크립트를 찾기
                        Attack attack2 = swordAttack2.GetComponent<Attack>();
                        if (attack2 != null)
                        {
                            // Attack 스크립트의 AttackDamage 값을 증가
                            attack2.attackDamage += AtkDamageToGive;
                        }
                    }
                    GameObject swordAttack3 = player.transform.Find("SwordAttack3").gameObject;
                    if (swordAttack3 != null)
                    {
                        // SwordAttack1 오브젝트에서 Attack 스크립트를 찾기
                        Attack attack3 = swordAttack3.GetComponent<Attack>();
                        if (attack3 != null)
                        {
                            // Attack 스크립트의 AttackDamage 값을 증가
                            attack3.attackDamage += AtkDamageToGive;
                        }
                    }
                    break;
                case PowerUp.DashRangeUp:
                    playercontroller.dashingPower = playercontroller.dashingPower * dashRGToGive;
                    break;
                case PowerUp.DashATKDamageUp:
                    GameObject dashAttack = player.transform.Find("DashAttack").gameObject;
                    if (dashAttack != null)
                    {
                        // SwordAttack1 오브젝트에서 Attack 스크립트를 찾기
                        Attack dashattack = dashAttack.GetComponent<Attack>();
                        if (dashattack != null)
                        {
                            // Attack 스크립트의 AttackDamage 값을 증가
                            dashattack.attackDamage += DashATKDamageToGive;
                        }
                    }
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