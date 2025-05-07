using System.Collections;
using System.Collections.Generic;
using Avocado.Combat.KnockBack;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es un Interfaz para objetos que pueden recibir un efecto de KnockBack.
---------------------------------------------------------------------------------------------*/
public interface IKnockBackable
{
    // Aplica un efecto de KnockBack con los datos especificados.
    void KnockBack(KnockBackData data);
}
