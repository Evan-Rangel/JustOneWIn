using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/*---------------------------------------------------------------------------------------------
Este script se encarga de manejar todos los inputs del jugador. Usa el sistema de Input Actions 
de Unity (PlayerInput) para recibir eventos como movimiento, salto, dash, ataques, etc. Cada 
input se almacena en propiedades que luego pueden ser usadas por otros scripts (por ejemplo, 
los estados del jugador).
También se incluye lógica para controlar la duración de algunos inputs (como salto o dash), 
lo cual permite que aunque el jugador suelte el botón rápidamente, la acción aún pueda 
ejecutarse si se mantiene dentro de una ventana de tiempo (inputHoldTime).
---------------------------------------------------------------------------------------------*/

public class PlayerInputHandler : MonoBehaviour
{
    public event Action<bool> OnInteractInputChanged;
    // Evento que notifica si se ha iniciado o cancelado una interacción

    private PlayerInput playerInput;
    private Camera cam;

    // Propiedades para obtener el input en bruto y normalizado
    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }

    // Estados de botones de acción
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }

    public bool[] AttackInputs { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;
    // Tiempo en que el input permanece activo aunque se haya soltado

    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count]; // Inicializa array para distintos ataques

        cam = Camera.main; // Obtiene la cámara principal
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnInteractInputChanged?.Invoke(true);
            return;
        }

        if (context.canceled)
        {
            OnInteractInputChanged?.Invoke(false);
        }
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
            AttackInputs[(int)CombatInputs.primary] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.primary] = false;
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
            AttackInputs[(int)CombatInputs.secondary] = true;

        if (context.canceled)
            AttackInputs[(int)CombatInputs.secondary] = false;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();
        NormInputX = Mathf.RoundToInt(RawMovementInput.x);
        NormInputY = Mathf.RoundToInt(RawMovementInput.y);
    }

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            JumpInput = true;
            JumpInputStop = false;
            jumpInputStartTime = Time.time;
        }

        if (context.canceled)
        {
            JumpInputStop = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
            GrabInput = true;

        if (context.canceled)
            GrabInput = false;
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            DashInput = true;
            DashInputStop = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            DashInputStop = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        RawDashDirectionInput = context.ReadValue<Vector2>();

        // Si el esquema es teclado, convierte el input a dirección relativa al mundo
        if (playerInput.currentControlScheme == "Keyboard")
        {
            RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
        }

        DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
    }

    public void UseJumpInput() => JumpInput = false;
    public void UseDashInput() => DashInput = false;
    public void UseAttackInput(int i) => AttackInputs[i] = false;

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
            JumpInput = false;
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
            DashInput = false;
    }
}

public enum CombatInputs
{
    primary,
    secondary
}
