using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Door : MonoBehaviour
{
    // 다음으로 이동할 씬의 이름
    public Image Panel;
    float time = 0f;
    float F_time = 1f;
    private PlayerController thePlayer;

    private bool hasPlayerEntered = false;
    private Animator anim;
    public float distance = 3.0f;
    public GameObject toObj;

    void Start()
    {
        thePlayer = FindObjectOfType<PlayerController>();
        anim = GetComponent<Animator>();
    }   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasPlayerEntered && collision.CompareTag("Player"))
        {
            hasPlayerEntered = true;
            StartCoroutine(FadeFlow());
        }
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, thePlayer.transform.position) < distance)
        {
            anim.SetBool("Open", true);
        }
        else
        {
            anim.SetBool("Open", false);
        }
    }

    IEnumerator FadeFlow()
    {
        Panel.gameObject.SetActive(true);
        time = 0f;
        Color alpha = Panel.color;
        while (alpha.a < 1f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(0, 1, time);
            Panel.color = alpha;
            yield return null;
        }
        // 플레이어 이동
        thePlayer.transform.position = toObj.transform.position;

        // 여기서 이동이 완료될 때까지 대기
        //yield return new WaitUntil(() => Vector3.Distance(thePlayer.transform.position, toObj.transform.position) < 0.1f);

        time = 0f;
        yield return new WaitForSeconds(2f);

        while (alpha.a > 0f)
        {
            time += Time.deltaTime / F_time;
            alpha.a = Mathf.Lerp(1, 0, time);
            Panel.color = alpha;
            yield return null;
        }
        Panel.gameObject.SetActive(false);
        yield return null;
    }
}