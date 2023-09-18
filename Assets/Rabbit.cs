using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class Rabbit : MonoBehaviour
{
    public SkeletonAnimation skeletonAnimation;
    public AnimationReferenceAsset[] AnimClip;

    //애니메이션에 대한 enum
    public enum AnimState
    {

    }

    //현재 애니메이션 처리가 무엇인지에 대한 변수
    private AnimState _AnimState;

    //현재 어떤 애니메이션이 재생되는지에 대한 변수
    private string CurrentAnimation;

    private Rigidbody2D rb;
    private float xx;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        
    }

    private void _AsncAnimation(AnimationReferenceAsset animClip, bool loop, float timeScale)
    {
        // 해당 애니메이션으로 변경하는 함수
        skeletonAnimation.state.SetAnimation(0,animClip,loop).TimeScale = timeScale;
    }

    private void SetCurrentAnimation(AnimState _state)
    {
        switch (_state)
        {
            /*case AnimState.ATTACK:
                _AsyncAnimation(AnimClip[(int)AnimState.ATTACK],true,1f);
                break;*/
        }
    }
}
