using UnityEngine;

/*---------------------------------------------------------------------------------------------
El componente InputHold controla una animación basada en si el jugador está manteniendo 
presionado un botón de ataque, y si ha pasado un tiempo mínimo requerido para realizar una 
acción especial. Usa un parámetro booleano "hold" en el Animator para controlar el estado 
visual del ataque o carga. El sistema responde tanto al cambio del input del jugador como a 
un evento (OnMinHoldPassed) emitido por la animación (probablemente desde un AnimationEvent).
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class InputHold : WeaponComponent
    {
        private Animator anim; // Referencia al Animator del objeto

        private bool input; // Estado actual del input (si se está manteniendo presionado)
        private bool minHoldPassed; // Indica si se ha pasado el tiempo mínimo de carga

        // Se ejecuta al comenzar el ataque. Reinicia el estado del mínimo tiempo de carga.
        protected override void HandleEnter()
        {
            base.HandleEnter();
            minHoldPassed = false;
        }

        // Se llama cuando el input cambia (por ejemplo, presionar o soltar el botón).
        private void HandleCurrentInputChange(bool newInput)
        {
            input = newInput;
            SetAnimatorParameter();
        }

        // Se llama cuando se ha cumplido el tiempo mínimo que se debe mantener el botón presionado.
        private void HandleMinHoldPassed()
        {
            minHoldPassed = true;
            SetAnimatorParameter();
        }

        // Cambia el parámetro "hold" del Animator en base al input y al tiempo mínimo.
        private void SetAnimatorParameter()
        {
            if (input)
            {
                anim.SetBool("hold", input);
                return;
            }

            // Si se soltó el botón y se ha cumplido el tiempo mínimo, se deja de hacer "hold"
            if (minHoldPassed)
            {
                anim.SetBool("hold", false);
            }
        }

        // Inicializa el componente, conecta eventos y obtiene el Animator.
        protected override void Awake()
        {
            base.Awake();

            anim = GetComponentInChildren<Animator>();

            weapon.OnCurrentInputChange += HandleCurrentInputChange;
            AnimationEventHandler.OnMinHoldPassed += HandleMinHoldPassed;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            weapon.OnCurrentInputChange -= HandleCurrentInputChange;
            AnimationEventHandler.OnMinHoldPassed -= HandleMinHoldPassed;
        }
    }
}
