using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //---Player Inputs Vars---//
    #region Player Movement Vars
    //Reference
    private PlayerInput playerInput;
    [SerializeField]private Camera cam;
    //Vectors
    public Vector2 RawMovementInput { get; private set; }
    public Vector2 RawDashDirectionInput { get; private set; }
    public Vector2Int DashDirectionInput { get; private set; }

    //Normalizers
    public int NormInputX { get; private set; }
    public int NormInputY { get; private set; }
    #endregion

    #region Player Inputs Vars
    //Inputs
    public bool JumpInput { get; private set; }
    public bool JumpInputStop { get; private set; }
    public bool GrabInput { get; private set; }
    public bool DashInput { get; private set; }
    public bool DashInputStop { get; private set; }
    public bool[] AttackInputs { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;
    #endregion
    //-----------------------//

    //---Unity Functions Basics---//
    #region Unity CallBack Functions
    private void Start()
    {
        //Intizlize
        playerInput = GetComponent<PlayerInput>();

        int count = Enum.GetValues(typeof(CombatInputs)).Length;
        AttackInputs = new bool[count];

        //cam = Camera.main;
    }
    private void OnEnable()
    {
        //cam = Camera.main;
    }
    private void Update()
    {
        //Constant Checks
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }
    #endregion

    //---Input Functions---//
    #region Player Input Functions
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        //Movement
        RawMovementInput = context.ReadValue<Vector2>();

        //Axe "X"
        //This to Condition give a little tolerance with a joystick to avoid giving problembs when we ant to top be in certain state like all WallStates
        /*if (Mathf.Abs(RawMovementInput.x) > 0.5f)
        {

            NormInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        }
        else
        {
            NormInputX = 0;
        }*/
        //Simplify option
        NormInputX = Mathf.RoundToInt(RawMovementInput.x);

        //Axe "Y"
        /*if (Mathf.Abs(RawMovementInput.y) > 0.5f)
        {
            NormInputY = (int)(RawMovementInput * Vector2.up).normalized.y;
        }
        else
        {
            NormInputY = 0;
        }*/
        //Simplify option
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
        {
            GrabInput = true;
        }

        if (context.canceled)
        {
            GrabInput = false;
        }
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
        if (gameObject.name=="LocalGamePlayer")
        {

            RawDashDirectionInput = context.ReadValue<Vector2>();

            if (playerInput.currentControlScheme == "Keyboard")
            {
                RawDashDirectionInput = cam.ScreenToWorldPoint((Vector3)RawDashDirectionInput) - transform.position;
            }

            DashDirectionInput = Vector2Int.RoundToInt(RawDashDirectionInput.normalized);
        }
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.primary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AttackInputs[(int)CombatInputs.secondary] = true;
        }

        if (context.canceled)
        {
            AttackInputs[(int)CombatInputs.secondary] = false;
        }
    }
    #endregion

    #region Player Input Set Functions
    public void UseJumpInput() => JumpInput = false; //--RECORDATORIO PARA PRONE--// -> Esto es lo mismo que un "public void" vacio, solo a la izquierda le di al destornillador y lo ocnverti en una version simplificada ya que esta funcion solo cambia una cosa simple que es true to false.

    public void UseDashInput() => DashInput = false;
    #endregion

    #region Player Input Check Funtions
    private void CheckJumpInputHoldTime()
    {
        //Condition that check if the time runs out to make false the jumpinput, with this avoid jumping if we spam the jump button
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            JumpInput = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        //Condition that check if the time runs out to make false the jumpinput, with this avoid jumping if we spam the jump button
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            DashInput = false;
        }
    }
    #endregion
}

#region Combat Enum
public enum CombatInputs
{
    primary,
    secondary
}
#endregion