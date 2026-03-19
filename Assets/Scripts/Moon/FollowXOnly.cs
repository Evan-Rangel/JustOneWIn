using UnityEngine;

public class FollowXOnly : MonoBehaviour
{
    public Transform cam;
    public float offsetX = 2f;
    public float offsetY = 3f;

    public int pixelsPerUnit = 16;

    void LateUpdate()
    {
        if (cam == null) return;

        Vector3 targetPos = new Vector3(
            cam.position.x + offsetX,
            cam.position.y + offsetY,
            transform.position.z
        );

        //SNAP FINAL (igual que pixel perfect)
        float unitsPerPixel = 1f / pixelsPerUnit;

        targetPos.x = Mathf.Round(targetPos.x / unitsPerPixel) * unitsPerPixel;
        targetPos.y = Mathf.Round(targetPos.y / unitsPerPixel) * unitsPerPixel;

        transform.position = targetPos;
    }
}