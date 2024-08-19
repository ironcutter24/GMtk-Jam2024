using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableCrate : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField, Range(0, 10)] int minPushStrength = 3;

    private bool IsPushable => PlayerStats.Instance.Strength >= minPushStrength;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        PlayerStats.StrengthChanged += OnStrengthChanged;
    }

    private void OnDestroy()
    {
        PlayerStats.StrengthChanged -= OnStrengthChanged;
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
