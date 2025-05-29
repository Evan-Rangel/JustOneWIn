using System;
using Avocado.CoreSystem;
using UnityEngine;
using Avocado.Utilities;

/*---------------------------------------------------------------------------------------------
El script Weapon es una clase base que representa un arma equipada por un personaje. Se encarga de:
-Controlar el estado del ataque, incluyendo su inicio (Enter), su final (Exit) y el control del 
input necesario para encadenar ataques.
-Administrar un contador de ataques, útil para armas con combos de múltiples pasos.
-Respetar un cooldown para resetear el contador si el jugador no ataca por un tiempo.
-Sincronizar con la animación, gracias al AnimationEventHandler.
-Comunicar eventos importantes hacia otros sistemas (como UI, lógica de combate, etc.) mediante 
eventos públicos.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    public class Weapon : MonoBehaviour
    {
        // Eventos para comunicar cambios de estado del arma
        public event Action<bool> OnCurrentInputChange;
        public event Action OnEnter;
        public event Action OnExit;
        public event Action OnUseInput;

        // Tiempo que debe pasar antes de reiniciar el contador de ataques
        [SerializeField] private float attackCounterResetCooldown;

        // Indica si el arma puede iniciar un ataque
        public bool CanEnterAttack { get; private set; }

        // Datos de configuración del arma (ScriptableObject)
        public WeaponDataSO Data { get; private set; }
        public bool IsDataSet => Data != null;

        // Contador del ataque actual, se reinicia si supera el número total de ataques definidos
        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set {
                if (Data==null)
                {
                    //Debug.LogWarning("Weapon.Data es null al intentar incrementar CurrentAttackCounter.");
                    currentAttackCounter = 0;
                    return;
                }
                
                currentAttackCounter = value >= Data.NumberOfAttacks ? 0 : value; }
        }

        // Control del input actual (presionado o no)
        public bool CurrentInput
        {
            get => currentInput;
            set
            {
                if (currentInput != value)
                {
                    currentInput = value;
                    OnCurrentInputChange?.Invoke(currentInput); // Notifica el cambio de input
                }
            }
        }

        // Tiempo en que inició el ataque actual
        public float AttackStartTime { get; private set; }

        // Referencias a componentes importantes
        public Animator Anim { get; private set; }
        public GameObject BaseGameObject { get; private set; }
        public GameObject WeaponSpriteGameObject { get; private set; }

        // Manejador de eventos desde la animación del arma
        public AnimationEventHandler EventHandler
        {
            get
            {
                if (!initDone)
                {
                    GetDependencies(); // Carga referencias si aún no se hizo
                }

                return eventHandler;
            }
            private set => eventHandler = value;
        }

        // Referencia al sistema central de lógica del jugador
        public Core Core { get; private set; }

        // Variables internas
        private int currentAttackCounter;
        private TimeNotifier attackCounterResetTimeNotifier;
        private bool currentInput;
        private bool initDone;
        private AnimationEventHandler eventHandler;

        /// <summary>
        /// Llamado al iniciar el ataque.
        /// </summary>
        public void Enter()
        {
            // print($"{transform.name} enter");

            AttackStartTime = Time.time;

            attackCounterResetTimeNotifier.Disable();

            Anim.SetBool("active", true);
            Anim.SetInteger("counter", currentAttackCounter);

            OnEnter?.Invoke();

        }

        // Asigna la referencia al Core del jugador
        public void SetCore(Core core)
        {
            Core = core;
        }

        // Asigna los datos del arma y reinicia el contador de ataque
        public void SetData(WeaponDataSO data)
        {
            Data = data;

            if (Data is null)
                return;

            ResetAttackCounter();
        }

        public void SetCanEnterAttack(bool value) => CanEnterAttack = value;

        /// <summary>
        /// Finaliza el ataque actual.
        /// </summary>
        public void Exit()
        {

            Anim.SetBool("active", false);

            CurrentAttackCounter++;
            attackCounterResetTimeNotifier.Init(attackCounterResetCooldown); // Comienza el cooldown para reiniciar el contador

            OnExit?.Invoke();

        }

        private void Awake()
        {
            GetDependencies();

            attackCounterResetTimeNotifier = new TimeNotifier();
        }

        // Busca y guarda todas las referencias necesarias
        private void GetDependencies()
        {
            if (initDone)
                return;

            BaseGameObject = transform.Find("Base").gameObject;
            WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;

            Anim = BaseGameObject.GetComponent<Animator>();
            EventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();

            initDone = true;
        }

        // Actualiza el temporizador del reset del contador de ataques
        private void Update()
        {
            attackCounterResetTimeNotifier.Tick();
        }

        // Reinicia el contador de ataques a 0
        private void ResetAttackCounter()
        {
            CurrentAttackCounter = 0;
        }

        private void OnEnable()
        {
            EventHandler.OnUseInput += HandleUseInput;
            attackCounterResetTimeNotifier.OnNotify += ResetAttackCounter;
        }

        private void OnDisable()
        {
            EventHandler.OnUseInput -= HandleUseInput;
            attackCounterResetTimeNotifier.OnNotify -= ResetAttackCounter;
        }

        // Evento que indica cuándo se debe "usar" el input del jugador (proviene de la animación)
        private void HandleUseInput() => OnUseInput?.Invoke();
    }
}
