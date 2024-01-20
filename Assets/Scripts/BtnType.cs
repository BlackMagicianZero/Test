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
    public CanvasGroup HelpGroup;
    public CanvasGroup AssetGroup;
    bool isSound;
    private Damageable playerDamageable;
    private PlayerController playerController;

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
            MoveToRespawnZone();
            break;

            case BTNType.OverNo:
            GameObject gameManager = GameObject.FindGameObjectWithTag("GameManager");
            if (gameManager != null)
            {
                Destroy(gameManager);
            }
            SceneManager.LoadScene(nextSceneName);
            break;

            case BTNType.Help:
            CanvasGroupOn(HelpGroup);
            CanvasGroupOff(ESCGroup);
            break;

            case BTNType.asset:
            CanvasGroupOn(AssetGroup);
            CanvasGroupOff(ESCGroup);
            break;

            case BTNType.BossYes:
            playerDamageable.Health = playerDamageable.MaxHealth;
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.localPosition = new Vector3(180f, -3.35f, 0f);
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
        buttonScale.localScale = defaultScale * 1.0f;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        buttonScale.localScale = defaultScale;
    }
    public void MoveToRespawnZone()
    {
        // Respawn 영역을 찾기
        GameObject[] respawnAreas = GameObject.FindGameObjectsWithTag("RespawnArea");

         List<GameObject> activeRespawnAreas = new List<GameObject>();
    foreach (var respawnArea in respawnAreas)
    {
        if (respawnArea.activeSelf)
        {
            activeRespawnAreas.Add(respawnArea);
        }
    }

    if (activeRespawnAreas.Count > 0)
    {
        // 리스트에서 무작위로 Respawn 영역 선택
        int randomIndex = Random.Range(0, activeRespawnAreas.Count);
        Vector3 respawnPosition = activeRespawnAreas[randomIndex].transform.position;

        // 플레이어를 respawn 위치로 이동
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = respawnPosition;
    }
        else
        {
            // Respawn 영역을 찾지 못한 경우에 대한 예외 처리
            Debug.LogWarning("Respawn 영역을 찾을 수 없습니다.");
        }
    }
}
