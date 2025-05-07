using System;
using System.Collections;
using System.Collections.Generic;
using Avocado.Interfaces;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script conecta un MonoBehaviour a su ObjectPool. Permite devolverse al pool 
automáticamente, con o sin retraso (ReturnItem(float delay)). Si el objeto no está asociado a 
ningún pool, simplemente se destruye (Destroy(gameObject)).
Funciones principales:
-ReturnItem() → Devuelve al instante o con retraso.
-SetObjectPool() → Asocia este objeto con un pool específico.
-Release() → Limpia la referencia al pool.
-OnDisable() → Limpia corrutinas activas.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ObjectPoolSystem
{
    // Implementación genérica de IObjectPoolItem.
    // Permite a un objeto saber cómo regresar al pool al que pertenece o, si no pertenece a ninguno, autodestruirse.
    public class ObjectPoolItem : MonoBehaviour, IObjectPoolItem
    {
        private ObjectPool objectPool; // Referencia al ObjectPool dueño de este objeto
        private Component component;   // Componente asociado que será retornado al pool

        // Método público que permite devolver el objeto al pool. Puede hacerse de inmediato o después de un retraso opcional.
        public void ReturnItem(float delay = 0f)
        {
            if (delay > 0)
            {
                StartCoroutine(ReturnItemWithDelay(delay));
                return;
            }

            ReturnItemToPool();
        }

        // Método interno que efectivamente devuelve el objeto al pool
        private void ReturnItemToPool()
        {
            if (objectPool != null)
            {
                objectPool.ReturnObject(component);
            }
            else
            {
                // Si no pertenece a ningún pool (por error o excepción), destrúyelo
                Destroy(gameObject);
            }
        }

        // Corrutina para devolver el objeto luego de cierto tiempo
        private IEnumerator ReturnItemWithDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            ReturnItemToPool();
        }
     
        // Método requerido por IObjectPoolItem. Permite asociar este objeto con un pool y su componente principal.
        public void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component
        {
            objectPool = pool;
            component = GetComponent(comp.GetType()); // Obtener el componente basado en el tipo del parámetro
        }
       
        // Método requerido por IObjectPoolItem. Libera la referencia al pool (por ejemplo, al destruir el pool).
        public void Release()
        {
            objectPool = null;
        }

        // Por seguridad, al desactivar el objeto cancelamos cualquier corrutina activa para evitar errores o referencias innecesarias.        
        private void OnDisable()
        {
            StopAllCoroutines();
        }
    }
}
