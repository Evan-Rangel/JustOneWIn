using System;
using System.Collections;
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
        public Stat Health { get; private set; }
        [field: SerializeField] public Stat[] HealthLevels { get; private set; }

        [field: SerializeField] public Stat[] StaminaLevels { get; private set; }
        [field: SerializeField] public Stat Poise { get; private set; }
        public Stat Stamina { get; private set; }

        [SerializeField] private float poiseRecoveryRate;
        IEnumerator rechargeStamina;
        // Inicializa los valores de los stats cuando el objeto despierta
        protected override void Awake()
        {
            base.Awake();
            Health= HealthLevels[0];
            Stamina = StaminaLevels[0];
            Health.Init();
            Poise.Init();
            Stamina.Init();
        }
        private void OnEnable()
        {
            rechargeStamina = IRechargeStamina();
            Stamina.OnCurrentValueDecrease += RechargeStamina;
            Stamina.OnCurrentValueChange += UpdateStamina;
            Health.OnCurrentValueChange += UpdateHealth;
        }
        private void OnDisable()
        {
            Stamina.OnCurrentValueDecrease -= RechargeStamina;
            Stamina.OnCurrentValueChange -= UpdateStamina;
            Health.OnCurrentValueChange -= UpdateHealth;
        }
        void UpdateStamina(float _value)
        {
            GameManager.instance.UpdateStaminaBar(_value);
        }void UpdateHealth(float _value)
        {
            GameManager.instance.UpdateHealthBar(_value);
        }
        public void UpdateHealthLevel(int _value)
        {
            if (_value >= HealthLevels.Length)return;
            Debug.Log("Updating Health Level to: " + _value);
            Health = HealthLevels[_value];
            Health.OnCurrentValueChange += UpdateHealth;
            Health.Init();
        }
        public void UpdateStaminaLevel(int _value)
        {
            if (_value >= StaminaLevels.Length)return;
            Stamina = StaminaLevels[_value];
            Stamina.OnCurrentValueDecrease += RechargeStamina;
            Stamina.OnCurrentValueChange += UpdateStamina;
            Stamina.Init();
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
        void RechargeStamina()
        {
            StopCoroutine(rechargeStamina);
            rechargeStamina = IRechargeStamina();
            StartCoroutine(rechargeStamina);
        }
        IEnumerator IRechargeStamina()
        {
            yield return Helpers.GetWait(1.5f);

            while (Stamina.CurrentValue < Stamina.MaxValue)
            {
                Stamina.Increase(1);
                yield return Helpers.GetWait(0.1f);
            }
        }
    }
}
