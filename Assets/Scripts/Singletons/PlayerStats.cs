using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerStats : Singleton<PlayerStats>
{
    [field: SerializeField, Range(0f, 2f)] public int MoveSpeed { get; private set; }
    [field: SerializeField, Range(0f, 2f)] public int JumpSpeed { get; private set; }
    [field: SerializeField, Range(0f, 2f)] public int Strength { get; private set; }
    [field: SerializeField, Range(0f, 2f)] public int Weight { get; private set; }


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
        MoveSpeed = (int)value;
        MoveSpeedChanged?.Invoke();
        AnyChanged?.Invoke();
    }

    public void SetJumpSpeed(float value)
    {
        JumpSpeed = (int)value;
        JumpSpeedChanged?.Invoke();
        AnyChanged?.Invoke();
    }

    public void SetStrength(float value)
    {
        Strength = (int)value;
        StrengthChanged?.Invoke();
        AnyChanged?.Invoke();
    }

    public void SetWeight(float value)
    {
        Weight = (int)value;
        WeightChanged?.Invoke();
        AnyChanged?.Invoke();
    }
}
