using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script controla el comportamiento visual de una after image del jugador.
Se activa temporalmente, copia el sprite y la posición del jugador, y luego se desvanece 
hasta ser devuelta al pool. Tras un tiempo (activeTime), se devuelve al pool 
(PlayerAfterImagePool) para reutilización.
---------------------------------------------------------------------------------------------*/

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;     // Duración visible de la after image
    private float timeActivated;                          // Tiempo en que se activó la after image
    private float alpha;                                  // Transparencia actual
    [SerializeField] private float alphaSet = 0.8f;       // Transparencia inicial
    [SerializeField] private float alphaDecay = 0.85f;    // Velocidad con la que se desvanece

    private Transform player;

    private SpriteRenderer SR;        // Renderer de esta after image
    private SpriteRenderer playerSR;  // Renderer del jugador (para copiar sprite)

    private Color color;

    // Se llama cuando el objeto se activa desde el pool
    private void OnEnable()
    {
        SR = GetComponent<SpriteRenderer>();

        // Buscar al jugador y obtener su renderer
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSR = player.GetComponent<SpriteRenderer>();

        // Inicializar valores
        alpha = alphaSet;
        SR.sprite = playerSR.sprite;            // Copiar sprite del jugador
        transform.position = player.position;   // Copiar posición
        transform.rotation = player.rotation;   // Copiar rotación
        timeActivated = Time.time;              // Guardar momento de activación
    }

    private void Update()
    {
        // Desvanecer gradualmente
        alpha -= alphaDecay * Time.deltaTime;
        color = new Color(1f, 1f, 1f, alpha);
        SR.color = color;

        // Si ya pasó el tiempo de vida, devolver al pool
        if (Time.time >= (timeActivated + activeTime))
        {
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
