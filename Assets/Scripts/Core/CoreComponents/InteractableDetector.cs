using System;
using System.Collections.Generic;
using Avocado.Interaction;
using Avocado.Utilities;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script detecta los objetos interactuables dentro del Collider2D, manteniendo referencia 
al más cercano. Permite intentar interactuar con el más cercano mediante el método TryInteract.
Actualiza visualmente (activar/desactivar estado de interacción) al cambiar el objeto más cercano.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableDetector : CoreComponent
    {
        [Header("Events")]
        public Action<IInteractable> OnTryInteract;

        private readonly List<IInteractable> interactables = new();
        private IInteractable closestInteractable;
        private float distanceToClosestInteractable = float.PositiveInfinity;

        // Llama a la interacción con el objeto más cercano si el input es válido.
        [ContextMenu("TryInteract")]
        public void TryInteract(bool inputValue)
        {
            if (!inputValue || closestInteractable is null)
                return;

            OnTryInteract?.Invoke(closestInteractable);
        }

        private void Update()
        {
            if (interactables.Count == 0)
                return;

            var oldClosestInteractable = closestInteractable;
            distanceToClosestInteractable = closestInteractable != null
                ? FindDistanceTo(closestInteractable)
                : float.PositiveInfinity;

            foreach (var interactable in interactables)
            {
                if (interactable == closestInteractable)
                    continue;

                float distance = FindDistanceTo(interactable);

                if (distance < distanceToClosestInteractable)
                {
                    closestInteractable = interactable;
                    distanceToClosestInteractable = distance;
                }
            }

            // Cambiar interacción visual si cambió el interactuable más cercano
            if (closestInteractable != oldClosestInteractable)
            {
                oldClosestInteractable?.DisableInteraction();
                closestInteractable?.EnableInteraction();
            }
        }

        private float FindDistanceTo(IInteractable interactable)
        {
            return Vector3.Distance(transform.position, interactable.GetPosition());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.IsInteractable(out var interactable))
            {
                interactables.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.IsInteractable(out var interactable))
            {
                interactables.Remove(interactable);

                if (interactable == closestInteractable)
                {
                    interactable.DisableInteraction();
                    closestInteractable = null;
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var interactable in interactables)
            {
                Gizmos.color = interactable == closestInteractable ? Color.red : Color.white;
                Gizmos.DrawLine(transform.position, interactable.GetPosition());
            }
        }
    }
}
