using Avocado.Combat.KnockBack;
using Avocado.ProjectileSystem.DataPackages;
using Avocado.Utilities;
using UnityEngine;
using UnityEngine.Events;

/*---------------------------------------------------------------------------------------------
El componente KnockBack detecta colisiones a través del HitBox y aplica una fuerza de knockback 
a los objetos afectados. Solo reacciona a objetos en ciertas capas, y los datos como fuerza y 
ángulo del empuje los recibe desde el arma mediante un KnockBackDataPackage. Este comportamiento 
es común en juegos de acción donde los proyectiles no solo dañan, sino que también empujan a los 
enemigos al impactarlos.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class KnockBack : ProjectileComponent
    {
        // Evento que se dispara cuando se aplica knockback
        public UnityEvent OnKnockBack;

        // Capas de objetos que pueden recibir knockback
        [field: SerializeField] public LayerMask LayerMask { get; private set; }

        private HitBox hitBox;   
        
        private int direction;    
        
        private float strength;  
        
        private Vector2 angle;         

        // Llamado por el HitBox cuando detecta colisiones
        private void HandleRaycastHit2D(RaycastHit2D[] hits)
        {
            if (!Active) return; // Ignora si el componente está inactivo

            // Determina si el proyectil va a la izquierda (-1) o derecha (+1)
            direction = (int)Mathf.Sign(transform.right.x);

            foreach (var hit in hits)
            {
                // Verifica si el objeto colisionado está en una capa válida
                if (!LayerMaskUtilities.IsLayerInMask(hit, LayerMask))
                    continue;

                // Intenta obtener el componente IKnockBackable del objeto golpeado
                if (!hit.collider.transform.gameObject.TryGetComponent(out IKnockBackable knockBackable))
                    continue;

                // Aplica knockback con los datos recibidos desde el arma
                knockBackable.KnockBack(new KnockBackData(angle, strength, direction, projectile.gameObject));

                // Dispara evento
                OnKnockBack?.Invoke();

                return; // Aplica knockback solo una vez por evento
            }
        }

        // Recibe los datos desde el arma (via DataPackage)
        protected override void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
            base.HandleReceiveDataPackage(dataPackage);

            // Solo procesa si el paquete es del tipo KnockBackDataPackage
            if (dataPackage is not KnockBackDataPackage knockBackDataPackage)
                return;

            // Extrae fuerza y ángulo
            strength = knockBackDataPackage.Strength;
            angle = knockBackDataPackage.Angle;
        }

        // Inicializa referencias y suscripciones
        protected override void Awake()
        {
            base.Awake();

            hitBox = GetComponent<HitBox>();

            hitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
        }

        // Limpieza al destruir el componente
        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);
        }
    }
}
