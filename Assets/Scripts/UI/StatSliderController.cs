using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatSliderController : MonoBehaviour
{
    [SerializeField] Slider sliderStrenght;
    [SerializeField] Slider sliderWeight;
    [SerializeField] Slider sliderSpeed;
    [SerializeField] Slider sliderJumpSpeed;

    private void Start()
    {
        sliderSpeed.value = PlayerStats.Instance.MoveSpeed;
        sliderJumpSpeed.value = PlayerStats.Instance.JumpSpeed;
        sliderWeight.value = PlayerStats.Instance.Weight;
        sliderStrenght.value = PlayerStats.Instance.Strength;

        sliderSpeed.onValueChanged.AddListener(SetSpeed);
        sliderJumpSpeed.onValueChanged.AddListener(SetJumpSpeed);
        sliderWeight.onValueChanged.AddListener(SetWeight);
        sliderStrenght.onValueChanged.AddListener(SetStrenght);
    }

    private void SetStrenght(float value)
    {
        PlayerStats.Instance.SetStrength(value);
    }

    private void SetWeight(float value)
    {
        PlayerStats.Instance.SetWeight(value);
    }

    private void SetSpeed(float value)
    {
        PlayerStats.Instance.SetMoveSpeed(value);
    }

    private void SetJumpSpeed(float value)
    {
        PlayerStats.Instance.SetJumpSpeed(value);
    }
}
