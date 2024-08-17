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


    public void SetMoveSpeed(float value) => MoveSpeed = value;
    public void SetJumpSpeed(float value) => JumpSpeed = value;
    public void SetStrength(float value) => Strength = value;
    public void SetWeight(float value) => Weight = value;

    protected override void Awake()
    {
        base.Awake();

        if (Instance == this)
        {
            ResetStats();
        }
    }

    public void ResetStats()
    {
        MoveSpeed = 6f;
        JumpSpeed = 9f;
        Strength = 1f;
        Weight = 1f;
    }
}
