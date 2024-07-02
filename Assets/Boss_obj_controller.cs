using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss_obj_controller : MonoBehaviour
{
    public GameObject obj_1;  // 비활성화할 오브젝트
    public GameObject obj_2;  // 활성화할 오브젝트
    public Damageable damageable;  // Damageable 컴포넌트
    public float moveDuration = 1.0f;  // 이동 애니메이션 지속 시간

    private Vector3 obj_2InitialPosition;
    private bool isMovingToObj1 = false;
    private bool isResetting = false;
    public GameObject boss_1_sound;
    public GameObject boss_2_sound;

    private void Start()
    {
        if (obj_1 == null || obj_2 == null || damageable == null)
        {
            Debug.LogError("obj_1, obj_2 또는 damageable이 할당되지 않았습니다.");
        }
        else
        {
            obj_2InitialPosition = obj_2.transform.position;
            obj_2.SetActive(false);  // 시작할 때 obj_2 비활성화
        }
    }

    private void Update()
    {
        if (damageable.Health <= 300 && !isMovingToObj1)
        {
            boss_1_sound.SetActive(false);
            boss_2_sound.SetActive(true);
            // obj_1을 비활성화하고, obj_2를 obj_1의 위치로 이동시키고 활성화.
            if (obj_1.activeSelf)
            {
                StartCoroutine(MoveObj2ToObj1());
                obj_1.SetActive(false);
            }
        }
        else if (damageable.Health >= 600 && !isResetting)
        {
            // obj_1을 활성화하고, obj_2를 초기 위치로 즉시 이동.
            if (!obj_1.activeSelf)
            {
                ResetObj2Position();
                obj_1.SetActive(true);
            }
        }
    }

    private IEnumerator MoveObj2ToObj1()
    {
        isMovingToObj1 = true;
        Vector3 startPosition = obj_2.transform.position;
        Vector3 targetPosition = obj_1.transform.position;
        float elapsedTime = 0;

        obj_2.SetActive(true);

        while (elapsedTime < moveDuration)
        {
            obj_2.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj_2.transform.position = targetPosition;
        isMovingToObj1 = false;
    }

    private void ResetObj2Position()
    {
        isResetting = true;
        obj_2.transform.position = obj_2InitialPosition;
        obj_2.SetActive(false);
        isResetting = false;
    }
}
