using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script clase auxiliar genérica para obtener y almacenar un CoreComponent de tipo T de 
un Core.
---------------------------------------------------------------------------------------------*/

namespace Avocado.CoreSystem
{
    public class CoreComp<T> where T : CoreComponent
    {
        private Core core;

        // Referencia cacheada al componente de tipo T.
        private T comp;

        // Propiedad pública que devuelve el componente de tipo T.
        // Si no está cacheado aún, lo obtiene desde el Core.
        public T Comp => comp ? comp : core.GetCoreComponent(ref comp);

        // Constructor que asigna el Core.
        // Muestra una advertencia si el Core es nulo.
        public CoreComp(Core core)
        {
            if (core == null)
            {
                Debug.LogWarning($"Core is Null for component {typeof(T)}");
            }

            this.core = core;
        }
    }
}
