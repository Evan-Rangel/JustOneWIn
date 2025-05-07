using UnityEngine;

/*---------------------------------------------------------------------------------------------
Convierte un Vector2 en una rotación que puedes aplicar a un objeto para que su eje right 
apunte hacia ese vector.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    public static class QuaternionExtensions
    {
        // Convierte un Vector2 de dirección en una rotación (Quaternion). Hace que el eje transform.right apunte en la dirección del vector.
        public static Quaternion Vector2ToRotation(Vector2 direction)
        {
            // Calcula el ángulo entre el vector y el eje x positivo
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Crea una rotación alrededor del eje z (modo 2D)
            var rotation = Quaternion.Euler(0f, 0f, angle);

            return rotation;
        }
    }
}