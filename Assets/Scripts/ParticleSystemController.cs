using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    private bool isPlaying = false;
    public GameObject bossObject; // 보스 오브젝트 추가
    public GameObject bossAniObject;
    public GameObject bossCamera;
    public GameObject bossHpbar;
    private Vector3 originalBossCameraPosition; // BossCamera의 원래 위치를 저장할 변수
    private float originalCameraSize; // BossCamera의 원래 Projection Size를 저장할 변수

    public Vector3 targetPosition = new Vector3(18f, -20f, 0f);
    public float moveDuration = 5f;
    public float cameraSizeChangeDuration = 3f;

    void Start()
    {
        // BossCamera의 초기 위치와 크기를 저장
        originalBossCameraPosition = bossCamera.transform.position;
        originalCameraSize = bossCamera.GetComponent<Camera>().orthographicSize;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체가 Player 태그를 가지고 있다면
        if (collision.CompareTag("Player") && !isPlaying)
        {
            // BossCamera 활성화
            if (bossCamera != null)
            {
                bossCamera.SetActive(true);
                StartCoroutine(MoveBossCamera(targetPosition, moveDuration));
            }

            StartCoroutine(StartParticleAfterMove(moveDuration + 0.3f)); // 카메라 이동 시간 + 0.3초 후에 파티클을 시작
        }
    }

    // 파티클의 모양을 변경하는 코루틴
    private IEnumerator ChangeShapeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // 파티클이 끝난 후에 BossCamera의 Projection Size를 변경
        StartCoroutine(ChangeCameraSize(bossCamera.GetComponent<Camera>().orthographicSize, 6.5f, 11.5f, cameraSizeChangeDuration));
    }

    // BossCamera를 부드럽게 이동시키는 코루틴
    private IEnumerator MoveBossCamera(Vector3 targetPos, float duration)
{
    float elapsed = 0f;
    Vector3 initialPos = bossCamera.transform.position;

    while (elapsed < duration)
    {
        Vector3 newPosition = Vector3.Lerp(initialPos, targetPos, elapsed / duration);
        newPosition.z = initialPos.z; // z 축 값을 초기 위치의 z 값으로 고정
        bossCamera.transform.position = newPosition;

        elapsed += Time.deltaTime;
        yield return null;
    }

    // 도착지점에 도달하면 최종 위치를 설정
    bossCamera.transform.position = new Vector3(bossCamera.transform.position.x, bossCamera.transform.position.y, -19.15f);
}

    // 카메라 이동이 완료된 후 파티클을 시작하는 코루틴
    private IEnumerator StartParticleAfterMove(float delay)
    {
        yield return new WaitForSeconds(delay + 1.5f); // delay 시간 동안 대기
        StartCoroutine(ChangeShapeAfterDelay(2f)); // 2초 후에 모양 변경 코루틴 시작
    }

    // BossCamera의 크기를 변경하는 코루틴
    private IEnumerator ChangeCameraSize(float startSize, float targetSize, float originalSize, float duration)
    {
        // 크기를 변경합니다.
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float newSize = Mathf.Lerp(startSize, targetSize, elapsed / duration);
            bossCamera.GetComponent<Camera>().orthographicSize = newSize;
            elapsed += Time.deltaTime;
            yield return null;
        }
        bossCamera.GetComponent<Camera>().orthographicSize = targetSize; // 크기 변화가 완료된 후 보정
        bossAniObject.SetActive(true);

        // 일정 시간 대기
        yield return new WaitForSeconds(3.5f); // 3.5초 대기
        
        // Opening 애니메이션 재생
        Animator bossAnimator = bossAniObject.GetComponent<Animator>();
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("OpeningTrigger");
        }
        yield return new WaitForSeconds(2f); // 3.5초 대기

        // 크기를 원래 크기로 복구
        elapsed = 0f;
        while (elapsed < duration)
        {
            float newSize = Mathf.Lerp(targetSize, originalSize, elapsed / duration);
            bossCamera.GetComponent<Camera>().orthographicSize = newSize;
            elapsed += Time.deltaTime;
            yield return null;
        }
        bossCamera.GetComponent<Camera>().orthographicSize = originalSize; // 크기가 원래 값으로 복구된 후 보정

        // 크기가 원래 값으로 복구된 후, BossCamera의 위치를 원래 위치로
        elapsed = 0f;
        Vector3 currentPosition = bossCamera.transform.position;
        while (elapsed < duration)
        {
            bossCamera.transform.position = Vector3.Lerp(currentPosition, originalBossCameraPosition, elapsed / (duration * 0.3f));
            elapsed += Time.deltaTime;
            yield return null;
        }
        bossAniObject.SetActive(false);
        bossObject.SetActive(true);
        bossCamera.transform.position = originalBossCameraPosition; // 위치가 원래 값으로 복구된 후 보정
        bossHpbar.SetActive(true);
        //이 게임오브젝트를 비활성화
        gameObject.SetActive(false);
        // 모든 기능이 끝나면 카메라를 비활성화
        bossCamera.SetActive(false);
    }
}

