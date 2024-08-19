using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Util;

public class AudioManager : Singleton<AudioManager>
{
    [Header("UI Events")]
    [SerializeField] FMODUnity.EventReference uiClickEvent;
    [SerializeField] FMODUnity.EventReference uiCancelEvent;
    [SerializeField] FMODUnity.EventReference uiHoverEnterEvent;
    [SerializeField] FMODUnity.EventReference uiHoverExitEvent;
    [SerializeField] FMODUnity.EventReference uiGameStartEvent;

    private void Start()
    {
        
    }

    public void PlayUIClick() => PlayOneShot(uiClickEvent);
    public void PlayUICancel() => PlayOneShot(uiCancelEvent);
    public void PlayUIHoverEnter() => PlayOneShot(uiHoverEnterEvent);
    public void PlayUIHoverExit() => PlayOneShot(uiHoverExitEvent);
    public void PlayUIGameStarted() => PlayOneShot(uiGameStartEvent);

    private void PlayOneShot(FMODUnity.EventReference soundEvent)
    {
        var playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvent);
        playerState.start();
    }
}
