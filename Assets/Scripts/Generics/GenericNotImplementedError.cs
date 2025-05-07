using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es una herramienta de utilidad para detectar cuando falta asignar correctamente
algún componente, clase o referencia genérica (T) en tu proyecto.
Funciones principales:
-Si value es distinto de null, simplemente lo retorna.
-Si es null, muestra un error en consola detallando:
-Qué tipo (typeof(T)) no está implementado.
-En qué objeto (name) ocurrió el error.
---------------------------------------------------------------------------------------------*/

public static class GenericNotImplementedError<T>
{
    // Método que verifica si un valor está asignado. Si no, lanza un error en consola.
    public static T TryGet(T value, string name)
    {
        // Si el valor no es nulo, lo devuelve normalmente
        if (value != null)
        {
            return value;
        }

        // Si el valor es nulo, muestra un error en consola indicando qué tipo y qué nombre falló
        Debug.LogError(typeof(T) + " not implemented on " + name);

        // Retorna el valor por defecto del tipo (normalmente null)
        return default;
    }
}
