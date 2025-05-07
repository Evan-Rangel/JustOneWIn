using UnityEngine;

/*---------------------------------------------------------------------------------------------
Ofrece funciones para comprobar si un layer específico está dentro de un LayerMask usando 
operaciones bitwise. También incluye una versión pensada para RaycastHit2D.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    public static class LayerMaskUtilities
    {
        // Verifica si un layer está contenido dentro de un LayerMask usando bit shifting.
        public static bool IsLayerInMask(int layer, LayerMask mask) => ((1 << layer) & mask) > 0;

        // Variante que permite verificar directamente desde un RaycastHit2D.
        public static bool IsLayerInMask(RaycastHit2D hit, LayerMask mask) => IsLayerInMask(hit.collider.gameObject.layer, mask);
    }
}
