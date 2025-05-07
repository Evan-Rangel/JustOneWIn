using System;
using System.Collections;
using Avocado.ProjectileSystem.DataPackages;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
ProjectileComponent es una clase base para cualquier script que sea parte de un sistema de 
proyectiles. Centraliza funcionalidades comunes como suscripción a eventos (OnInit, OnReset, 
OnReceiveDataPackage), el control del estado activo del componente (Active), y proporciona 
puntos de extensión (Init, ResetProjectile, HandleReceiveDataPackage) que otros componentes 
pueden sobrescribir. Esto permite que todos los componentes (como daño, movimiento, efectos, 
etc.) trabajen de forma modular y reactiva según los eventos del proyectil.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.Components
{
    public class ProjectileComponent : MonoBehaviour
    {
        // Referencia al objeto principal del proyectil
        protected Projectile projectile;

        // Acceso rápido al Rigidbody2D del proyectil
        protected Rigidbody2D rb => projectile.Rigidbody2D;

        // Estado activo del componente (útil para deshabilitar temporalmente lógica sin desactivar el GameObject)
        public bool Active { get; private set; }

        // Se llama cuando el proyectil se lanza por primera vez
        protected virtual void Init()
        {
            SetActive(true);
        }

        // Se llama cuando el proyectil se reinicia. Se puede sobrescribir en componentes que lo necesiten
        protected virtual void ResetProjectile()
        {
        }

        // Se sobrescribe para recibir paquetes de datos específicos (como daño, knockback, etc.)
        protected virtual void HandleReceiveDataPackage(ProjectileDataPackage dataPackage)
        {
        }

        // Activa o desactiva el componente manualmente
        public virtual void SetActive(bool value) => Active = value;

        // Activa o desactiva el componente en el siguiente frame (útil para esperar una física o colisión)
        public virtual void SetActiveNextFrame(bool value)
        {
            StartCoroutine(SetActiveNextFrameCoroutine(value));
        }

        // Corrutina para esperar un frame antes de aplicar el cambio de estado
        public IEnumerator SetActiveNextFrameCoroutine(bool value)
        {
            yield return null;
            SetActive(value);
        }

        // Suscripción automática a eventos del proyectil cuando el componente se instancia
        protected virtual void Awake()
        {
            projectile = GetComponent<Projectile>();

            projectile.OnInit += Init;
            projectile.OnReset += ResetProjectile;
            projectile.OnReceiveDataPackage += HandleReceiveDataPackage;
        }

        // Métodos virtuales vacíos para permitir personalización sin romper herencia
        protected virtual void Start() { }
        protected virtual void Update() { }
        protected virtual void FixedUpdate() { }

        // Limpieza de eventos para evitar fugas de memoria o errores
        protected virtual void OnDestroy()
        {
            projectile.OnInit -= Init;
            projectile.OnReset -= ResetProjectile;
            projectile.OnReceiveDataPackage -= HandleReceiveDataPackage;
        }
    }
}
