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
        [field: SerializeField] public Stat healthRecovery { get; private set; }

        [field: SerializeField] public Stat[] StaminaLevels { get; private set; }
        [field: SerializeField] public Stat Poise { get; private set; }
        public Stat Stamina { get; private set; }
        
        [SerializeField] private float poiseRecoveryRate;
        IEnumerator rechargeStamina;
        IEnumerator rechargeHealth;
        // Inicializa los valores de los stats cuando el objeto despierta
        protected override void Awake()
        {
            base.Awake();
            InitStats();        
        }
        public void InitStats()
        {
            Health = HealthLevels[0];
            Stamina = StaminaLevels[0];
            Poise.Init();
            Stamina.Init();
            healthRecovery.Init();
            Health.Init();

        }
        private void OnEnable()
        {
            rechargeStamina = IRechargeStamina();
            rechargeHealth = IRechargeStamina();
            healthRecovery.OnCurrentValueChangeCurrentValue += UpdateHealthRecovery;
            Stamina.OnCurrentValueDecrease += RechargeStamina;
            Stamina.OnCurrentValueChange += UpdateStamina;
            Health.OnCurrentValueChange += UpdateHealth;
            Health.OnCurrentValueDecrease += () => StopCoroutine(rechargeHealth);

        }
        private void OnDisable()
        {
            healthRecovery.OnCurrentValueChangeCurrentValue -= UpdateHealthRecovery;
            Stamina.OnCurrentValueDecrease -= RechargeStamina;
            Stamina.OnCurrentValueChange -= UpdateStamina;
            Health.OnCurrentValueChange -= UpdateHealth;
            Health.OnCurrentValueDecrease -= () => StopCoroutine(rechargeHealth);

        }
        void UpdateStamina(float _value)
        {
            CanvasManager.instance.UpdateStaminaBar(_value);
        }void UpdateHealth(float _value)
        {
            CanvasManager.instance.UpdateHealthBar(_value);
        }
        void UpdateHealthRecovery(float _value)
        {
            CanvasManager.instance.UpdateHealthRecoveryImages(_value);
        }
        public void UpdateHealthLevel(int _value)
        {
            if (_value >= HealthLevels.Length)return;
            Health = HealthLevels[_value];
            Health.OnCurrentValueChange += UpdateHealth;
            Health.Init();
        } 
        public void UpdateHealthRecoveryLevel(int _value)
        {
            healthRecovery.NewMaxValue(_value);
            healthRecovery.Init();
            RechargeHealth();
            //rechargeHealth = IRechargeHealth();
        }
        public void UpdateStaminaLevel(int _value)
        {
            if (_value >= StaminaLevels.Length)return;
            Stamina = StaminaLevels[_value];
            Stamina.OnCurrentValueDecrease += RechargeStamina;
            Stamina.OnCurrentValueChange += UpdateStamina;
            Stamina.Init();
        }
        public void OnHealthRecovery()
        {
            if (healthRecovery.CurrentValue<=0)
            {
                Debug.Log("Not Health Recovery Disponible");
                return;
            }
            healthRecovery.Decrease(1);
            RechargeHealth();
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
        void RechargeHealth()
        {
            if (rechargeHealth!=null)
                StopCoroutine(rechargeHealth);
            rechargeHealth = IRechargeHealth();
            StartCoroutine(rechargeHealth);
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
        IEnumerator IRechargeHealth()
        {
            while (Health.CurrentValue < Health.MaxValue)
            {
                Health.Increase(1);
                yield return Helpers.GetWait(0.1f);
            }
        }
    }
}
