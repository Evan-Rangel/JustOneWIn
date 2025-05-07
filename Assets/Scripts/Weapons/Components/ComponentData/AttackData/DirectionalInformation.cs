using System;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
La clase DirectionalInformation define un rango angular que representa una región "defendible" 
en términos de bloqueo o parry. Esto es útil para mecánicas en las que un ataque solo puede 
bloquearse si viene desde una cierta dirección. También especifica cuánta parte del daño, 
retroceso y daño de poise se reduce o absorbe si el ataque entra dentro del rango.
El método IsAngleBetween() permite verificar si un ángulo dado (por ejemplo, la dirección 
desde donde viene el ataque) está dentro del rango definido, incluso si el rango cruza el 
eje de ±180°.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons.Components
{
    [Serializable]
    public class DirectionalInformation
    {
        // Ángulo mínimo del rango (en grados) que define la región. -180 a 180 grados.
        [Range(-180f, 180f)] public float MinAngle;

        // Ángulo máximo del rango (en grados) que define la región. -180 a 180 grados.
        [Range(-180f, 180f)] public float MaxAngle;

        // Porcentaje del daño que será absorbido (0 = nada se absorbe, 1 = se absorbe todo).
        [Range(0f, 1f)] public float DamageAbsorption;

        // Porcentaje del retroceso (knockback) que será absorbido.
        [Range(0f, 1f)] public float KnockBackAbsorption;

        // Porcentaje del daño de poise (resistencia) que será absorbido.
        [Range(0f, 1f)] public float PoiseDamageAbsorption;

        // Comprueba si un ángulo dado se encuentra dentro del rango definido por MinAngle y MaxAngle.
        public bool IsAngleBetween(float angle)
        {
            // Caso normal: el ángulo máximo es mayor que el mínimo
            if (MaxAngle > MinAngle)
            {
                return angle >= MinAngle && angle <= MaxAngle;
            }

            // Caso especial: el rango cruza el límite de -180/180 grados (por ejemplo, de 150 a -150)
            return (angle >= MinAngle && angle <= 180f) || (angle <= MaxAngle && angle >= -180f);
        }
    }
}
