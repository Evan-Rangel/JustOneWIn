using Avocado.ProjectileSystem.DataPackages;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
El componente Graphics permite personalizar visualmente cada proyectil según la información 
enviada por el arma. Cuando el arma lanza un proyectil, puede adjuntar un SpriteDataPackage que 
contiene un sprite específico. Este sprite se guarda en el componente y se asigna al SpriteRenderer 
en el método Init, asegurando que el proyectil tenga el aspecto adecuado cuando aparece en escena.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class Graphics : ProjectileComponent
    {
        private Sprite sprite; // Sprite recibido desde el arma a través de un paquete de datos

        private SpriteRenderer spriteRenderer; // Referencia al SpriteRenderer en el hijo "Graphics"

        // Método llamado cuando se inicializa el proyectil
        protected override void Init()
        {
            base.Init();

            // Asigna el sprite al renderer cuando el proyectil es inicializado
            spriteRenderer.sprite = sprite;
        }

        // Maneja la recepción de paquetes de datos
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            // Si el paquete no es del tipo esperado, se ignora
            if (dataPackage is not SpriteDataPackage spriteDataPackage)
                return;

            // Se guarda el sprite recibido para asignarlo más adelante en Init
            sprite = spriteDataPackage.Sprite;
        }

        // Se ejecuta al iniciar el componente
        protected override void Awake()
        {
            base.Awake();

            // Obtiene el SpriteRenderer del hijo (usualmente un GameObject llamado "Graphics")
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
}
