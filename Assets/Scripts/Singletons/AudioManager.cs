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
    [SerializeField] FMODUnity.EventReference cratePushEvent;
    [SerializeField] FMODUnity.EventReference platePressEvent;
    [SerializeField] FMODUnity.EventReference plateReleaseEvent;

    [Header("Player Events")]
    [SerializeField] FMODUnity.EventReference playerStepEvent;
    [SerializeField] FMODUnity.EventReference playerJumpEvent;
    [SerializeField] FMODUnity.EventReference playerDashEvent;
    [SerializeField] FMODUnity.EventReference playerDeathDrownEvent;
    [SerializeField] FMODUnity.EventReference playerDeathBladeEvent;
    [SerializeField] FMODUnity.EventReference playerDeathKnightEvent;


    public void PlayUIClick() => PlayOneShot(uiClickEvent);
    public void PlayUICancel() => PlayOneShot(uiCancelEvent);
    public void PlayUIHoverEnter() => PlayOneShot(uiHoverEnterEvent);
    public void PlayUIHoverExit() => PlayOneShot(uiHoverExitEvent);
    public void PlayUIGameStarted() => PlayOneShot(uiGameStartEvent);

    public void PlayGameOver() => PlayOneShot(gameOverEvent);
    public void PlayCratePush() => PlayOneShot(cratePushEvent);
    public void PlayPlatePress() => PlayOneShot(platePressEvent);
    public void PlayPlateRelease() => PlayOneShot(plateReleaseEvent);

    public void PlayPlayerStep() => PlayOneShot(playerStepEvent);
    public void PlayPlayerJump() => PlayOneShot(playerJumpEvent);
    public void PlayPlayerDash() => PlayOneShot(playerDashEvent);
    public void PlayPlayerDeathDrown() => PlayOneShot(playerDeathDrownEvent);
    public void PlayPlayerDeathBlade() => PlayOneShot(playerDeathBladeEvent);
    public void PlayPlayerDeathKnight() => PlayOneShot(playerDeathKnightEvent);


    private void PlayOneShot(FMODUnity.EventReference soundEvent)
    {
        var playerState = FMODUnity.RuntimeManager.CreateInstance(soundEvent);
        playerState.start();
    }
}
