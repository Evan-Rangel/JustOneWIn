using System.Collections.Generic;
using Avocado.Interfaces;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script Centraliza todos los ObjectPool en un solo lugar. Permite crear, obtener y 
devolver objetos de diferentes tipos dinámicamente. Si no existe un ObjectPool para un prefab, 
lo crea automáticamente.
Funciones principales:
-GetPool<T>(T prefab, int startCount = 1) → Crea o devuelve el pool de un prefab.
-GetObject<T>(T prefab, int startCount = 1) → Obtiene un objeto del pool del prefab.
-ReturnObject<T>(T obj) → Devuelve un objeto a su pool.
-Release() → Libera todos los pools almacenados.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ObjectPoolSystem
{
    // Clase que administra múltiples pools de objetos.
    // Permite almacenar diferentes pools asociados a distintos prefabs de forma centralizada y eficiente. 
    public class ObjectPools
    {
        // Diccionario que almacena los pools, usando como clave el nombre del prefab
        private readonly Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();
      
        // Obtiene un pool existente para el prefab dado, o crea uno nuevo si no existe. startCount define cuántos objetos iniciales se instancian en el nuevo pool.
        public ObjectPool<T> GetPool<T>(T prefab, int startCount = 1) where T : Component
        {
            if (!pools.ContainsKey(prefab.name))
            {
                // Crea un nuevo pool si no existe
                pools[prefab.name] = new ObjectPool<T>(prefab, startCount);
            }

            // Retorna el pool asociado (haciendo cast a ObjectPool<T>)
            return (ObjectPool<T>)pools[prefab.name];
        }

        // Devuelve un objeto instanciado a partir del pool asociado al prefab. Si el pool no existe, se crea automáticamente.
        public T GetObject<T>(T prefab, int startCount = 1) where T : Component
        {
            return GetPool(prefab, startCount).GetObject();
        }

        
        // Retorna un objeto al pool correspondiente. Si no existe el pool del objeto, se crea sobre la marcha.
        public void ReturnObject<T>(T obj) where T : Component
        {
            var objPool = GetPool(obj);
            objPool.ReturnObject(obj);
        }

        
        // Libera todos los pools almacenados.
        public void Release()   // Llama a Release() en cada pool individualmente.
        {
            foreach (var pool in pools)
            {
                pool.Value.Release();
            }
        }
    }
}
