using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class logolight : MonoBehaviour
{
    public Image image; // 알파값을 조정할 Image 컴포넌트
    public float duration = 1.0f; // 애니메이션이 완료되는 시간 (초 단위)

    private float elapsedTime = 0f;
    private bool isFadingOut = true; // 알파값이 줄어드는지 늘어나는지 여부
    public string sceneName;

    private void Start()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
        }
    }

    private void Update()
    {
        if (image == null)
            return;

        elapsedTime += Time.deltaTime;

        if (isFadingOut)
        {
            // 알파값을 255에서 120으로 감소
            float alphaValue = Mathf.Lerp(255, 70, elapsedTime / duration) / 255f;
            SetAlpha(alphaValue);

            if (elapsedTime >= duration)
            {
                elapsedTime = 0f;
                isFadingOut = false; // 방향 전환
            }
        }
        else
        {
            // 알파값을 120에서 255로 증가
            float alphaValue = Mathf.Lerp(70, 255, elapsedTime / duration) / 255f;
            SetAlpha(alphaValue);

            if (elapsedTime >= duration)
            {
                elapsedTime = 0f;
                isFadingOut = true; // 방향 전환
            }
        }
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void SetAlpha(float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }
}
