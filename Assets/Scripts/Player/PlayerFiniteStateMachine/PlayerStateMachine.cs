using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script define una clase PlayerStateMachine, que se encarga de manejar el flujo entre los 
distintos estados del jugador. Cada estado es una instancia de PlayerState (o una subclase de esta).
Initialize() se usa para establecer el estado inicial al comenzar el juego.
ChangeState() permite hacer una transición ordenada entre estados: primero llama al método Exit() 
del estado actual y luego al método Enter() del nuevo estado.
Este sistema es fundamental para implementar comportamientos como caminar, saltar, atacar o 
estar en el aire, cada uno encapsulado en su propio estado.
---------------------------------------------------------------------------------------------*/

public class PlayerStateMachine
{
    public PlayerState CurrentState { get; private set; }

    // Método para inicializar la máquina de estados con un estado inicial
    public void Initialize(PlayerState startingState)
    {
        CurrentState = startingState;   // Se establece el estado inicial
        CurrentState.Enter();           // Se llama al método Enter del estado inicial
    }

    // Método para cambiar de un estado actual a un nuevo estado
    public void ChangeState(PlayerState newState)
    {
        CurrentState.Exit();    // Se ejecuta la salida del estado actual
        CurrentState = newState; // Se asigna el nuevo estado
        CurrentState.Enter();   // Se ejecuta la entrada del nuevo estado
    }
}
