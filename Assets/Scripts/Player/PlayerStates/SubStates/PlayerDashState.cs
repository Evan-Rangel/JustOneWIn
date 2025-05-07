using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es el estado PlayerDashState permite al jugador hacer un desplazamiento rápido en 
una dirección elegida. Al entrar al estado, se ralentiza el tiempo para que el jugador apunte 
el dash con un indicador visual.
Si suelta el botón o pasa el tiempo máximo, el jugador se lanza en esa dirección.
Se crean efectos visuales (after images) mientras se mueve.
Finaliza el dash después de un tiempo, se quita la fricción, y se establece un cooldown antes 
de permitir otro dash.
---------------------------------------------------------------------------------------------*/

public class PlayerDashState : PlayerAbilityState {
	public bool CanDash { get; private set; }
	private bool isHolding;
	private bool dashInputStop;

	private float lastDashTime;

	private Vector2 dashDirection;
	private Vector2 dashDirectionInput;
	private Vector2 lastAIPos;

	public PlayerDashState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName) : base(player, stateMachine, playerData, animBoolName) 
	{
	}
	public override void Enter() {
		base.Enter();

		CanDash = false;
		player.InputHandler.UseDashInput(); // Marca el input de dash como usado

        isHolding = true;
		dashDirection = Vector2.right * Movement.FacingDirection;

		Time.timeScale = playerData.holdTimeScale; // Dirección inicial: adelante
        startTime = Time.unscaledTime;

		player.DashDirectionIndicator.gameObject.SetActive(true); // Muestra la flecha de dirección

    }

	public override void Exit() {
		base.Exit();

        // Si va hacia arriba, reduce su velocidad vertical para suavizar la transición
        if (Movement?.CurrentVelocity.y > 0) 
		{
			Movement?.SetVelocityY(Movement.CurrentVelocity.y * playerData.dashEndYMultiplier);
		}
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		if (!isExitingState) 
		{
            // Actualiza animaciones con la velocidad actual
            player.Anim.SetFloat("yVelocity", Movement.CurrentVelocity.y);
			player.Anim.SetFloat("xVelocity", Mathf.Abs(Movement.CurrentVelocity.x));


			if (isHolding) 
			{
                // Mientras mantiene presionado, obtiene la dirección de entrada del dash
                dashDirectionInput = player.InputHandler.DashDirectionInput;
				dashInputStop = player.InputHandler.DashInputStop;

                // Si se dio entrada válida, se actualiza la dirección del dash
                if (dashDirectionInput != Vector2.zero) 
				{
					dashDirection = dashDirectionInput;
					dashDirection.Normalize();
				}

                // Rota el indicador visual hacia esa dirección
                float angle = Vector2.SignedAngle(Vector2.right, dashDirection);
				player.DashDirectionIndicator.rotation = Quaternion.Euler(0f, 0f, angle - 45f);

                // Si suelta el botón o pasa el tiempo máximo de espera, se lanza el dash
                if (dashInputStop || Time.unscaledTime >= startTime + playerData.maxHoldTime) 
				{
					isHolding = false;
					Time.timeScale = 1f;
					startTime = Time.time; // Restaura el tiempo normal
                    Movement?.CheckIfShouldFlip(Mathf.RoundToInt(dashDirection.x)); // Gira si es necesario
                    player.RB.drag = playerData.drag; // Aplica fricción durante el dash
                    Movement?.SetVelocity(playerData.dashVelocity, dashDirection); // Aplica velocidad
                    player.DashDirectionIndicator.gameObject.SetActive(false); // Oculta indicador
                    PlaceAfterImage(); // Crea la primera imagen sombra
                }
			} 
			else 
			{
                // Mantiene la velocidad durante el dash
                Movement?.SetVelocity(playerData.dashVelocity, dashDirection);
				CheckIfShouldPlaceAfterImage(); // Genera más sombras si se ha desplazado lo suficiente

                // Cuando termina el tiempo del dash, termina la habilidad
                if (Time.time >= startTime + playerData.dashTime) 
				{
					player.RB.drag = 0f;
					isAbilityDone = true;
					lastDashTime = Time.time;
				}
			}
		}
	}

    // Verifica si ha recorrido suficiente distancia para generar una nueva sombra
    private void CheckIfShouldPlaceAfterImage() 
	{
		if (Vector2.Distance(player.transform.position, lastAIPos) >= playerData.distBetweenAfterImages) 
		{
			PlaceAfterImage();
		}
	}

    // Genera una imagen sombra (after image)
    private void PlaceAfterImage() 
	{
		PlayerAfterImagePool.Instance.GetFromPool();
		lastAIPos = player.transform.position;
	}

    // Permite al sistema externo verificar si puede hacer dash
    public bool CheckIfCanDash() 
	{
		return CanDash && Time.time >= lastDashTime + playerData.dashCooldown;
	}

    // Restablece la capacidad de hacer dash
    public void ResetCanDash() => CanDash = true;

}
