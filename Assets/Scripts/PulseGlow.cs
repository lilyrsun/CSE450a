using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseGlow : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float pulseScale = 0.05f;

    void Update()
    {
        float scale = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseScale;
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}