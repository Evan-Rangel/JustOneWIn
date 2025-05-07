using Avocado.Combat.Damage;
using Avocado.ModifierSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script Componente que recibe daño en un objeto y aplica modificadores de daño.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class DamageReceiver : CoreComponent, IDamageable
    {
        [SerializeField] private GameObject damageParticles;

        // Sistema de modificadores que permiten alterar el daño recibido antes de aplicarlo.
        // Ejemplo: Un escudo puede reducir el daño recibido.
        public Modifiers<Modifier<DamageData>, DamageData> Modifiers { get; } = new();

        private Stats stats;
        private ParticleManager particleManager;

        // Método que recibe daño, aplica modificadores, y afecta la salud.
        public void Damage(DamageData data)
        {
            // Mostrar el daño antes de aplicar modificadores
            print($"Damage Amount Before Modifiers: {data.Amount}");

            // Aplicar todos los modificadores activos al daño
            data = Modifiers.ApplyAllModifiers(data);

            // Mostrar el daño después de modificadores
            print($"Damage Amount After Modifiers: {data.Amount}");

            // Si después de modificar el daño es 0 o menor, no hacer nada
            if (data.Amount <= 0f)
                return;

            // Disminuir salud
            stats.Health.Decrease(data.Amount);

            // Lanzar partículas de daño
            particleManager.StartWithRandomRotation(damageParticles);
        }

        // Inicialización del componente, buscando las dependencias necesarias.
        protected override void Awake()
        {
            base.Awake();

            // Obtener referencias a otros componentes del Core
            stats = core.GetCoreComponent<Stats>();
            particleManager = core.GetCoreComponent<ParticleManager>();
        }
    }
}
