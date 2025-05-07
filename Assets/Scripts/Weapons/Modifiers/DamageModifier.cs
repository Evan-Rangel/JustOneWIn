using System;
using Avocado.ModifierSystem;             
using Avocado.Utilities;                  
using Avocado.Weapons.Components;         
using UnityEngine;                        
using DamageData = Avocado.Combat.Damage.DamageData;
using Movement = Avocado.CoreSystem.Movement;

/*---------------------------------------------------------------------------------------------
Este script define una clase DamageModifier que extiende el sistema de modificadores para alterar 
el daño recibido por un personaje cuando está bloqueando. El modificador se activa antes de aplicar 
el daño real, verifica si el ataque es bloqueable (según su ángulo y el sistema de DirectionalInformation), 
y si lo es, reduce el daño según el valor de absorción definido en la configuración del bloque.
Además, lanza un evento (OnModified) cuando el bloqueo es exitoso, lo cual es útil para efectos 
visuales o retroalimentación al jugador.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Modifiers
{
    public class DamageModifier : Modifier<DamageData>
    {
        // Evento que se dispara si el bloqueo fue exitoso. Puede usarse para efectos visuales o sonidos.
        public event Action<GameObject> OnModified;

        // Delegado que indica si el ataque fue bloqueado y retorna información direccional del bloqueo
        private readonly ConditionalDelegate isBlocked;

        // Constructor que recibe el delegado para validación del bloqueo
        public DamageModifier(ConditionalDelegate isBlocked)
        {
            this.isBlocked = isBlocked;
        }
        
        // Método principal que modifica el valor del daño recibido.
        // Recibe un objeto DamageData, verifica si el ángulo del ataque está dentro del rango bloqueable.
        // Si el ataque es bloqueado, reduce el daño usando el valor de absorción definido.
        public override DamageData ModifyValue(DamageData value)
        {
            // Verifica si el ataque fue bloqueado usando el delegado
            if (isBlocked(value.Source.transform, out var blockDirectionInformation))
            {
                // Reduce el daño basado en el porcentaje de absorción
                value.SetAmount(value.Amount * (1 - blockDirectionInformation.DamageAbsorption));

                // Lanza el evento de bloqueo exitoso
                OnModified?.Invoke(value.Source);
            }

            // Retorna el daño (modificado o no)
            return value;
        }
    }
}
