using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraFollowObject : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _PlayerTransform;
    [Header("Flip Rotation Stats")]
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;
    private PlayerController _player;
    private bool _isFacingRight;
    private void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if(playerObject != null)
        {
            _PlayerTransform = playerObject.transform;
            _player = _PlayerTransform.gameObject.GetComponent<PlayerController>();
            _isFacingRight = _player.IsFacingRight;
        }
    }
    void Start()
    {
        if(PlayerController.Instance != null)
        {
            _isFacingRight = PlayerController.Instance.IsFacingRight;
        }
        else
        {
            Debug.LogError("플레이어 어디감");
        }
    }

    private void Update()
    {
        if(PlayerController.Instance != null)
        {
            transform.position = _PlayerTransform.position;
        }
    }

    public void CallTurn()
    {
        _turnCoroutine = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while(elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;
            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime / _flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;
        if(_isFacingRight)
        {
            return 180f;
        }
        else
        {
            return 0;
        }
    }
}
