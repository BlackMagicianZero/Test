using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BtnType : MonoBehaviour,IPointerEnterHandler, IPointerExitHandler
{
    public BTNType currentType;
    public Transform buttonScale;
    Vector3 defaultScale;
    public string nextSceneName;
    public CanvasGroup ESCGroup;
    public CanvasGroup SoundGroup;
    public CanvasGroup MainGroup;
    public CanvasGroup RealQuitQuestionGroup;
    bool isSound;
    public Damageable playerDamageable;
    public PlayerController playerController;

    private void Start()
    {
        defaultScale = buttonScale.localScale;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        playerDamageable = player.GetComponent<Damageable>();
        playerController = GetComponent<PlayerController>();
    }
    public void OnBtnClick()
    {
        switch(currentType)
        {
            case BTNType.New:
            SceneManager.LoadScene(nextSceneName);
            break;
            case BTNType.Sound:
            CanvasGroupOn(SoundGroup);
            CanvasGroupOff(ESCGroup);
            break;
            case BTNType.Option:
            CanvasGroupOn(ESCGroup);
            CanvasGroupOff(SoundGroup);
            CanvasGroupOff(MainGroup);
            break;
            case BTNType.OptionBack:
            CanvasGroupOn(MainGroup);
            CanvasGroupOff(ESCGroup);
            break;
            case BTNType.Back:
            CanvasGroupOn(ESCGroup);
            CanvasGroupOff(SoundGroup);
            break;
            case BTNType.IngameBack:
            CanvasGroupOn(ESCGroup);
            CanvasGroupOff(RealQuitQuestionGroup);
            break;
            case BTNType.QuitReal:
            CanvasGroupOn(RealQuitQuestionGroup);
            CanvasGroupOff(ESCGroup);
            break;
            case BTNType.Quit:
            Application.Quit();
            break;
            case BTNType.OverYes:
            playerDamageable.Health = playerDamageable.MaxHealth;
            playerController.MoveToRespawnZone();
            break;
            case BTNType.OverNo:
            GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
            if (gameManager != null)
            {
                Destroy(gameManager);
            }
            SceneManager.LoadScene(nextSceneName);
            break;
        }
    }
    public void CanvasGroupOn(CanvasGroup cg)
    {
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }
    public void CanvasGroupOff(CanvasGroup cg)
    {
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale * 1.2f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
}
