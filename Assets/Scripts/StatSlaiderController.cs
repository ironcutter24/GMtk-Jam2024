using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSlaiderController : MonoBehaviour
{
    [SerializeField] Slider sliderStrenght;
    [SerializeField] Slider sliderWeight;
    [SerializeField] Slider sliderSpeed;
    [SerializeField] Slider sliderJumpSpeed;

    public bool movingStrenght = false;
    public bool movingWeight = false;
    public bool movingSpeed = false;
    public bool movingJumpSpeed = false;

    private void Start()
    {
        sliderSpeed.onValueChanged.AddListener(SetSpeed);
        sliderJumpSpeed.onValueChanged.AddListener(SetJumpSpeed);
        sliderWeight.onValueChanged.AddListener(SetWeight);
        sliderStrenght.onValueChanged.AddListener(SetStrenght);

        sliderSpeed.value = PlayerStats.Instance.MoveSpeed;
        sliderJumpSpeed.value = PlayerStats.Instance.JumpSpeed;
        sliderWeight.value = PlayerStats.Instance.Weight;
        sliderStrenght.value = PlayerStats.Instance.Strength;
    }

    private void SetStrenght(float value)
    {
        PlayerStats.Instance.SetStrength(value);
        GameManager.Instance.PlayerController.CheckAndSetSize(sliderStrenght);
    }

    private void SetWeight(float value)
    {
        PlayerStats.Instance.SetWeight(value);
        GameManager.Instance.PlayerController.CheckAndSetSize(sliderWeight);
    }

    private void SetSpeed(float value)
    {
        PlayerStats.Instance.SetMoveSpeed(value);
        GameManager.Instance.PlayerController.CheckAndSetSize(sliderSpeed);
    }

    private void SetJumpSpeed(float value)
    {
        PlayerStats.Instance.SetJumpSpeed(value);
        GameManager.Instance.PlayerController.CheckAndSetSize(sliderJumpSpeed);
    }
}
