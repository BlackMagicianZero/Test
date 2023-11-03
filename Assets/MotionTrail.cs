using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionTrail : MonoBehaviour
{
    public GameObject trailPrefab; // 궤적 효과를 그릴 프리팹
    public float recordInterval = 0.1f; // 궤적을 기록할 간격 (초)
    
    private List<Vector2> positions; // 물체의 위치를 기록할 리스트
    private float recordTimer; // 기록 간격을 측정하기 위한 타이머 변수

    private void Start()
    {
        positions = new List<Vector2>();
        recordTimer = 0f;
    }

    private void Update()
    {
        // 물체의 현재 위치를 기록
        positions.Add(transform.position);

        // 일정 시간마다 물체의 위치를 기록하고 궤적을 그림
        recordTimer += Time.deltaTime;
        if (recordTimer >= recordInterval)
        {
            recordTimer = 0f;
            DrawMotionTrail();
        }
    }

    private void DrawMotionTrail()
    {
        // 궤적을 그리기 위해 프리팹을 생성하고 위치를 설정
        GameObject trail = Instantiate(trailPrefab, positions[0], Quaternion.identity);
        LineRenderer lineRenderer = trail.GetComponent<LineRenderer>();
        lineRenderer.positionCount = positions.Count;

        // 궤적을 그리기 위해 물체의 위치를 라인 렌더러에 설정
        for (int i = 0; i < positions.Count; i++)
        {
            lineRenderer.SetPosition(i, positions[i]);
        }

        // 궤적을 그린 후 기록한 위치 초기화
        positions.Clear();
    }
}
