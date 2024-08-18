using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyCrate : MonoBehaviour
{
    const float MIN_PUSH_STRENGTH = 2.5f;

    private Rigidbody2D rb;

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
        rb.constraints = PlayerStats.Instance.Strength < MIN_PUSH_STRENGTH ? RigidbodyConstraints2D.FreezeAll : RigidbodyConstraints2D.FreezeRotation;
    }
}
