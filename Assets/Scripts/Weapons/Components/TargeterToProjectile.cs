using Avocado.ProjectileSystem;
using Avocado.ProjectileSystem.DataPackages;

/*---------------------------------------------------------------------------------------------
El componente TargeterToProjectile actúa como un puente entre el Targeter (que detecta enemigos 
en el área) y los Projectile (proyectiles instanciados). Su función es:
Escuchar cuándo se crea un proyectil mediante el evento OnSpawnProjectile.
Recuperar la lista de enemigos detectados (targets) del Targeter.
Enviar esa información al proyectil a través del método SendDataPackage.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    // Este componente transfiere la lista de objetivos detectados por el componente Targeter al proyectil cuando es instanciado, usando un paquete de datos (TargetsDataPackage).
    public class TargeterToProjectile : WeaponComponent
    {
        // Referencia al componente que instancia proyectiles
        private ProjectileSpawner projectileSpawner;

        // Referencia al componente que detecta objetivos cercanos
        private Targeter targeter;

        // Paquete de datos que contiene la lista de objetivos a pasar al proyectil
        private readonly TargetsDataPackage targetsDataPackage = new TargetsDataPackage();

        // Evento que se ejecuta cada vez que se genera un proyectil. 
        private void HandleSpawnProjectile(Projectile projectile)
        {
            targetsDataPackage.targets = targeter.GetTargets();
            projectile.SendDataPackage(targetsDataPackage);
        }

        // En Start, obtiene referencias al spawner y al targeter, y se suscribe al evento de creación de proyectil.
        protected override void Start()
        {
            base.Start();

            projectileSpawner = GetComponent<ProjectileSpawner>();
            targeter = GetComponent<Targeter>();

            projectileSpawner.OnSpawnProjectile += HandleSpawnProjectile;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            projectileSpawner.OnSpawnProjectile -= HandleSpawnProjectile;
        }
    }
}
