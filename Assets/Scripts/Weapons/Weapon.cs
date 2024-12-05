using System;
using Avocado.CoreSystem;
using UnityEngine;
using Avocado.Utilities;

namespace Avocado.Weapons
{
    public class Weapon : MonoBehaviour
    {
        #region References
        public WeaponDataSO Data { get; private set; }

        private Animator anim;
        public AnimationEventHandler EventHandler { get; private set; }

        public Core Core { get; private set; }
        public GameObject BaseGameObject {get; private set;}
        public GameObject WeaponSpriteGameObject { get; private set;}
        #endregion

        #region Events
        public event Action OnEnter;
        public event Action OnExit;

        public event Action<bool> OnCurrentInputChange;
        #endregion

        #region Timers
        private Timer attackCounterResetTimer;
        #endregion

        #region Properties

        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value >= Data.NumberOfAttacks ? 0 : value; 
        }
        private int currentAttackCounter;


        public bool CurrentInput
        {
            get => currentInput;
            set
            {
                if (currentInput != value)
                {
                    currentInput = value;
                    OnCurrentInputChange?.Invoke(currentInput);
                }
            }
        }
        private bool currentInput;
        #endregion

        #region Floats
        [SerializeField] private float attackCounterResetCooldown;
        #endregion

        #region Functions
        public void Enter()
        {
            //print($"{transform.name} enter");

            attackCounterResetTimer.StopTimer();

            anim.SetBool("active", true);
            anim.SetInteger("counter", currentAttackCounter);

            OnEnter?.Invoke();
        }

        public void SetCore(Core core)
        {
            Core = core;
        }

        public void SetData(WeaponDataSO data)
        {
            Data = data;
        }

        private void Exit()
        {
            anim.SetBool("active", false);

            CurrentAttackCounter++;

            attackCounterResetTimer.StartTimer();

            OnExit?.Invoke();
        }

        private void Awake()
        {
            BaseGameObject = transform.Find("Base").gameObject;
            WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;

            anim = BaseGameObject.GetComponent<Animator>();

            EventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();

            attackCounterResetTimer = new Timer(attackCounterResetCooldown);
        }

        private void Update()
        {
            attackCounterResetTimer.Tick();
        }

        private void ResetAttackCounter() => CurrentAttackCounter = 0;

        private void OnEnable()
        {
            EventHandler.OnFinish += Exit;
            attackCounterResetTimer.OnTimerDone += ResetAttackCounter;
        }

        private void OnDisable()
        {
            EventHandler.OnFinish -= Exit;
            attackCounterResetTimer.OnTimerDone -= ResetAttackCounter;
        }
        #endregion
    }
}


