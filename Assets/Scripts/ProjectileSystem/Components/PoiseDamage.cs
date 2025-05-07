using Avocado.Combat.PoiseDamage;
using Avocado.Interfaces;
using Avocado.ProjectileSystem.DataPackages;
using Avocado.Utilities;
using UnityEngine;
using UnityEngine.Events;

/*---------------------------------------------------------------------------------------------
El componente PoiseDamage permite que un proyectil cause daño a la "poise" de un enemigo. 
Funciona en conjunto con el componente HitBox, que detecta colisiones. Cuando se detecta un 
impacto, se revisa si el objeto impactado está en la capa deseada y si puede recibir daño a la 
poise. Si cumple ambas condiciones, se le aplica el daño configurado, el cual proviene de un 
paquete de datos (PoiseDamageDataPackage) enviado desde el arma que disparó el proyectil.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class PoiseDamage : ProjectileComponent
    {
        // Evento que se dispara cuando se causa daño a la poise
        public UnityEvent OnPoiseDamage;

        // Capa de objetos a los que se les puede aplicar daño a la poise
        [field: SerializeField] public LayerMask LayerMask { get; private set; }

        private float amount;

        private HitBox hitBox;

        // Método que se ejecuta cuando el HitBox detecta un impacto
        private void HandleRaycastHit2D(RaycastHit2D[] hits)
        {
            if (!Active)
                return;

            foreach (var hit in hits)
            {
                // Verifica si el objeto impactado está en la capa correcta
                if (!LayerMaskUtilities.IsLayerInMask(hit, LayerMask))
                    continue;

                // Verifica si el objeto impactado implementa IPoiseDamageable (puede recibir daño a la poise)
                if (!hit.collider.transform.gameObject.TryGetComponent(out IPoiseDamageable poiseDamageable))
                    continue;

                // Aplica daño a la poise
                poiseDamageable.DamagePoise(new PoiseDamageData(amount, projectile.gameObject));

                // Dispara el evento
                OnPoiseDamage?.Invoke();

                // Solo se aplica a un objetivo, por eso se hace return
                return;
            }
        }

        // Recibe el paquete de datos del arma y extrae la cantidad de daño a la poise
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            if (dataPackage is not PoiseDamageDataPackage package)
                return;

            amount = package.Amount;
        }

        // Se ejecuta al inicializar el componente
        protected override void Awake()
        {
            base.Awake();

            hitBox = GetComponent<HitBox>();

            // Se suscribe al evento de colisión del HitBox
            hitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
        }

        // Se ejecuta al destruir el componente
        protected override void OnDestroy()
        {
            base.OnDestroy();

            // Se desuscribe del evento del HitBox para evitar errores
            hitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);
        }
    }
}
