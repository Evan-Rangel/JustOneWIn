using System;
using Avocado.Utilities;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este componente DelayedGravity desactiva temporalmente la gravedad de un proyectil al ser 
instanciado y luego la activa automáticamente cuando el proyectil ha recorrido una cierta 
distancia en línea recta. Esto permite simular proyectiles que primero se mueven de forma 
recta (como una flecha) y luego comienzan a caer por efecto de la gravedad, ofreciendo un 
comportamiento más natural o estilizado para ciertos tipos de armas. El distanceMultiplier 
permite que otros componentes modifiquen dinámicamente la distancia requerida antes de activar 
la gravedad.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class DelayedGravity : ProjectileComponent
    {
        // Distancia que debe recorrer antes de que la gravedad empiece a actuar
        [field: SerializeField] public float Distance { get; private set; } = 10f;

        private DistanceNotifier distanceNotifier = new DistanceNotifier(); // Objeto que rastrea cuánto ha viajado el proyectil

        private float gravity; // Valor original de gravedad que se restaurará

        // Multiplicador usado por otros componentes para modificar dinámicamente la distancia requerida
        [HideInInspector]
        public float distanceMultiplier = 1;

        // Método que se llama cuando se ha recorrido la distancia definida
        private void HandleNotify()
        {
            rb.gravityScale = gravity; // Restaura la gravedad al proyectil
        }

        // Inicializa el estado del proyectil al activarse
        protected override void Init()
        {
            base.Init();

            rb.gravityScale = 0f; // Desactiva la gravedad inicialmente
            distanceNotifier.Init(transform.position, Distance * distanceMultiplier); // Comienza a rastrear distancia
            distanceMultiplier = 1; // Reinicia el multiplicador
        }

        // Inicialización general
        protected override void Awake()
        {
            base.Awake();

            gravity = rb.gravityScale; // Guarda el valor original de gravedad

            distanceNotifier.OnNotify += HandleNotify; // Se suscribe al evento de distancia recorrida
        }

        // Se llama cada frame mientras el proyectil está activo
        protected override void Update()
        {
            base.Update();

            distanceNotifier?.Tick(transform.position); // Revisa si ya recorrió la distancia definida
        }

        // Limpieza al destruirse el componente
        protected override void OnDestroy()
        {
            base.OnDestroy();

            distanceNotifier.OnNotify -= HandleNotify; // Se desuscribe del evento
        }
    }
}
