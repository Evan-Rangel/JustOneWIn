using System;
using Avocado.Combat.Parry;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script permite que un objeto dentro del sistema de Core pueda ser "parryeado" 
Funciones principales:
-Parry(ParryData data): Dispara el evento OnParried para notificar que este objeto fue 
parryeado.
-SetParryColliderActive(bool value): Permite activar o desactivar el Collider2D que se usa 
para detectar cuándo puede ser parryeado.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class ParryReceiver : CoreComponent, IParryable
    {
        public event Action OnParried;

        private Collider2D parryCollider;

        // Método que se llama cuando se realiza un parry sobre este objeto
        public void Parry(ParryData data)
        {
            OnParried?.Invoke(); // Lanza el evento para notificar que fue parryeado
        }

        // Método para activar o desactivar el collider de parry
        public void SetParryColliderActive(bool value)
        {
            parryCollider.enabled = value;
        }

        // Inicializa referencias al despertar
        protected override void Awake()
        {
            base.Awake();

            parryCollider = GetComponent<Collider2D>();
            parryCollider.enabled = false; // Se empieza desactivado
        }
    }
}
