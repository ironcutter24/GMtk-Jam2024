using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableCrate : GeneralObject
{
    private Rigidbody2D rb;

    [SerializeField, Range(0, 2)] int minPushStrength = 2;

    private bool IsPushable => PlayerStats.Instance.Strength >= minPushStrength;

    protected override void Start()
    {
        base.Start();

        rb = GetComponent<Rigidbody2D>();

        PlayerStats.StrengthChanged += OnStrengthChanged;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        PlayerStats.StrengthChanged -= OnStrengthChanged;
    }

    protected override void ResetState()
    {
        OnStrengthChanged();
    }

    private void OnStrengthChanged()
    {
        if (IsPushable)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            SetLayer("Actor");
        }
        else
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            SetLayer("Default");
        }
    }

    private void SetLayer(string layerName)
    {
        var layer = LayerMask.NameToLayer(layerName);

        gameObject.layer = layer;

        foreach(Transform t in transform)
        {
            t.gameObject.layer = layer;
        }
    }
}
