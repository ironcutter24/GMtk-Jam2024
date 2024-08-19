using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class AudioManager : Singleton<AudioManager>
{
    [Header("UI Events")]
    [SerializeField] FMODUnity.EventReference uiClickEvent;
    [SerializeField] FMODUnity.EventReference uiCancelEvent;
    [SerializeField] FMODUnity.EventReference uiHoverEnterEvent;
    [SerializeField] FMODUnity.EventReference uiHoverExitEvent;
    [SerializeField] FMODUnity.EventReference uiGameStartEvent;

    [Header("Game Events")]
    [SerializeField] FMODUnity.EventReference gameOverEvent;


    public void PlayUIClick() => PlayOneShot(uiClickEvent);
    public void PlayUICancel() => PlayOneShot(uiCancelEvent);
    public void PlayUIHoverEnter() => PlayOneShot(uiHoverEnterEvent);
    public void PlayUIHoverExit() => PlayOneShot(uiHoverExitEvent);
    public void PlayUIGameStarted() => PlayOneShot(uiGameStartEvent);

    public void PlayGameOver() => PlayOneShot(gameOverEvent);


    private void PlayOneShot(FMODUnity.EventReference soundEvent)
    {
        var playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvent);
        playerState.start();
    }
}
