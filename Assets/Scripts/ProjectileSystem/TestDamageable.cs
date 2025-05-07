using System;
using Avocado.Combat.Damage;
using Avocado.ProjectileSystem.Components;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
TestDamageable es un componente de prueba que implementa la interfaz IDamageable, permitiendo 
que cualquier objeto con este script reciba daño. Su única función es imprimir en consola el 
nombre del objeto y la cantidad de daño que ha recibido. Se usa típicamente para confirmar 
que los proyectiles están colisionando correctamente con objetivos en la escena de pruebas.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem
{
    public class TestDamageable : MonoBehaviour, IDamageable
    {
        // Método requerido por la interfaz IDamageable.
        // Se llama cuando este objeto recibe daño.
        public void Damage(DamageData data)
        {
            // Imprime en consola el nombre del objeto dañado y la cantidad de daño recibido.
            print($"{gameObject.name} Damaged: {data.Amount}");
        }
    }
}
