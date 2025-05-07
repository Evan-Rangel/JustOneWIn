using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script actúa como un intermediario entre los eventos de animación y la lógica de ataque 
(AttackState).
¿Qué hace cada método?
-TriggerAttack(): Llama a TriggerAttack en el estado de ataque (normalmente al comenzar una 
animación de ataque).
-FinishAttack(): Llama a FinishAttack en el estado de ataque (normalmente al terminar la 
animación de ataque).
-SetParryWindowActive(int value): Activa o desactiva una "ventana" para poder realizar 
parry (defensa/contraataque) basada en un valor int (probablemente enviado por el animador 
como 0 o 1).
¿Cómo se usa en práctica?
-Se colocan eventos de animación que llaman a estos métodos justo en los frames importantes 
(ej.: cuando el golpe empieza o termina).
-La animación manda señales, pero el control de la lógica lo sigue llevando AttackState.
---------------------------------------------------------------------------------------------*/

public class AnimationToStatemachine : MonoBehaviour
{
    // Referencia al estado de ataque
    public AttackState attackState;

    // Método llamado desde un evento de animación para iniciar el ataque
    private void TriggerAttack()
    {
        attackState.TriggerAttack();
    }

    // Método llamado desde un evento de animación para finalizar el ataque
    private void FinishAttack()
    {
        attackState.FinishAttack();
    }

    // Método llamado desde un evento de animación para activar/desactivar la ventana de parry
    private void SetParryWindowActive(int value)
    {
        attackState.SetParryWindowActive(Convert.ToBoolean(value));
    }
}
