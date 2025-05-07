using System.Collections.Generic;

/*---------------------------------------------------------------------------------------------
Este script gestionar un grupo de modificadores y aplicar su efecto acumulado a un valor.
¿Qué hace?
-Guarda una lista de modificadores específicos (TModifierType debe heredar de Modifier<TValueType>).
-Aplica todos los modificadores en orden al valor inicial.
-Permite agregar y remover modificadores dinámicamente.
¿Cómo funciona el encadenamiento?
-Empieza con un valor inicial.
-Cada modificador transforma el valor según su lógica (ModifyValue).
-El resultado de un modificador es el valor de entrada del siguiente.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ModifierSystem
{
    /*
     * La clase "Modifiers" es una clase genérica diseñada para contener y aplicar una lista de Modifiers de un tipo específico.
     * Permite aplicar fácilmente todos los modificadores a un valor.
     * Ejemplo de uso: en el componente Core "DamageReceiver", para modificar el daño recibido.
     */
    public class Modifiers<TModifierType, TValueType> where TModifierType : Modifier<TValueType>
    {
        private readonly List<TModifierType> modifierList = new List<TModifierType>();

        // Método que aplica todos los modificadores de la lista al valor inicial. Cada modificador toma el valor modificado por el anterior (aplicación en cadena).
        // Este sistema no ordena los modificadores antes de aplicarlos.
        public TValueType ApplyAllModifiers(TValueType initialValue)
        {
            var modifiedValue = initialValue;

            foreach (var modifier in modifierList)
            {
                modifiedValue = modifier.ModifyValue(modifiedValue);
            }

            return modifiedValue;
        }

        // Método para agregar un modificador a la lista
        public void AddModifier(TModifierType modifier) => modifierList.Add(modifier);

        // Método para eliminar un modificador de la lista
        public void RemoveModifier(TModifierType modifier) => modifierList.Remove(modifier);
    }
}
