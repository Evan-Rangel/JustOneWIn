using System;
using Avocado.Weapons;
using Mirror;
using UnityEngine;
using UnityEngine.Serialization;

/*---------------------------------------------------------------------------------------------
Este script representa un objeto arma en el mundo que el jugador puede recoger.
Detalles:
-Usa IInteractable<WeaponDataSO>, permitiendo que otros sistemas puedan interactuar con este objeto y obtener su información de arma (WeaponDataSO).
-Al ser interactuado (Interact()), se destruye a sí mismo (simulando ser recogido).
-Tiene un sistema de bobbing visual para indicar que es interactuable (Bobber).
-Al asignar un WeaponDataSO, actualiza automáticamente el ícono (SpriteRenderer).
Componentes requeridos:
-Un Rigidbody2D (obligatorio por [RequireComponent]).
---------------------------------------------------------------------------------------------*/

namespace Avocado.Interaction.Interactables
{
    // Obliga a que este objeto tenga un Rigidbody2D
    [RequireComponent(typeof(Rigidbody2D))]
    public class WeaponPickup : MonoBehaviour, IInteractable<WeaponDataSO>
    {
        [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }

        [SerializeField] private SpriteRenderer weaponIcon; // Icono visual del arma
        [SerializeField] private Bobber bobber;             // Efecto visual de bobbing (flotar hacia arriba y abajo)

        [SerializeField] private WeaponDataSO weaponData;   // Datos del arma que este pickup contiene

        // Implementación de IInteractable<WeaponDataSO>

        // Devuelve los datos del arma
        public WeaponDataSO GetContext() => weaponData;

        // Establece nuevos datos de arma, actualizando el icono también
        public void SetContext(WeaponDataSO context)
        {
            weaponData = context;
            weaponIcon.sprite = weaponData.Icon;
        }

        // Lógica cuando el jugador interactúa con el objeto (lo recoge)
        public void Interact()
        {
            Destroy(gameObject);
        }

        // Habilita interacción visual (empieza el efecto de bobbing)
        public void EnableInteraction()
        {
            bobber.StartBobbing();
        }

        // Desactiva interacción visual (detiene el bobbing)
        public void DisableInteraction()
        {
            bobber.StopBobbing();
        }

        // Devuelve la posición del pickup en el mundo
        public Vector3 GetPosition()
        {
            return transform.position;
        }

        private void Awake()
        {
            // Asegura que Rigidbody2D y weaponIcon están asignados (si no se asignaron manualmente en el editor)
            Rigidbody2D ??= GetComponent<Rigidbody2D>();
            weaponIcon ??= GetComponentInChildren<SpriteRenderer>();

            // Si ya tiene un arma asignada, actualiza el icono visual
            if (weaponData is null)
                return;

            weaponIcon.sprite = weaponData.Icon;
        }
    }
}
