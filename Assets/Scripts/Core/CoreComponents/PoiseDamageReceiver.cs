using Avocado.Combat.PoiseDamage;
using Avocado.ModifierSystem;

/*---------------------------------------------------------------------------------------------
Este script permitir que el objeto que tenga este componente pueda recibir daño de Poise 
(aguante o estabilidad), generalmente usado en combates para controlar si un personaje puede 
ser aturdido o derribado.
Funciones principales:
-Aplica modificadores sobre el daño de poise recibido (reducciones por armaduras o buffs).
-Reduce el valor de Poise en el sistema de Stats cuando recibe daño.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class PoiseDamageReceiver : CoreComponent, IPoiseDamageable
    {
        private Stats stats;

        // Modificadores que pueden alterar el daño de poise recibido
        public Modifiers<Modifier<PoiseDamageData>, PoiseDamageData> Modifiers { get; } = new();

        // Método para recibir daño de poise
        public void DamagePoise(PoiseDamageData data)
        {
            // Aplica todos los modificadores al dato de daño antes de procesarlo
            data = Modifiers.ApplyAllModifiers(data);

            // Disminuye la cantidad de poise usando el sistema de estadísticas
            stats.Poise.Decrease(data.Amount);
        }

        // Inicialización: obtiene referencia al componente Stats
        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
        }
    }
}
