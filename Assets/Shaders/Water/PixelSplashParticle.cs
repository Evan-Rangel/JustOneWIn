using UnityEngine;

public class PixelSplashParticle : MonoBehaviour
{
    [Header("Motion")]
    public Vector2 Velocity;
    public float Gravity = 35f;

    [Header("Life")]
    public float Lifetime = 0.25f;

    private float _t;

    private void Update()
    {
        float dt = Time.deltaTime;

        _t += dt;
        if (_t >= Lifetime)
        {
            Destroy(gameObject);
            return;
        }

        Velocity += Vector2.down * Gravity * dt;
        transform.position += (Vector3)(Velocity * dt);
    }
}