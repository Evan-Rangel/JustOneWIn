using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidSurfaceTest : MonoBehaviour
{
    public AcidSurface surface;

    [Header("Keyboard")]
    public float centerSplashVelocity = 2.5f;
    public float randomSplashVelocity = 2.0f;

    [Header("Mouse")]
    public float mouseSplashVelocity = 2.5f;

    void Reset()
    {
        surface = GetComponent<AcidSurface>();
    }

    void Update()
    {
        if (surface == null) return;

        // Space: splash in the center
        if (Input.GetKeyDown(KeyCode.Space))
        {
            surface.Splash(0.5f, centerSplashVelocity);
        }

        // R: splash at random x
        if (Input.GetKeyDown(KeyCode.R))
        {
            float x01 = Random.value;
            surface.Splash(x01, randomSplashVelocity);
        }

        // Left click: splash where you click (world X)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouse = Input.mousePosition;
            Vector3 world = Camera.main.ScreenToWorldPoint(mouse);
            surface.SplashWorldX(world.x, mouseSplashVelocity);
        }
    }
}