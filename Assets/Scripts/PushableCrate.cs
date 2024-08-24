using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableCrate : GeneralObject
{
    private int editorLayer;
    private Rigidbody2D rb;

    [SerializeField, Range(0, 2)] int minPushStrength = 2;

    private bool IsPushable => PlayerStats.Instance.Strength >= minPushStrength;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected override void Start()
    {
        base.Start();

        editorLayer = gameObject.layer;
        ResetState();

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
            SetLayer(LayerMask.LayerToName(editorLayer));
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
