using System;
using Avocado.ObjectPoolSystem;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script Esta interfaz define lo que necesita cualquier objeto para ser gestionado por un 
Object Pool.
Detalles:
-SetObjectPool<T>(ObjectPool pool, T comp): Asigna el pool al cual pertenece el objeto y un 
Component relacionado.
-Release(): Llama para devolver el objeto al pool en lugar de destruirlo, ahorrando rendimiento.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Interfaces
{
    // Interfaz para objetos que formarán parte de un Object Pool (sistema de reutilización de objetos)
    public interface IObjectPoolItem
    {
        // Método para asignar el Object Pool y un componente asociado
        void SetObjectPool<T>(ObjectPool pool, T comp) where T : Component;

        // Método para devolver o liberar el objeto al Object Pool
        void Release();
    }
}
