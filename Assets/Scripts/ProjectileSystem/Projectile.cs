using System;
using Avocado.ProjectileSystem.DataPackages;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script representa un sistema de proyectil modular en el que se pueden conectar distintos 
componentes de comportamiento (como daños, movimiento, rotación, efectos visuales). Sus 
principales responsabilidades son:
-Inicializar el proyectil con Init(), notificando a sus componentes.
-Reiniciarlo con Reset() (por ejemplo, para reusar con object pooling).
-Enviar paquetes de datos antes de ser activado, usando SendDataPackage().
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem
{
    public class Projectile : MonoBehaviour
    {
        // Evento que notifica a todos los componentes del proyectil que se ha llamado a Init
        public event Action OnInit;

        // Evento que notifica que el proyectil se ha reiniciado
        public event Action OnReset;

        // Evento que permite enviar información adicional (daño, rango, etc.) desde el arma al proyectil
        public event Action<ProjectileDataPackage> OnReceiveDataPackage;

        // Referencia al Rigidbody2D del proyectil
        public Rigidbody2D Rigidbody2D { get; private set; }

        // Inicializa el proyectil y notifica a todos los componentes suscritos
        public void Init()
        {
            OnInit?.Invoke();
        }

        // Resetea el proyectil (por ejemplo, al ser reciclado)
        public void Reset()
        {
            OnReset?.Invoke();
        }

        // Este método se llama antes de Init, desde el arma.
        // Permite a cualquier componente del arma enviar datos al proyectil, como: daño, velocidad, comportamiento especial, etc.
        public void SendDataPackage(ProjectileDataPackage dataPackage)
        {
            OnReceiveDataPackage?.Invoke(dataPackage);
        }

        #region Plumbing

        // Se ejecuta al instanciar el GameObject. Aquí se obtiene la referencia al Rigidbody2D
        private void Awake()
        {
            Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        #endregion
    }
}
