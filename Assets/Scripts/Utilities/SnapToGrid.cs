using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

[ExecuteInEditMode]
public class SnapToGrid : MonoBehaviour
{
    [SerializeField, Range(0.1f, 5f)] float gridSize = 1f;

#if UNITY_EDITOR
    private void Update()
    {
        transform.position = transform.position.SnapToGrid(gridSize);
    }
#endif
}
