using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStatsSheet", menuName = "ScriptableObjects/PlayerStatsSheet", order = 1)]
public class StatsSheet_SO : ScriptableObject
{
    [SerializeField, Min(0f)]
    Vector2 _defaultSize = new Vector2(.8f, 1.8f);

    [Header("Stats sheet")]
    [SerializeField]
    ParamSlider _speed = new ParamSlider
        (
            new ParamValue(5f, Vector2.zero),
            new ParamValue(10f, new Vector2(0f, 1f)),
            new ParamValue(15f, new Vector2(0f, 2f))
        );

    [SerializeField]
    ParamSlider _jump = new ParamSlider
    (
            new ParamValue(5f, Vector2.zero),
            new ParamValue(9f, Vector2.zero),
            new ParamValue(14f, Vector2.zero)
    );

    [SerializeField]
    ParamSlider _weight = new ParamSlider
    (
            new ParamValue(0f, Vector2.zero),
            new ParamValue(1f, Vector2.zero),
            new ParamValue(2f, new Vector2(.5f, 0f))
    );

    [SerializeField]
    ParamSlider _strength = new ParamSlider
    (
            new ParamValue(0f, Vector2.zero),
            new ParamValue(1f, Vector2.zero),
            new ParamValue(2f, new Vector2(.5f, 1f))
    );


    public Vector2 DefaultSize => _defaultSize;
    public ParamSlider Speed => _speed;
    public ParamSlider Jump => _jump;
    public ParamSlider Weight => _weight;
    public ParamSlider Strength => _strength;
}


[System.Serializable]
public struct ParamSlider
{
    [SerializeField] ParamValue _zero;
    [SerializeField] ParamValue _one;
    [SerializeField] ParamValue _two;

    public ParamSlider(ParamValue zero, ParamValue one, ParamValue two)
    {
        _zero = zero;
        _one = one;
        _two = two;
    }

    public ParamValue GetParamAt(int index)
    {
        return index switch
        {
            0 => _zero,
            1 => _one,
            2 => _two,
            _ => throw new System.Exception("ParamSlider: Invalid index")
        };
    }
}


[System.Serializable]
public struct ParamValue
{
    [SerializeField, Min(0f)]
    public float value;

    [SerializeField, Min(0f)]
    public Vector2 sizeDelta;

    public ParamValue(float value, Vector2 sizeDelta)
    {
        this.value = value;
        this.sizeDelta = sizeDelta;
    }
}
