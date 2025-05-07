using System;
using Avocado.CoreSystem.StatsSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script administrar los atributos principales de un personaje, específicamente Salud 
(Health) y Poise (aguante o resistencia).
Funciones principales:
-Inicializa ambos Stat (Health y Poise) al iniciar (Awake).
-Cada frame (Update), recupera automáticamente el Poise si no está lleno.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class Stats : CoreComponent
    {
        [field: SerializeField] public Stat Health { get; private set; }

        [field: SerializeField] public Stat Poise { get; private set; }

        [SerializeField] private float poiseRecoveryRate;

        // Inicializa los valores de los stats cuando el objeto despierta
        protected override void Awake()
        {
            base.Awake();

            Health.Init();
            Poise.Init();
        }

        // Actualización continua para recuperar Poise
        private void Update()
        {
            // Si el Poise ya está al máximo, no hace nada
            if (Poise.CurrentValue.Equals(Poise.MaxValue))
                return;

            // Aumenta el Poise poco a poco con el tiempo
            Poise.Increase(poiseRecoveryRate * Time.deltaTime);
        }
    }
}
