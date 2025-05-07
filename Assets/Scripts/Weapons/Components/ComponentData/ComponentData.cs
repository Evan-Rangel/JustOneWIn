using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script define la estructura de datos para componentes de armas. Es parte clave del sistema modular de combate:
ComponentData: clase base no genérica, contiene el tipo de componente al que pertenece (ComponentDependency) y lógica general.
ComponentData<T>: clase base genérica usada para componentes con diferentes tipos de AttackData (por ejemplo, proyectiles, bloqueos, cargas...).
Soporta configuración de datos por ataque o repetición global con repeatData.
Usa reflexión (Activator.CreateInstance) para instanciar nuevos datos dinámicamente en el editor o al inicializar.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public abstract class ComponentData
    {
        // Nombre del componente (oculto en el inspector, probablemente usado internamente o en el editor).
        [SerializeField, HideInInspector] private string name;

        // Tipo del componente al que esta data pertenece (por ejemplo: typeof(Block), typeof(Charge), etc.).
        public Type ComponentDependency { get; protected set; }

        // Constructor: establece el nombre y la dependencia del componente.
        public ComponentData()
        {
            SetComponentName();
            SetComponentDependency();
        }

        // Asigna el nombre de la clase como nombre del componente.
        public void SetComponentName() => name = GetType().Name;

        // Método abstracto que debe implementar cada subclase para definir su dependencia específica.
        protected abstract void SetComponentDependency();

        // Virtual: usado para nombrar internamente los datos de ataque (puede sobrescribirse si se necesita).
        public virtual void SetAttackDataNames() { }

        // Virtual: permite inicializar estructuras de datos internas según el número de ataques del arma.
        public virtual void InitializeAttackData(int numberOfAttacks) { }
    }

    // Clase base genérica para datos de componentes que dependen de un tipo específico de AttackData.
    [Serializable]
    public abstract class ComponentData<T> : ComponentData where T : AttackData
    {
        // Si es verdadero, se repite la misma data para todos los ataques (para evitar duplicación de datos).
        [SerializeField] private bool repeatData;

        // Arreglo de datos de ataque específicos (uno por ataque o uno compartido si repeatData es true).
        [SerializeField] private T[] attackData;

        // Obtiene los datos del ataque actual (usa el primero si repeatData está activo).
        public T GetAttackData(int index) => attackData[repeatData ? 0 : index];

        // Retorna todos los datos de ataque.
        public T[] GetAllAttackData() => attackData;

        // Asigna nombres a cada objeto de ataque para identificación (como "Attack 1", "Attack 2", etc.).
        public override void SetAttackDataNames()
        {
            base.SetAttackDataNames();

            for (var i = 0; i < attackData.Length; i++)
            {
                attackData[i].SetAttackName(i + 1);
            }
        }

        // Inicializa el arreglo de datos de ataque según el número de ataques. Crea instancias si faltan.
        public override void InitializeAttackData(int numberOfAttacks)
        {
            base.InitializeAttackData(numberOfAttacks);

            // Determina cuántos elementos debe tener el arreglo.
            var newLen = repeatData ? 1 : numberOfAttacks;

            var oldLen = attackData != null ? attackData.Length : 0;

            // Si el tamaño es el mismo, no hace nada.
            if (oldLen == newLen)
                return;

            // Cambia el tamaño del arreglo.
            Array.Resize(ref attackData, newLen);

            // Si el arreglo era más corto, crea instancias nuevas para los elementos faltantes.
            if (oldLen < newLen)
            {
                for (var i = oldLen; i < attackData.Length; i++)
                {
                    var newObj = Activator.CreateInstance(typeof(T)) as T;
                    attackData[i] = newObj;
                }
            }

            // Asigna nombres a los ataques.
            SetAttackDataNames();
        }
    }
}
