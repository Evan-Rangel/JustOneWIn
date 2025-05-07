using Avocado.Combat.Damage;
using Avocado.ProjectileSystem.DataPackages;
using Avocado.Utilities;
using UnityEngine;
using UnityEngine.Events;

/*---------------------------------------------------------------------------------------------
Este componente Damage se encarga de aplicar daño a los objetos detectados por el HitBox del 
proyectil. Funciona en conjunto con el sistema de paquetes de datos (ProjectileDataPackage) 
para recibir la cantidad de daño desde el arma que dispara el proyectil. Solo daña a objetos 
en ciertas capas (LayerMask) y puede desactivarse tras aplicar daño si así se configura. 
También maneja un tiempo de enfriamiento entre daños para evitar múltiples aplicaciones inmediatas.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class Damage : ProjectileComponent
    {
        // Evento que se dispara cuando se daña a un objeto que implementa IDamageable
        public UnityEvent<IDamageable> OnDamage;
        // Evento que se dispara cuando ocurre un impacto de raycast válido
        public UnityEvent<RaycastHit2D> OnRaycastHit;

        // Máscara de capas que define qué objetos pueden recibir daño
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
        // Si se debe desactivar este componente tras causar daño
        [field: SerializeField] public bool SetInactiveAfterDamage { get; private set; }
        // Tiempo mínimo entre aplicaciones de daño
        [field: SerializeField] public float Cooldown { get; private set; }

        private HitBox hitBox;         // Referencia al componente HitBox asociado
        private float amount;          // Cantidad de daño actual
        private float lastDamageTime;  // Última vez que se aplicó daño

        // Se llama cuando el proyectil se activa por primera vez
        protected override void Init()
        {
            base.Init();
            lastDamageTime = Mathf.NegativeInfinity; // Permite daño inmediato
        }

        // Lógica para procesar impactos detectados por el HitBox
        private void HandleRaycastHit2D(RaycastHit2D[] hits)
        {
            if (!Active)
                return;

            if (Time.time < lastDamageTime + Cooldown)
                return;

            foreach (var hit in hits)
            {
                // Verifica si el objeto golpeado está en una capa válida para recibir daño
                if (!LayerMaskUtilities.IsLayerInMask(hit, LayerMask))
                    continue;

                // Intenta obtener un componente que implemente IDamageable del objeto impactado
                if (!hit.collider.transform.gameObject.TryGetComponent(out IDamageable damageable))
                    continue;

                // Aplica daño al objeto
                damageable.Damage(new DamageData(amount, projectile.gameObject));

                // Dispara los eventos correspondientes
                OnDamage?.Invoke(damageable);
                OnRaycastHit?.Invoke(hit);

                lastDamageTime = Time.time;

                // Si está configurado, desactiva el componente después de dañar
                if (SetInactiveAfterDamage)
                {
                    SetActive(false);
                }

                return; // Aplica daño solo al primer objetivo válido
            }
        }

        // Extrae la cantidad de daño desde el paquete de datos recibido
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            if (dataPackage is not DamageDataPackage package)
                return;

            amount = package.Amount;
        }

        // Inicializa referencias y registra eventos
        protected override void Awake()
        {
            base.Awake();
            hitBox = GetComponent<HitBox>();
            hitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
        }

        // Limpieza de eventos al destruirse
        protected override void OnDestroy()
        {
            base.OnDestroy();
            hitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);
        }
    }
}
