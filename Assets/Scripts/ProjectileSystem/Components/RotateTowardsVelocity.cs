using UnityEngine;

/*---------------------------------------------------------------------------------------------
RotateTowardsVelocity es un componente que hace que el proyectil se oriente automáticamente en 
la dirección en la que se mueve. Calcula el ángulo del vector de velocidad y ajusta la rotación 
del transform en consecuencia, de modo que el eje transform.right del objeto apunte siempre hacia 
adelante en su movimiento. Esto mejora la coherencia visual del proyectil en movimiento, 
especialmente en juegos con proyectiles curvos o dinámicos.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class RotateTowardsVelocity : ProjectileComponent
    {
        // Esta función se llama en cada FixedUpdate 
        protected override void FixedUpdate()
        {
            // Llama también al FixedUpdate base (por si hay lógica heredada que debe ejecutarse)
            base.FixedUpdate();

            // Obtiene la velocidad actual del Rigidbody2D
            var velocity = rb.velocity;

            // Si no hay movimiento, no se hace nada
            if (velocity.Equals(Vector3.zero))
                return;

            // Calcula el ángulo del vector de velocidad en grados (de radianes a grados)
            var angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;

            // Aplica la rotación alrededor del eje Z (Vector3.forward en 2D)
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
