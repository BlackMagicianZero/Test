using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public ParticleSystem particleSystems;
    private bool isPlaying = false;
    public GameObject bossObject; // 보스 오브젝트 추가
    public GameObject bossCamera;
    private Vector3 originalBossCameraPosition; // BossCamera의 원래 위치를 저장할 변수
    private float originalCameraSize; // BossCamera의 원래 Projection Size를 저장할 변수
    public GameObject bossroomWall;

    public Vector3 targetPosition = new Vector3(48.26f, -40.4f, -1f);
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
            bossroomWall.SetActive(false);
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

        var shape = particleSystems.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;

        var main = particleSystems.main;
        main.startSpeed = 2f;

        yield return new WaitForSeconds(2f); // 2초 동안 Cone shape로 작동

        particleSystems.Stop(); // 파티클 시스템 멈추기
        isPlaying = false; // 재생 중이 아님을 표시

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
            bossCamera.transform.position = Vector3.Lerp(initialPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        bossCamera.transform.position = targetPos; // 이동이 완료된 후 위치 보정
    }

    // 카메라 이동이 완료된 후 파티클을 시작하는 코루틴
    private IEnumerator StartParticleAfterMove(float delay)
    {
        yield return new WaitForSeconds(delay + 1.5f); // delay 시간 동안 대기

        // 파티클 시스템 설정
        var main = particleSystems.main;
        main.startLifetime = 2f;
        main.startSpeed = 1f;

        var shape = particleSystems.shape;
        shape.shapeType = ParticleSystemShapeType.Box;

        particleSystems.Play(); // 파티클 시스템 재생
        isPlaying = true; // 재생 중임을 표시

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
        bossObject.SetActive(true);

        // 일정 시간 대기
        yield return new WaitForSeconds(3.5f); // 3.5초 대기

        // Opening 애니메이션 재생
        Animator bossAnimator = bossObject.GetComponent<Animator>();
        if (bossAnimator != null)
        {
            bossAnimator.SetTrigger("OpeningTrigger");
        }

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
        bossCamera.transform.position = originalBossCameraPosition; // 위치가 원래 값으로 복구된 후 보정
        //이 게임오브젝트를 비활성화
        gameObject.SetActive(false);
        // 모든 기능이 끝나면 카메라를 비활성화
        bossCamera.SetActive(false);
    }
}

