using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerEvents : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    public UnityEvent OnVideoEnded;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.loopPointReached += OnLoopPointReached;
    }

    private void OnDestroy()
    {
        videoPlayer.loopPointReached -= OnLoopPointReached;
    }

    private void OnLoopPointReached(VideoPlayer vp)
    {
        OnVideoEnded?.Invoke();
    }
}
