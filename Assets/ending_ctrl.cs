using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ending_ctrl : MonoBehaviour
{
    private Damageable ThunderRamGDamageable;
    public GameObject endimage;
    public Image Panel;
    float time = 0f;
    float F_time = 1f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject ThunderRamG = GameObject.FindGameObjectWithTag("BOSS");
        if(!ThunderRamG)
        {
            endimage.SetActive(true);
        }
        else
        {
            endimage.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(endimage.activeSelf == true)
        {
            StartCoroutine(Fadeout());
        }
    }
    IEnumerator Fadeout()
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
    }
}
