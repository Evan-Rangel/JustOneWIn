using System;
using UnityEngine;
using UnityEngine.InputSystem; // Para usar PlayerInput y Action Maps

/*---------------------------------------------------------------------------------------------
Este script escucha el evento OnGameStateChanged del GameManager. Cambia el mapa de entrada 
(Action Map) activo para que el jugador use diferentes controles según el contexto:
-"Gameplay" → Para mover, atacar, etc.
-"UI" → Para navegar menús.
---------------------------------------------------------------------------------------------*/

public class ActionMapChanger : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private GameManager gameManager;

    // Método que responde a cambios en el estado del juego
    private void HandleGameStateChanged(GameManager.GameState state)
    {
        switch (state)
        {
            // Si el juego está en modo UI, cambia al Action Map "UI"
            case GameManager.GameState.UI:
                playerInput.SwitchCurrentActionMap("UI");
                break;

            // Si está en modo gameplay, cambia al Action Map "Gameplay"
            case GameManager.GameState.Gameplay:
                playerInput.SwitchCurrentActionMap("Gameplay");
                break;

            // Si el estado no es reconocido, lanza una excepción para depurar
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    // Suscribe el método al evento cuando este GameObject se activa
    private void OnEnable()
    {
        if (gameManager != null) // Para evitar errores si no se asignaron desde el inspector, si no solo quitar este if
            gameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    // Desuscribe el método cuando el GameObject se desactiva
    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }
}
