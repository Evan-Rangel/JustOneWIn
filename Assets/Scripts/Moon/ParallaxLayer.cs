using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public Transform cam;
    public float parallaxFactor = 0.5f; // 0 = fijo, 1 = sigue cámara
    public int pixelsPerUnit = 16;

    private float startX;
    private float startCamX;

    void Start()
    {
        startX = transform.position.x;
        startCamX = cam.position.x;
    }

    void LateUpdate()
    {
        float deltaX = cam.position.x - startCamX;

        float targetX = startX + deltaX * parallaxFactor;

        // SNAP A PIXEL GRID (CLAVE)
        float unitsPerPixel = 1f / pixelsPerUnit;
        targetX = Mathf.Round(targetX / unitsPerPixel) * unitsPerPixel;

        transform.position = new Vector3(
            targetX,
            transform.position.y,
            transform.position.z
        );
    }
}