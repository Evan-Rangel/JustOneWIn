using System;
using Avocado.Interaction.Interactables;
using Avocado.Weapons;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script escucha el evento OnWeaponDiscarded de WeaponSwap cuando un arma es descartada:
-Instancia un objeto WeaponPickup en una posición relativa (spawnOffset).
-Aplica una velocidad inicial al objeto (spawnVelocity en spawnDirection ajustada a la 
dirección del personaje).
-Suscribe y desuscribe correctamente para evitar memory leaks o errores.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class DiscardedWeaponPickupSpawner : CoreComponent
    {
        [Header("Spawn Settings")]
        [SerializeField] private Vector2 spawnDirection;
        [SerializeField] private float spawnVelocity;
        [SerializeField] private Vector2 spawnOffset;

        [Header("References")]
        [SerializeField] private WeaponPickup weaponPickupPrefab;

        private WeaponSwap weaponSwap;
        private Movement movement;

        // Se llama cuando un arma es descartada. Instancia el arma en el mundo con una velocidad inicial.
        private void HandleWeaponDiscarded(WeaponDataSO discardedWeaponData)
        {
            var poc = transform.root.GetComponent<PlayerObjectController>();
            //if (!poc.authority ||poc==null) return;
            
            Debug.Log("Has Authority");
            // Calculas punto y dirección igual que antes
            var spawnPoint = movement.FindRelativePoint(spawnOffset);
            var adjustedSpawnDirection = new Vector2(spawnDirection.x * movement.FacingDirection, spawnDirection.y);
            var vel = adjustedSpawnDirection.normalized * spawnVelocity;
            poc.SpawnDiscardedWeapon(discardedWeaponData, spawnPoint, vel);




            // Calcula el punto de aparición relativo basado en el movimiento
           // var spawnPoint = movement.FindRelativePoint(spawnOffset);

            //var weaponPickup = Instantiate(weaponPickupPrefab, spawnPoint, Quaternion.identity);

            // Configura el objeto de pickup con los datos del arma descartada
            //weaponPickup.SetContext(discardedWeaponData);

            // Ajusta la dirección del spawn considerando la dirección a la que mira el personaje
            //var adjustedSpawnDirection = new Vector2(spawnDirection.x * movement.FacingDirection, spawnDirection.y);

            // Aplica la velocidad inicial al objeto
           // weaponPickup.Rigidbody2D.velocity = adjustedSpawnDirection.normalized * spawnVelocity;
        }

        protected override void Awake()
        {
            base.Awake();

            weaponSwap = core.GetCoreComponent<WeaponSwap>();
            movement = core.GetCoreComponent<Movement>();
        }

        private void OnEnable()
        {
            weaponSwap.OnWeaponDiscarded += HandleWeaponDiscarded;
        }

        private void OnDisable()
        {
            weaponSwap.OnWeaponDiscarded -= HandleWeaponDiscarded;
        }
    }
}
