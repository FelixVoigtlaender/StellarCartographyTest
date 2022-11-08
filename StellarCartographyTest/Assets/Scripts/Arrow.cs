
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Vector2 start;
    public Vector2 end;
    public SpriteRenderer sprite;

    private void Update()
    {
        HandlePositioning();
    }

    public void HandlePositioning()
    {
        Vector2 dif = end - start;

        transform.position = start + dif * 0.5f;

        Vector3 scale = transform.localScale;
        scale.x = dif.magnitude;
        transform.localScale = scale;

        transform.right = dif.normalized;
    }
}
