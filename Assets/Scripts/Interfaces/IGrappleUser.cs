using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script define una interfaz (CancelGrapple) que obliga a cualquier clase que la implemente 
a tener un m�todo a que cancele la accion del gancho.
---------------------------------------------------------------------------------------------*/

public interface IGrappleUser
{
    void ICancelGrapple();
}

