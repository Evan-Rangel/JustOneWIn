using Avocado.ProjectileSystem.DataPackages;

/*---------------------------------------------------------------------------------------------
El componente DrawModifyDelayedGravity actúa como puente entre el sistema de armas y el 
comportamiento de gravedad retardada (DelayedGravity). Cuando el arma dispara un proyectil, 
puede enviar un DrawModifierDataPackage que incluye un valor de carga (DrawPercentage), el 
cual se usa para modificar cuánto debe viajar el proyectil antes de que la gravedad empiece a 
actuar. Esto permite que los proyectiles que han sido cargados más tiempo (por ejemplo un arco) 
vuelen más lejos en línea recta antes de caer.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class DrawModifyDelayedGravity : ProjectileComponent
    {
        private DelayedGravity delayedGravity;

        // Este método se llama cuando el proyectil recibe un paquete de datos
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            // Si el paquete no es del tipo esperado, se ignora
            if (dataPackage is not DrawModifierDataPackage drawModifierDataPackage)
                return;

            // Se modifica el multiplicador de distancia en el componente DelayedGravity
            // usando el porcentaje de "draw" (carga) proporcionado por el arma
            delayedGravity.distanceMultiplier = drawModifierDataPackage.DrawPercentage;
        }

        // Se ejecuta cuando el componente es instanciado
        protected override void Awake()
        {
            base.Awake();

            // Obtiene la referencia al componente DelayedGravity en el mismo GameObject
            delayedGravity = GetComponent<DelayedGravity>();
        }
    }
}
