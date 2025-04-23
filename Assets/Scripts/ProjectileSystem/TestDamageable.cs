using System.Collections;
using System.Collections.Generic;
using Avocado.ProjectileSystem.Components;
using Avocado.Combat.Damage;
using UnityEngine;

namespace Avocado.ProjectileSystem
{
    /*
     * This MonoBehaviour is simply used to print the damage amount received in the ProjectileTestScene
     */
    public class TestDamageable : MonoBehaviour, IDamageable
    {
        public void Damage(DamageData data)
        {
            print($"{gameObject.name} Damaged: {data.Amount}");
        }
    }
}
