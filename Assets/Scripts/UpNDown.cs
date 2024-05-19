using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpNDown : MonoBehaviour
{
    public float floatSpeed = 1f;  // 위아래로 움직이는 속도
    public float floatAmplitude = 0.5f;  // 위아래로 움직이는 진폭

    private Vector3 initialPosition;  // 초기 위치를 저장할 변수

    private void Awake()
    {
        initialPosition = transform.position;  // 초기 위치 저장
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 위아래로 움직이게 하는 로직
        float newY = initialPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        transform.position = new Vector3(initialPosition.x, newY, initialPosition.z);
    }
}
