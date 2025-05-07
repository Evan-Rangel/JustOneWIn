using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script calcula el ángulo entre el eje X positivo/negativo y una línea formada por dos 
Transforms (el receptor y la fuente).

receiver = el objeto desde el cual se mide
source = el objeto hacia el cual se mide
direction = 1 si estás mirando hacia la derecha, -1 si hacia la izquierda
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    public static class AngleUtilities
    {
        public static float AngleFromFacingDirection(Transform receiver, Transform source, int direction)
        {
            // Calcula el ángulo firmado (positivo o negativo) entre:
            return Vector2.SignedAngle(Vector2.right * direction, source.position - receiver.position) * direction;
        }
    }
}
