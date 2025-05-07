using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
WeaponAttackDetails es una estructura serializable usada para definir los parámetros específicos 
de un ataque dentro de un arma. Esto permite que un arma tenga múltiples ataques con diferentes 
nombres, velocidades de movimiento, daños y efectos de retroceso. Es ideal para juegos con 
combos, ataques cargados o distintos tipos de golpes.
---------------------------------------------------------------------------------------------*/

[System.Serializable]
public struct WeaponAttackDetails
{
    // Nombre del ataque (puede usarse para identificarlo en animaciones o lógica)
    public string attackName;

    // Velocidad de movimiento del jugador durante este ataque
    public float movementSpeed;

    // Cantidad de daño que inflige el ataque
    public float damageAmount;

    // Fuerza del retroceso que causa el ataque al enemigo
    public float knockbackStrength;

    // Dirección del retroceso en forma de vector 2D (por ejemplo: (1,1) para arriba a la derecha)
    public Vector2 knockbackAngle;
}
