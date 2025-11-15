using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Avocado
{
    public class FabricTorretCanon : MonoBehaviour
    {
        Transform target;
        bool aiming=false;
        private void Update()
        {
            if (target != null&&aiming)
            {
                AimingAtPlayer();
            }
        }
        public void Aiming(Transform _target)
        {
            target = _target;
            aiming = true;
        }
        void AimingAtPlayer()
        {
            Vector2 direction = (target.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 200 * Time.deltaTime);
        }
    }
}
