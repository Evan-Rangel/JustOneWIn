using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script Base para todos los componentes del Core. Se asegura de registrarse 
automáticamente al Core principal.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class CoreComponent : MonoBehaviour, ILogicUpdate
    {
        protected Core core;

        // Inicializa el componente buscando el Core en el objeto padre
        // y se registra en él.
        protected virtual void Awake()
        {
            // Busca el Core en el padre
            core = transform.parent.GetComponent<Core>();

            if (core == null)
            {
                Debug.LogError("There is no Core on the parent"); // Error si no encuentra el Core
            }
            core.AddComponent(this); // Se añade a la lista de componentes del Core
        }

        // Método virtual para actualizar la lógica.
        // Los hijos pueden sobreescribir este método.
        public virtual void LogicUpdate() { }
    }
}
