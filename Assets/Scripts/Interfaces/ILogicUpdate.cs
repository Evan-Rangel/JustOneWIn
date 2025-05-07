using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script define una interfaz (ILogicUpdate) que obliga a cualquier clase que la implemente 
a tener un método LogicUpdate(). Puedes llamar LogicUpdate() solo cuando quieras 
(solo en ciertos estados).
---------------------------------------------------------------------------------------------*/

public interface ILogicUpdate
{
    // Método que se implementa para realizar actualizaciones lógicas personalizadas
    void LogicUpdate();
}
