using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemController : MonoBehaviour
{
    public ParticleSystem particleSystems;
    private bool isPlaying = false;

    public GameObject bossCamera;
    private Vector3 originalBossCameraPosition; // BossCamera의 원래 위치를 저장할 변수

    public Vector3 targetPosition = new Vector3(48.26f, -40.4f, -1f);
    public float moveDuration = 5f;

    void Start()
    {
        // BossCamera의 초기 위치를 저장합니다.
        originalBossCameraPosition = bossCamera.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 충돌한 객체가 Player 태그를 가지고 있다면
        if (collision.CompareTag("Player") && !isPlaying)
        {
            // BossCamera를 활성화합니다.
            if (bossCamera != null)
            {
                bossCamera.SetActive(true);
                // 5초 동안 부드럽게 이동합니다.
                StartCoroutine(MoveBossCamera(targetPosition, moveDuration));
                // 5초 후에 BossCamera를 비활성화합니다.
                StartCoroutine(DisableBossCameraAfterDelay(moveDuration));
            }

            // 파티클 시스템을 작동시킵니다.
            var main = particleSystems.main;
            main.startLifetime = 2f;
            main.startSpeed = 1f;

            var shape = particleSystems.shape;
            shape.shapeType = ParticleSystemShapeType.Box;

            particleSystems.Play();
            isPlaying = true;

            StartCoroutine(ChangeShapeAfterDelay(2f));
        }
    }

    private IEnumerator ChangeShapeAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        var shape = particleSystems.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;

        var main = particleSystems.main;
        main.startSpeed = 2f;

        yield return new WaitForSeconds(2f);

        particleSystems.Stop();
        isPlaying = false;
    }

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

    private IEnumerator DisableBossCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // BossCamera를 원래 위치로 되돌립니다.
        bossCamera.transform.position = originalBossCameraPosition;

        // BossCamera를 비활성화합니다.
        if (bossCamera != null)
        {
            bossCamera.SetActive(false);
        }
    }
}