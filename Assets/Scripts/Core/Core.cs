using System.Collections.Generic;
using System.Linq;
using Avocado.CoreSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script Core es responsable de administrar y actualizar los CoreComponents asociados 
a una entidad.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class Core : MonoBehaviour
    {
        // El GameObject que representa la raíz de esta entidad.
        // Por defecto, se asigna al GameObject padre de este Core.
        [field: SerializeField] public GameObject Root { get; private set; }

        // Lista que mantiene todos los CoreComponents registrados en este Core.
        private readonly List<CoreComponent> CoreComponents = new List<CoreComponent>();

        private void Awake()
        {
            // Si Root no está asignado manualmente, se usa el padre de este GameObject.
            Root = Root ? Root : transform.parent.gameObject;
        }

        // Llama al método LogicUpdate de cada componente registrado.
        public void LogicUpdate()
        {
            foreach (CoreComponent component in CoreComponents)
            {
                component.LogicUpdate();
            }
        }

        // Agrega un CoreComponent a la lista si aún no está agregado.
        public void AddComponent(CoreComponent component)
        {
            if (!CoreComponents.Contains(component))
            {
                CoreComponents.Add(component);
            }
        }

        // Obtiene un componente de tipo T de la lista de componentes o busca en los hijos si no se encuentra.
        public T GetCoreComponent<T>() where T : CoreComponent
        {
            // Primero busca en la lista
            var comp = CoreComponents.OfType<T>().FirstOrDefault();

            if (comp)
                return comp;

            // Si no lo encuentra, busca en los hijos
            comp = GetComponentInChildren<T>();

            if (comp)
                return comp;

            // Si tampoco se encuentra en los hijos, lanza una advertencia
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
            return null;
        }

        // Variante de GetCoreComponent que asigna el componente al valor pasado por referencia.
        public T GetCoreComponent<T>(ref T value) where T : CoreComponent
        {
            value = GetCoreComponent<T>();
            return value;
        }
    }
}
