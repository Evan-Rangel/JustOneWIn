using Avocado.Combat.KnockBack;
using Avocado.ModifierSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script escucha eventos de KnockBack. Aplica una fuerza al personaje usando Movement.
Desactiva temporalmente la capacidad de cambiar la velocidad.Vuelve a habilitar el movimiento 
cuando:
-El personaje cae al suelo o Pasa el tiempo máximo de retroceso (maxKnockBackTime).
-Permite modificar dinámicamente el KnockBack usando un sistema de modificadores.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class KnockBackReceiver : CoreComponent, IKnockBackable
    {
        [Header("KnockBack Settings")]
        [SerializeField] private float maxKnockBackTime = 0.2f;

        public Modifiers<Modifier<KnockBackData>, KnockBackData> Modifiers { get; } = new();

        private bool isKnockBackActive;
        private float knockBackStartTime;

        private Movement movement;
        private CollisionSenses collisionSenses;

        protected override void Awake()
        {
            base.Awake();
            movement = core.GetCoreComponent<Movement>();
            collisionSenses = core.GetCoreComponent<CollisionSenses>();
        }

        public override void LogicUpdate()
        {
            CheckKnockBack();
        }

        // Recibe un golpe de retroceso (KnockBack) aplicando fuerza y deshabilitando el control de movimiento por un tiempo breve.
        public void KnockBack(KnockBackData data)
        {
            data = Modifiers.ApplyAllModifiers(data);

            movement.SetVelocity(data.Strength, data.Angle, data.Direction);
            movement.CanSetVelocity = false;
            isKnockBackActive = true;
            knockBackStartTime = Time.time;
        }

        // Verifica si el KnockBack debe terminar, ya sea porque aterrizó o pasó suficiente tiempo.
        private void CheckKnockBack()
        {
            if (!isKnockBackActive)
                return;

            bool hasLanded = movement.CurrentVelocity.y <= 0.01f && collisionSenses.Ground;
            bool timeExceeded = Time.time >= knockBackStartTime + maxKnockBackTime;

            if (hasLanded || timeExceeded)
            {
                isKnockBackActive = false;
                movement.CanSetVelocity = true;
            }
        }
    }
}
