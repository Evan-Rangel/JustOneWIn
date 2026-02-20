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
        SoundReproductor soundReproductor;

        // Sistema de modificadores que permiten alterar el daño recibido antes de aplicarlo.
        // Ejemplo: Un escudo puede reducir el daño recibido.
        public Modifiers<Modifier<DamageData>, DamageData> Modifiers { get; } = new();

        private Stats stats;
        private ParticleManager particleManager;
        RespawnController respawnController;
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            /*
            if(invulneravilityTime>0)
            invulneravilityTime -= Time.deltaTime;
        */
            }

        // Método que recibe daño, aplica modificadores, y afecta la salud.
        public void Damage(DamageData data)
        {
            if (core.invulneravilityTime > 0) return;

            // core.invulneravilityTime = 1;

            // Mostrar el daño antes de aplicar modificadores
            //print($"Damage Amount Before Modifiers: {data.Amount}");
            //GlobalVolumeController.instance.SetChromaticAberration(1);
            // Aplicar todos los modificadores activos al daño
            data = Modifiers.ApplyAllModifiers(data);

            // Mostrar el daño después de modificadores
            //print($"Damage Amount After Modifiers: {data.Amount}");
            // Si después de modificar el daño es 0 o menor, no hacer nada
            //if ( data.Amount <= 0f)
            if ( Mathf.Approximately( data.Amount, 0f)|| data.Amount<=0f)
                return;
            if (soundReproductor==null)
                soundReproductor = core.Root.GetComponent<SoundReproductor>();
            respawnController.FastDamageEffect();
            soundReproductor.PlayDamageSound();

            // Disminuir salud
            stats.Health.Decrease(data.Amount);
            GlobalVolumeController.instance.SetChromaticAberration(1);

            // GameManager.instance.UpdateHealthBar(stats.Health.CurrentValue/stats.Health.MaxValue);

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
            respawnController = transform.root.GetComponent<RespawnController>();
        }
    }
}
