using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Transform launchPoint;             // 발사 지점
    public GameObject projectilePrefab;       // 발사할 프로젝타일 프리팹
    public float fireCooldown = 1.5f;         // 발사 쿨다운 시간 (초)
    private float lastFireTime;               // 마지막 발사 시간 기록

    void Start()
    {
        lastFireTime = -fireCooldown;         // 초기에는 즉시 발사할 수 있도록 최초 발사 시간을 설정
    }

    public void FireProjectile()
    {
        // 충분한 시간이 지났는지 확인하여 발사 여부 결정
        if (Time.time - lastFireTime >= fireCooldown)
        {
            // 프로젝타일 생성 및 초기화
            GameObject projectile = Instantiate(projectilePrefab, launchPoint.position, projectilePrefab.transform.rotation);
            Vector3 origScale = projectile.transform.localScale;

            // 발사 시 캐릭터가 향한 방향에 따라 프로젝타일의 방향과 이동을 조절
            projectile.transform.localScale = new Vector3(
                origScale.x * transform.localScale.x > 0 ? 1 : -1,
                origScale.y,
                origScale.z
            );

            lastFireTime = Time.time;   // 마지막 발사 시간 업데이트
        }
    }
}
