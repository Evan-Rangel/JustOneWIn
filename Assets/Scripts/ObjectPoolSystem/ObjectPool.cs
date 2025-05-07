using System.Collections.Generic;
using Avocado.Interfaces;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script reutilizar objetos (Componentes, no GameObjects completos) para optimizar el 
rendimiento evitando Instantiate y Destroy constantes.
¿Cómo trabaja?
-Guarda objetos inactivos en una Queue.
-Al pedir un objeto (GetObject), toma uno disponible o crea uno nuevo.
-Al devolver (ReturnObject), lo desactiva y lo mete de nuevo en la cola.
-Implementa una interfaz IObjectPoolItem opcional para que el objeto sepa a qué pool pertenece 
y pueda liberarse correctamente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ObjectPoolSystem
{
    // Permite liberar o devolver objetos sin conocer el tipo específico del objeto.
    // Útil para almacenar todos los pools genéricos juntos en un solo contenedor (por ejemplo, en un diccionario).
    public abstract class ObjectPool
    {
        public abstract void Release();
        public abstract void ReturnObject(Component comp);
    }

     // Nos permite crear un pool para un tipo específico de componente, en lugar de trabajar con GameObject directamente.
     // Ejemplo: Podrías hacer un pool solo para proyectiles (Projectile), en lugar de manejar todo el GameObject.
    public class ObjectPool<T> : ObjectPool where T : Component
    {
        private readonly T prefab; // Prefab usado para instanciar nuevos objetos
        private readonly Queue<T> pool = new Queue<T>(); // Objetos disponibles para usar
        private readonly List<IObjectPoolItem> allItems = new List<IObjectPoolItem>(); // Todos los objetos creados (activos o inactivos)

        // Constructor - Inicializa el pool con un número de objetos de inicio (por default, 1)
        public ObjectPool(T prefab, int startCount = 1)
        {
            this.prefab = prefab;

            for (var i = 0; i < startCount; i++)
            {
                var obj = InstantiateNewObject();
                pool.Enqueue(obj);
            }
        }

        // Instancia un nuevo objeto y lo configura si implementa IObjectPoolItem
        private T InstantiateNewObject()
        {
            var obj = Object.Instantiate(prefab);
            obj.name = prefab.name; // Opcional: Renombrar para reconocerlo fácilmente en jerarquía

            if (!obj.TryGetComponent<IObjectPoolItem>(out var objectPoolItem))
            {
                Debug.LogWarning($"{obj.name} does not have a component that implements IObjectPoolItem");
                return obj;
            }

            objectPoolItem.SetObjectPool(this, obj);
            allItems.Add(objectPoolItem);

            return obj;
        }

        // Solicita un objeto del pool
        public T GetObject()
        {
            if (!pool.TryDequeue(out var obj))
            {
                // Si no hay objetos disponibles, instancia uno nuevo
                obj = InstantiateNewObject();
                return obj;
            }

            obj.gameObject.SetActive(true);
            return obj;
        }

        // Devuelve un objeto al pool
        public override void ReturnObject(Component comp)
        {
            if (comp is not T compObj)
                return;

            compObj.gameObject.SetActive(false);
            pool.Enqueue(compObj);
        }

        // Libera todo el pool (destruye objetos en memoria)
        public override void Release()
        {
            foreach (var item in pool)
            {
                allItems.Remove(item as IObjectPoolItem);
                Object.Destroy(item.gameObject);
            }

            foreach (var item in allItems)
            {
                item.Release();
            }
        }
    }
}
