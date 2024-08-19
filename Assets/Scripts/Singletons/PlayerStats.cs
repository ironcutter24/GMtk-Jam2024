using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerStats : Singleton<PlayerStats>
{
    const int rangeMin = 0;
    const int rangeMax = 2;

    [field: SerializeField, Range(rangeMin, rangeMax)] public int MoveSpeed { get; private set; } = 0;
    [field: SerializeField, Range(rangeMin, rangeMax)] public int JumpSpeed { get; private set; } = 0;
    [field: SerializeField, Range(rangeMin, rangeMax)] public int Strength { get; private set; } = 0;
    [field: SerializeField, Range(rangeMin, rangeMax)] public int Weight { get; private set; } = 0;


    public static Action AnyChanged;
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
        SetMoveSpeed(0f);
        SetJumpSpeed(0f);
        SetStrength(0f);
        SetWeight(0f);
    }

    public void SetMoveSpeed(float value)
    {
        MoveSpeed = CastAndClamp(value);
        MoveSpeedChanged?.Invoke();
        AnyChanged?.Invoke();
    }

    public void SetJumpSpeed(float value)
    {
        JumpSpeed = CastAndClamp(value);
        JumpSpeedChanged?.Invoke();
        AnyChanged?.Invoke();
    }

    public void SetStrength(float value)
    {
        Strength = CastAndClamp(value);
        StrengthChanged?.Invoke();
        AnyChanged?.Invoke();
    }

    public void SetWeight(float value)
    {
        Weight = CastAndClamp(value);
        WeightChanged?.Invoke();
        AnyChanged?.Invoke();
    }

    private int CastAndClamp(float value)
    {
        return Mathf.Clamp((int)value, rangeMin, rangeMax);
    }
}
