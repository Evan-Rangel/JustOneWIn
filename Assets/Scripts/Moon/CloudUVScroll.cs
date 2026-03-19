using UnityEngine;

public class CloudUVScroll : MonoBehaviour
{
    public float speed = 0.1f;

    private Material mat;
    private Vector2 offset;

    void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
    }

    void Update()
    {
        offset.x += speed * Time.deltaTime;
        mat.mainTextureOffset = offset;
    }
}