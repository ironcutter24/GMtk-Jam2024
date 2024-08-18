using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerStats : Singleton<PlayerStats>
{
    [field: SerializeField, Range(0f, 10f)] public float MoveSpeed { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float JumpSpeed { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float Strength { get; private set; }
    [field: SerializeField, Range(0f, 10f)] public float Weight { get; private set; }


    public static Action MoveSpeedChanged;
    public static Action JumpSpeedChanged;
    public static Action StrengthChanged;
    public static Action WeightChanged;


    IEnumerator Start()
    {
        yield return null;
        ResetStats();
    }

    public void ResetStats()
    {
        SetMoveSpeed(6f);
        SetJumpSpeed(9f);
        SetStrength(1f);
        SetWeight(1f);
    }

    public void SetMoveSpeed(float value)
    {
        MoveSpeed = value;
        MoveSpeedChanged?.Invoke();
    }

    public void SetJumpSpeed(float value)
    {
        JumpSpeed = value;
        JumpSpeedChanged?.Invoke();
    }

    public void SetStrength(float value)
    {
        Strength = value;
        StrengthChanged?.Invoke();
    }

    public void SetWeight(float value)
    {
        Weight = value;
        WeightChanged?.Invoke();
    }
}
