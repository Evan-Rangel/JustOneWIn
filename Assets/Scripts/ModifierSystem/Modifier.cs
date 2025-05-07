/*---------------------------------------------------------------------------------------------
Este script crear una estructura flexible para aplicar modificadores a cualquier tipo de dato 
(por ejemplo: daño, defensa, velocidad, energía, etc.).
¿Qué hace?
-Modifier: Clase base no genérica. Sirve para almacenar todos los modificadores en una lista 
común.
-Modifier<T>: Clase base genérica. Permite crear modificadores que saben cómo cambiar un valor 
de tipo T.
-El método ModifyValue(T value) debe ser implementado en clases derivadas para definir 
exactamente cómo cambiar el valor.
¿Cómo se usa en práctica?
-Crear una clase que herede de Modifier<T>.
-Implementar el método ModifyValue.
-Aplicar varios modificadores en cadena (por ejemplo, aumentar daño un 10%, luego sumarle +5).
---------------------------------------------------------------------------------------------*/

namespace Avocado.ModifierSystem
{
    /*
     * Clase base abstracta "Modifier" permite almacenar modificadores en una lista de manera genérica.
     * Esto facilita iterar sobre todos los modificadores, sin importar el tipo específico.
     */
    public abstract class Modifier
    {
        // Clase vacía: solo sirve como tipo base común
    }

    /*
     * Clase base abstracta genérica "Modifier<T>".
     * Permite especificar qué tipo de valor será modificado (por ejemplo, daño, velocidad, resistencia, etc.).
     * La mayoría de los modificadores heredan de esta clase o de una subclase que ya define el tipo "T".
     */
    public abstract class Modifier<T> : Modifier
    {
        // Método abstracto que debe ser implementado en clases hijas.
        // Define cómo modificar un valor del tipo T.
        public abstract T ModifyValue(T value);
    }
}
