using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    //게임 오브첵트 시차의 시작 위치
    Vector2 startingPosition;

    //게임 오브젝트 시차의 시작 값
    float startingZ;

    //시차 오브젝트의 시작 위치에서 카메라가 이동한 거리
    Vector2 camMoveSinceStart => (Vector2) cam.transform.position - startingPosition;

    float zDistanceFromTarget => transform.position.z - followTarget.transform.position.z;

    //개체가 대상 앞에 있으면 clip plane 주변을 사용하고, 대상 뒤에 있으면 외곽의 clip plane을 사용.
    float clippingPlane => (cam.transform.position.z + (zDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane));

    //플레이어에서 객체가 멀어질수록 시차 효과 개체가 더 빠르게 이동. Z값을 대상에 가깝게 끌어와 느리게 이동시키기.
    float ParallaxFactor => Mathf.Abs(zDistanceFromTarget) / clippingPlane;

    void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    void Update()
    {
        //객체가 이동할 때 시차 객체를 같은 거리에 곱한 값으로 이동
        Vector2 newPosition = startingPosition + camMoveSinceStart * ParallaxFactor;

        //X,Y 위치는 목표 이동 속도와 시차 위치를 기반으로 변경, Z값은 일정하게 유지
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);        
    }
}
