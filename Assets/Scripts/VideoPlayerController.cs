using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Video;

[RequireComponent(typeof(VideoPlayer))]
public class VideoPlayerController : MonoBehaviour
{
    const string remotePath = "https://ironcutter24.github.io/GMtk-Jam2024/";

    private VideoPlayer videoPlayer;

    [SerializeField] string remoteFileName;
    [Space]
    public UnityEvent OnVideoStarted;
    public UnityEvent OnVideoEnded;

    private void Awake()
    {
        videoPlayer = gameObject.GetComponent<VideoPlayer>();
    }

    private void Start()
    {
        videoPlayer.started += OnStarted;
        videoPlayer.loopPointReached += OnLoopPointReached;
        videoPlayer.errorReceived += OnErrorReceived;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = remotePath + remoteFileName;
        videoPlayer.Play();

        InputManager.Actions.UI.Skip.performed += UISkip_performed;

        InputManager.SwitchActionMapToUI();
    }

    private void OnDestroy()
    {
        videoPlayer.started -= OnStarted;
        videoPlayer.loopPointReached -= OnLoopPointReached;
        videoPlayer.errorReceived -= OnErrorReceived;

        InputManager.Actions.UI.Skip.performed -= UISkip_performed;
    }

    private void UISkip_performed(InputAction.CallbackContext context)
    {
        OnVideoEnded?.Invoke();
    }

    private void OnStarted(VideoPlayer vp)
    {
        OnVideoStarted?.Invoke();
    }

    private void OnLoopPointReached(VideoPlayer vp)
    {
        OnVideoEnded?.Invoke();
    }

    private void OnErrorReceived(VideoPlayer vp, string error)
    {
        OnVideoEnded?.Invoke();
    }
}
