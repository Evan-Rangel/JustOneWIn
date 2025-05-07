using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script es una herramienta de visualización para BoxCast en 2D. Te permite:
-Dibujar con líneas (Debug.DrawLine) la caja de colisión en la escena.
-Usar BoxCastAll y al mismo tiempo ver la caja lanzada en el Scene View para debug.
-Ideal para detectar colisiones con plataformas, paredes, enemigos, etc., y ver visualmente 
el área que estás escaneando.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Utilities
{
    public class BoxCastUtilities
    {
        // Dibuja una caja y su desplazamiento en el espacio, útil para visualizar BoxCasts.
        public static void Draw(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity)
        {          
            Vector2[] originalBox = CreateOriginalBox(origin, size, angle); // Calcula la caja original           
            Vector2 distanceVector = GetDistanceVector(distance, direction); // Calcula cuánto se moverá        
            Vector2[] shiftedBox = CreateShiftedBox(originalBox, distanceVector); // Crea una copia desplazada de la caja original

            // Dibuja ambas cajas y las conecta visualmente
            DrawBox(originalBox, Color.red);
            DrawBox(shiftedBox, Color.red);
            ConnectBoxes(originalBox, shiftedBox, Color.gray);
        }

        // Realiza un BoxCast y lo dibuja en pantalla.
        public static RaycastHit2D[] BoxCastAndDraw(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers, float minDepth = -Mathf.Infinity, float maxDepth = Mathf.Infinity)
        {        
            var hitInfo = Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth); // Lanza el BoxCast
            Draw(origin, size, angle, direction, distance); // Visualiza el BoxCast
            return hitInfo;
        }

        // Crea una caja rotada y posicionada para visualización
        private static Vector2[] CreateOriginalBox(Vector2 origin, Vector2 size, float angle)
        {
            float w = size.x * 0.5f;
            float h = size.y * 0.5f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            var box = new Vector2[4]
            {
                new Vector2(-w, h),
                new Vector2(w, h),
                new Vector2(w, -h),
                new Vector2(-w, -h),
            };

            // Aplica la rotación y traslada los puntos al origen
            for (int i = 0; i < 4; i++)
            {
                box[i] = (Vector2)(q * box[i]) + origin;
            }

            return box;
        }

        // Desplaza la caja en la dirección del cast
        private static Vector2[] CreateShiftedBox(Vector2[] box, Vector2 distance)
        {
            var shiftedBox = new Vector2[4];
            for (int i = 0; i < 4; i++)
            {
                shiftedBox[i] = box[i] + distance;
            }

            return shiftedBox;
        }

        // Dibuja la caja con líneas conectando sus 4 esquinas
        private static void DrawBox(Vector2[] box, Color color)
        {
            Debug.DrawLine(box[0], box[1], color);
            Debug.DrawLine(box[1], box[2], color);
            Debug.DrawLine(box[2], box[3], color);
            Debug.DrawLine(box[3], box[0], color);
        }

        // Conecta la caja original con la desplazada para mostrar el "volumen" del cast
        private static void ConnectBoxes(Vector2[] firstBox, Vector2[] secondBox, Color color)
        {
            Debug.DrawLine(firstBox[0], secondBox[0], color);
            Debug.DrawLine(firstBox[1], secondBox[1], color);
            Debug.DrawLine(firstBox[2], secondBox[2], color);
            Debug.DrawLine(firstBox[3], secondBox[3], color);
        }

        // Calcula cuánto se debe mover la caja. Si es infinito, calcula una distancia basada en el tamaño de la cámara
        private static Vector2 GetDistanceVector(float distance, Vector2 direction)
        {
            if (distance == Mathf.Infinity)
            {
                float sceneWidth = Camera.main.orthographicSize * Camera.main.aspect * 2f;
                distance = sceneWidth * 5f; // Escala grande para que se vea en el editor
            }

            return direction.normalized * distance;
        }
    }
}
