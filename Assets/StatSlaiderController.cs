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

    private void Start()
    {
        sliderSpeed.value = PlayerStats.Instance.MoveSpeed;
        sliderJumpSpeed.value = PlayerStats.Instance.JumpSpeed;
        sliderWeight.value = PlayerStats.Instance.Weight;
        sliderStrenght.value = PlayerStats.Instance.Strength;
    }

    public void SetStrenght()
    {
         PlayerStats.Instance.SetMoveSpeed(sliderStrenght.value);
    }

    public void SetWeight()
    {
        PlayerStats.Instance.SetWeight(sliderWeight.value);
    }

    public void SetSpeed()
    {
        PlayerStats.Instance.SetMoveSpeed(sliderSpeed.value);
    }

    public void SetJumpSpeed()
    {
        PlayerStats.Instance.SetJumpSpeed(sliderJumpSpeed.value);
    }
}
