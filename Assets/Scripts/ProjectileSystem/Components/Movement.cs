using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
El componente Movement controla el movimiento básico del proyectil. Puede aplicarse de dos formas:
-Una sola vez cuando el proyectil es lanzado, para proyectiles que simplemente se impulsan y 
luego siguen su trayectoria por inercia.
-De forma continua, útil para proyectiles que se comportan como cohetes o misiles que mantienen 
su velocidad activamente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class Movement : ProjectileComponent
    {
        // Si está activado, la velocidad se aplicará en cada FixedUpdate
        [field: SerializeField] public bool ApplyContinuously { get; private set; }

        // Velocidad del proyectil
        [field: SerializeField] public float Speed { get; private set; }

        // En Init se aplica la velocidad una vez si ApplyContinuously está desactivado
        protected override void Init()
        {
            base.Init();

            SetVelocity();
        }

        // Aplica la velocidad en la dirección hacia la derecha del transform
        private void SetVelocity() => rb.velocity = Speed * transform.right;

        // Si ApplyContinuously está activado, se aplica la velocidad constantemente
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!ApplyContinuously)
                return;

            SetVelocity();
        }
    }
}
