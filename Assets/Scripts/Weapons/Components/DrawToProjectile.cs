using Avocado.ProjectileSystem;
using Avocado.ProjectileSystem.DataPackages;

/*---------------------------------------------------------------------------------------------
El componente DrawToProjectile transfiere el valor de carga (draw percentage) desde el sistema 
de entrada (cuando el jugador suelta el botón) hacia los proyectiles generados. Funciona como 
un puente entre los componentes Draw y ProjectileSpawner. Cuando se evalúa la curva de carga, 
guarda el resultado, y cuando se genera un proyectil, le adjunta ese valor en un paquete de 
datos (DrawModifierDataPackage), para que el proyectil pueda usarlo (por ejemplo, cambiar su 
daño, velocidad o apariencia según la carga).
Este componente conecta el sistema de "Draw" (carga del disparo) con el sistema de proyectiles.
Escucha el evento del componente Draw cuando se evalúa la curva (al soltar el botón).
Guarda ese valor.
Luego, cuando se genera un proyectil, le pasa ese valor de carga como un paquete de datos.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class DrawToProjectile : WeaponComponent
    {
        private Draw draw;
        private ProjectileSpawner projectileSpawner;

        // Paquete de datos que transportará el valor de carga (draw) al proyectil
        private readonly DrawModifierDataPackage drawModifierDataPackage = new DrawModifierDataPackage();

        // Recibe el valor evaluado del componente Draw y lo almacena en el paquete.
        private void HandleEvaluateCurve(float value)
        {
            drawModifierDataPackage.DrawPercentage = value;
        }

        // Cuando se genera un proyectil, se le envía el paquete de datos con el valor de carga.
        private void HandleSpawnProjectile(Projectile projectile)
        {
            projectile.SendDataPackage(drawModifierDataPackage);
        }

        // Al iniciar un nuevo ataque, se reinicia el valor de carga.
        protected override void HandleEnter()
        {
            drawModifierDataPackage.DrawPercentage = 0f;
        }

        // Se suscribe a los eventos de Draw y ProjectileSpawner.
        protected override void Start()
        {
            base.Start();

            draw = GetComponent<Draw>();
            projectileSpawner = GetComponent<ProjectileSpawner>();

            draw.OnEvaluateCurve += HandleEvaluateCurve;
            projectileSpawner.OnSpawnProjectile += HandleSpawnProjectile;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            draw.OnEvaluateCurve -= HandleEvaluateCurve;
            projectileSpawner.OnSpawnProjectile -= HandleSpawnProjectile;
        }
    }
}
