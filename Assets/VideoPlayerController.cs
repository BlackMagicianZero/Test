using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        // VideoPlayer가 존재하고 재생할 비디오 클립이 할당되어 있을 때
        if (videoPlayer != null && videoPlayer.clip != null)
        {
            // 비디오를 자동으로 재생
            videoPlayer.Play();
        }
    }
}
