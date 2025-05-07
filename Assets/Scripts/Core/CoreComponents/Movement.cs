using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script controla el movimiento de un personaje basado en Rigidbody2D. Permite:
-Establecer velocidades en X, Y o en ambas.
-Aplicar Knockback o fuerzas dirigidas.
-Flip (girar) al personaje si cambia de dirección de movimiento.
-Obtener puntos de spawn relativos según hacia dónde mira el personaje.
-Usa CanSetVelocity para bloquear el movimiento si es necesario (por ejemplo, durante un 
-impacto o aturdimiento).
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class Movement : CoreComponent
    {
        public Rigidbody2D RB { get; private set; }
        public int FacingDirection { get; private set; } = 1;
        public bool CanSetVelocity { get; set; } = true;
        public Vector2 CurrentVelocity { get; private set; }

        private Vector2 workspace;

        protected override void Awake()
        {
            base.Awake();
            RB = GetComponentInParent<Rigidbody2D>();
        }

        public override void LogicUpdate()
        {
            CurrentVelocity = RB.velocity;
        }

        // Establece la velocidad a cero.
        public void SetVelocityZero()
        {
            workspace = Vector2.zero;
            SetFinalVelocity();
        }

        // Establece la velocidad usando un ángulo y dirección (útil para saltos o knockbacks).
        public void SetVelocity(float velocity, Vector2 angle, int direction)
        {
            angle.Normalize();
            workspace.Set(angle.x * velocity * direction, angle.y * velocity);
            SetFinalVelocity();
        }

        // Establece la velocidad usando un vector de dirección normalizado.
        public void SetVelocity(float velocity, Vector2 direction)
        {
            workspace = direction.normalized * velocity;
            SetFinalVelocity();
        }

        // Establece solo la velocidad en X, manteniendo la Y actual.
        public void SetVelocityX(float velocity)
        {
            workspace.Set(velocity, CurrentVelocity.y);
            SetFinalVelocity();
        }

        // Establece solo la velocidad en Y, manteniendo la X actual.
        public void SetVelocityY(float velocity)
        {
            workspace.Set(CurrentVelocity.x, velocity);
            SetFinalVelocity();
        }

        // Aplica la velocidad final al Rigidbody si está permitido.
        private void SetFinalVelocity()
        {
            if (CanSetVelocity)
            {
                RB.velocity = workspace;
                CurrentVelocity = workspace;
            }
        }

        // Verifica si debe girar el sprite basado en el input horizontal.
        public void CheckIfShouldFlip(int xInput)
        {
            if (xInput != 0 && xInput != FacingDirection)
            {
                Flip();
            }
        }

        // Voltea el objeto horizontalmente.
        public void Flip()
        {
            FacingDirection *= -1;
            RB.transform.Rotate(0.0f, 180.0f, 0.0f);
        }

        // Encuentra un punto relativo al objeto, respetando la dirección hacia la que está mirando.
        public Vector2 FindRelativePoint(Vector2 offset)
        {
            offset.x *= FacingDirection;
            return transform.position + (Vector3)offset;
        }
    }
}
