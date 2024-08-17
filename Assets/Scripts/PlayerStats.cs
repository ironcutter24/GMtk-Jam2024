using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class PlayerStats : Singleton<PlayerStats>
{
    [field: SerializeField, Range(0f, 4f)] public float MoveSpeed { get; private set; } = 2f;
    [field: SerializeField, Range(0f, 4f)] public float JumpSpeed { get; private set; } = 4f;
    [field: SerializeField, Range(0f, 4f)] public float Strength { get; private set; } = 1f;
    [field: SerializeField, Range(0f, 4f)] public float Weight { get; private set; } = 1f;


    public void SetMoveSpeed(float value) => MoveSpeed = value;
    public void SetJumpSpeed(float value) => JumpSpeed = value;
    public void SetStrength(float value) => Strength = value;
    public void SetWeight(float value) => Weight = value;

    public void ResetStats()
    {
        MoveSpeed = 2f;
        JumpSpeed = 4f;
        Strength = 1f;
        Weight = 1f;
    }
}
