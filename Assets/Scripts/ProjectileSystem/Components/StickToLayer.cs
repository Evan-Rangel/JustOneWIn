using System;
using System.Collections;
using Avocado.Utilities;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

/*---------------------------------------------------------------------------------------------
El componente StickToLayer permite que un proyectil se quede pegado a un objeto si colisiona 
con uno de cierta capa (LayerMask). Cuando se pega, se detiene, se vuelve estático y opcionalmente 
cambia su capa de dibujo (sortingLayer). Si el objeto al que está pegado desaparece o es destruido, 
el proyectil se libera automáticamente y vuelve a ser afectado por la física. También puedes usar 
los eventos setStuck y setUnstuck para activar o desactivar otros componentes como daño o efectos
visuales.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    [RequireComponent(typeof(HitBox))]
    public class StickToLayer : ProjectileComponent
    {
        // Eventos que puedes configurar desde el editor para ejecutar lógica cuando el proyectil se queda pegado o se despega.
        [SerializeField] public UnityEvent setStuck;
        [SerializeField] public UnityEvent setUnstuck;

        // Capas con las que el proyectil puede quedarse pegado
        [field: SerializeField] public LayerMask LayerMask { get; private set; }

        // Información visual al quedarse pegado
        [field: SerializeField] public string InactiveSortingLayerName { get; private set; }
        [field: SerializeField] public float CheckDistance { get; private set; }

        private bool isStuck;
        private bool subscribedToDisableNotifier;

        private HitBox hitBox;
        private string activeSortingLayerName;
        private SpriteRenderer sr;

        private OnDisableNotifier onDisableNotifier;

        private Transform referenceTransform;
        private Transform _transform;

        private Vector3 offsetPosition;
        private Quaternion offsetRotation;

        private float gravityScale;

        // Función que se llama al detectar colisiones por raycast del HitBox
        private void HandleRaycastHit2D(RaycastHit2D[] hits)
        {
            if (isStuck)
                return;

            SetStuck();

            // Se lanza un raycast hacia adelante para encontrar un punto de impacto más preciso
            var lineHit = Physics2D.Raycast(_transform.position, _transform.right, CheckDistance, LayerMask);

            if (lineHit)
            {
                SetReferenceTransformAndPoint(lineHit.transform, lineHit.point);
                return;
            }

            // Si el raycast directo falla, revisa los impactos del HitBox
            foreach (var hit in hits)
            {
                if (!LayerMaskUtilities.IsLayerInMask(hit, LayerMask))
                    continue;

                SetReferenceTransformAndPoint(hit.transform, hit.point);
                return;
            }

            // Si no hay nada válido, se libera el proyectil
            SetUnstuck();
        }

        // Asocia el proyectil a un transform externo y guarda su offset
        private void SetReferenceTransformAndPoint(Transform newReferenceTransform, Vector2 newPoint)
        {
            if (newReferenceTransform.TryGetComponent(out onDisableNotifier))
            {
                onDisableNotifier.OnDisableEvent += HandleDisableNotifier;
                subscribedToDisableNotifier = true;
            }

            _transform.position = newPoint;

            referenceTransform = newReferenceTransform;
            offsetPosition = _transform.position - referenceTransform.position;
            offsetRotation = Quaternion.Inverse(referenceTransform.rotation) * _transform.rotation;
        }

        // Cambia el estado del proyectil a "pegado"
        private void SetStuck()
        {
            isStuck = true;

            sr.sortingLayerName = InactiveSortingLayerName;
            rb.velocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Static;

            setStuck?.Invoke();
        }

        // Libera el proyectil y lo deja caer
        private void SetUnstuck()
        {
            isStuck = false;

            sr.sortingLayerName = activeSortingLayerName;
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = gravityScale;

            setUnstuck?.Invoke();
        }

        // Se llama si el objeto al que está pegado se destruye/desactiva
        private void HandleDisableNotifier()
        {
            SetUnstuck();

            if (!subscribedToDisableNotifier)
                return;

            onDisableNotifier.OnDisableEvent -= HandleDisableNotifier;
            subscribedToDisableNotifier = false;
        }

        // Si el proyectil se reinicia, se deshace el estado pegado
        protected override void ResetProjectile()
        {
            base.ResetProjectile();
            SetUnstuck();
        }

        protected override void Awake()
        {
            base.Awake();

            gravityScale = rb.gravityScale;
            _transform = transform;

            sr = GetComponentInChildren<SpriteRenderer>();
            activeSortingLayerName = sr.sortingLayerName;

            hitBox = GetComponent<HitBox>();
            hitBox.OnRaycastHit2D.AddListener(HandleRaycastHit2D);
        }

        // Si el proyectil está pegado, actualiza su posición y rotación con respecto al objeto al que está pegado
        protected override void Update()
        {
            base.Update();

            if (!isStuck)
                return;

            if (!referenceTransform)
            {
                SetUnstuck();
                return;
            }

            var referenceRotation = referenceTransform.rotation;
            _transform.position = referenceTransform.position + referenceRotation * offsetPosition;
            _transform.rotation = referenceRotation * offsetRotation;
        }

        // Limpieza de eventos al destruir el objeto
        protected override void OnDestroy()
        {
            base.OnDestroy();

            hitBox.OnRaycastHit2D.RemoveListener(HandleRaycastHit2D);

            if (subscribedToDisableNotifier)
                onDisableNotifier.OnDisableEvent -= HandleDisableNotifier;
        }
    }
}
