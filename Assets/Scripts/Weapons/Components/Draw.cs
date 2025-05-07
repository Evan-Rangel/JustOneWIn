using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
El componente Draw evalúa una curva de carga (DrawCurve) basada en cuánto tiempo se mantuvo 
presionado el botón de ataque. Cuando el jugador suelta el botón, se calcula el valor evaluado 
de la curva (drawPercentage), que representa el nivel de carga del ataque, y se emite el evento 
OnEvaluateCurve.
Esto es útil para ataques que cambian su comportamiento según cuánto tiempo se cargaron 
(como un arco o un disparo potenciado).
Este componente evalúa una curva cuando se suelta el botón de ataque. Luego, emite un evento 
con el valor evaluado, indicando qué tan "cargado" estuvo el ataque.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    public class Draw : WeaponComponent<DrawData, AttackDraw>
    {
        // Evento que se dispara cuando se evalúa la curva de carga. Entrega el valor evaluado (0 a 1).
        public event Action<float> OnEvaluateCurve;

        // Indica si ya se evaluó la curva en esta ejecución del ataque
        private bool hasEvaluatedDraw;

        // Valor final evaluado de la curva (porcentaje de carga)
        private float drawPercentage;

        // Reinicia variables al comenzar el ataque
        protected override void HandleEnter()
        {
            base.HandleEnter();
            hasEvaluatedDraw = false;
        }

        // Se ejecuta cuando el estado del input cambia (se suelta el botón).Si se suelta y aún no se ha evaluado la curva, la evalúa.
        private void HandleCurrentInputChange(bool newInput)
        {
            if (newInput || hasEvaluatedDraw)
                return;

            EvaluateDrawPercentage();
        }

        // Calcula cuánto tiempo se mantuvo presionado el input, lo evalúa en una curva y lanza el evento.
        private void EvaluateDrawPercentage()
        {
            hasEvaluatedDraw = true;

            // Evalúa la curva usando el tiempo de carga normalizado (entre 0 y 1)
            drawPercentage = currentAttackData.DrawCurve.Evaluate(
                Mathf.Clamp(
                    (Time.time - attackStartTime) / currentAttackData.DrawTime,
                    0f, 1f));

            // Dispara el evento con el resultado
            OnEvaluateCurve?.Invoke(drawPercentage);
        }

        // Se suscribe al evento del input cuando se instancia el componente
        protected override void Awake()
        {
            base.Awake();
            weapon.OnCurrentInputChange += HandleCurrentInputChange;
        }

        // Se desuscribe del evento al destruirse el componente
        protected override void OnDestroy()
        {
            base.OnDestroy();
            weapon.OnCurrentInputChange -= HandleCurrentInputChange;
        }
    }
}
