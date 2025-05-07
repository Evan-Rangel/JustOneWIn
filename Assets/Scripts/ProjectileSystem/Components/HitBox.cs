using System;
using Avocado.Utilities;
using UnityEngine;
using UnityEngine.Events;

/*---------------------------------------------------------------------------------------------
El componente HitBox define una zona de colisión personalizada para los proyectiles. Utiliza un 
BoxCastAll para detectar colisiones en forma de rectángulo, considerando la dirección y velocidad 
del proyectil para evitar que atraviese objetos a alta velocidad. Si detecta alguna colisión, 
lanza un evento (OnRaycastHit2D) que otros componentes pueden escuchar, como el componente Damage. 
Además, el OnDrawGizmosSelected permite visualizar el área del hitbox en el editor.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class HitBox : ProjectileComponent
    {
        // Evento invocado cuando se detecta una colisión mediante el BoxCast
        public UnityEvent<RaycastHit2D[]> OnRaycastHit2D;

        // Rectángulo que define la forma y posición local del HitBox
        [field: SerializeField] public Rect HitBoxRect { get; private set; }

        // Define qué capas pueden ser detectadas por el HitBox
        [field: SerializeField] public LayerMask LayerMask { get; private set; }

        private RaycastHit2D[] hits; // Resultados del BoxCast
        private float checkDistance; // Distancia del BoxCast según la velocidad del proyectil

        private Transform _transform; // Referencia cacheada al transform del GameObject

        // Realiza el BoxCast y emite el evento si hay colisiones
        private void CheckHitBox()
        {
            hits = Physics2D.BoxCastAll(transform.TransformPoint(HitBoxRect.center), HitBoxRect.size, _transform.rotation.eulerAngles.z, _transform.right, checkDistance, LayerMask);

            if (hits.Length <= 0) return;

            // Llama a los listeners si se detectan colisiones
            OnRaycastHit2D?.Invoke(hits);
        }

        // Se ejecuta al iniciar el componente
        protected override void Awake()
        {
            base.Awake();
            _transform = transform; // Cachea el transform para mejorar rendimiento
        }

        // Llamado cada frame de física. Calcula la distancia y revisa colisiones
        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            checkDistance = rb.velocity.magnitude * Time.deltaTime; // Para evitar que se atraviesen objetos

            CheckHitBox();
        }

        // Dibuja el hitbox en la escena cuando se selecciona el objeto en el editor
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z), Vector3.one);

            Gizmos.matrix = rotationMatrix;

            Gizmos.DrawWireCube(HitBoxRect.center, HitBoxRect.size);
        }
    }
}
