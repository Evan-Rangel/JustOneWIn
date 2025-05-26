using System.Collections;
using System.Collections.Generic;
using Avocado.Weapons;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estdo PlayerAttackState es una subclase de PlayerAbilityState que controla el 
comportamiento del jugador cuando realiza un ataque. Se asocia a un arma (Weapon) y un tipo de 
ataque (primario o secundario), y se suscribe a eventos que ocurren durante el ciclo de ataque 
como el inicio, interrupción, flip y finalización.
Durante el ataque, el jugador puede moverse o realizar otra acción solo si el arma permite 
interrupciones.
Se evalúan los inputs y se decide si el jugador debe salir del estado de ataque.
También gestiona si debe voltear al personaje (flip) según el input horizontal.
Se asegura de limpiar los eventos cuando se sale del estado.
---------------------------------------------------------------------------------------------*/

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;                         // Referencia al arma utilizada en este estado
    private WeaponGenerator weaponGenerator;       // Referencia al generador de armas (instancia de ataque)

    private int inputIndex;                        // Índice del input que se está usando (primario o secundario)

    private bool canInterrupt;                     // Indica si el ataque puede ser interrumpido
    private bool checkFlip;                        // Indica si el personaje debe voltear en función del input

    // Constructor del estado de ataque
    public PlayerAttackState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName, Weapon weapon, CombatInputs input) : base(player, stateMachine, playerData, animBoolName)
    {
 
        this.weapon = weapon;
        weaponGenerator = weapon.GetComponent<WeaponGenerator>();

        inputIndex = (int)input;

        // Eventos del arma y su generador
        weapon.OnUseInput += HandleUseInput;
        weapon.EventHandler.OnEnableInterrupt += HandleEnableInterrupt;
        weapon.EventHandler.OnFinish += HandleFinish;
        weapon.EventHandler.OnFlipSetActive += HandleFlipSetActive;
    }

    // Evento que indica si se debe voltear el personaje al atacar
    private void HandleFlipSetActive(bool value)
    {
        checkFlip = value;
    }

    // Se llama cada frame en la lógica del estado
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        var playerInputHandler = player.InputHandler;

        var xInput = playerInputHandler.NormInputX;
        var attackInputs = playerInputHandler.AttackInputs;

        weapon.CurrentInput = attackInputs[inputIndex]; // Se guarda el input actual en el arma

        // Si debe voltearse, se revisa si el input lo requiere
        if (checkFlip)
        {
            Movement.CheckIfShouldFlip(xInput);
        }

        // Si no se puede interrumpir el ataque, se detiene aquí
        if (!canInterrupt)
            return;

        // Si hay input de movimiento o de ataque, se marca como terminado
        if (xInput != 0 || attackInputs[0] || attackInputs[1])
        {
            isAbilityDone = true;
        }
    }

    // Se activa cuando el arma está generando un ataque
    private void HandleWeaponGenerating()
    {
        stateMachine.ChangeState(player.IdleState); // Transición al estado Idle
    }

    // Se llama al entrar en este estado
    public override void Enter()
    {
        base.Enter();

        weaponGenerator.OnWeaponGenerating += HandleWeaponGenerating;

        checkFlip = true;
        canInterrupt = false;

        player.CancelGrapple(); // Cancelar gancho al hacer dash

        weapon.Enter(); // Inicia el ataque del arma
    }

    // Se llama al salir de este estado
    public override void Exit()
    {
        base.Exit();

        weaponGenerator.OnWeaponGenerating -= HandleWeaponGenerating;

        weapon.Exit(); // Finaliza el ataque del arma
    }

    // Método para verificar si el jugador puede entrar en este estado de ataque
    public bool CanTransitionToAttackState() => weapon.CanEnterAttack;

    // Habilita la posibilidad de interrumpir el ataque
    private void HandleEnableInterrupt() => canInterrupt = true;

    // Maneja la lógica cuando se usa el input de ataque
    //private void HandleUseInput() => player.InputHandler.UseAttackInput(inputIndex);
    private void HandleUseInput() {
        if ((core.Root.name == "LocalGamePlayer"))
        player.InputHandler.UseAttackInput(inputIndex); 
    }

    // Finaliza la animación de ataque y el estado
    private void HandleFinish()
    {
        AnimationFinishTrigger();
        isAbilityDone = true;
    }
}
