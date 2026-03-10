using System.Collections.Generic;
using UnityEngine;

namespace Avocado
{
    [RequireComponent(typeof(EdgeCollider2D))]
    [RequireComponent(typeof(InteractableWater))]
    public class WaterTriggerHandler : MonoBehaviour
    {
        [SerializeField] private LayerMask _waterMask;
        [SerializeField] private GameObject _splashParticles;

        [Header("Splash Tuning")]
        [SerializeField] private float _splashCooldown = 0.12f; // seconds, per Rigidbody2D
        [SerializeField] private float _maxEntrySpeed = 12f;    // clamp speed used for splash force

        private readonly Dictionary<Rigidbody2D, float> _nextAllowedTime = new Dictionary<Rigidbody2D, float>();

        private InteractableWater _water;

        private void Awake()
        {
            _water = GetComponent<InteractableWater>();
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            // layer filter
            if ((_waterMask.value & (1 << collision.gameObject.layer)) == 0) return;

            Rigidbody2D rb = collision.attachedRigidbody;
            if (rb == null) return;

            // only when moving down into water
            if (rb.velocity.y >= 0f) return;

            // per-rigidbody cooldown (prevents spam while intersecting trigger)
            float now = Time.time;
            if (_nextAllowedTime.TryGetValue(rb, out float nextTime) && now < nextTime) return;
            _nextAllowedTime[rb] = now + _splashCooldown;

            // optional particles
            if (_splashParticles != null)
                Instantiate(_splashParticles, collision.bounds.center, Quaternion.identity);

            float downSpeed = Mathf.Clamp(-rb.velocity.y, 0f, _maxEntrySpeed);
            float force = Mathf.Clamp(downSpeed * _water.ForceMultiplier, 0f, _water.MaxForce);

            // negative = push surface down
            _water.Splash(collision, -force);
        }

        // Optional cleanup: remove stale entries if rigidbodies get destroyed
        private void OnTriggerExit2D(Collider2D collision)
        {
            Rigidbody2D rb = collision.attachedRigidbody;
            if (rb != null) _nextAllowedTime.Remove(rb);
        }
    }
}