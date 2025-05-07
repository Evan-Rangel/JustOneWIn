using UnityEngine;

/*---------------------------------------------------------------------------------------------
OptionalSpriteMarker es un script de utilidad que actúa como etiqueta para identificar un 
GameObject específico, que será usado como un sprite opcional para el arma (por ejemplo, una 
aura, destello o efecto). Esto permite que animaciones o scripts puedan encontrar fácilmente 
este objeto y cambiar su visibilidad, color o cualquier otro aspecto visual durante el ataque 
o acciones del arma.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    // Este MonoBehaviour vacío se usa únicamente como un marcador o identificador.
    // Debe colocarse como hijo del GameObject principal del arma.
    // El objetivo es que este objeto pueda ser animado por las animaciones del arma base.
    public class OptionalSpriteMarker : MonoBehaviour
    {
        // Propiedad que devuelve el componente SpriteRenderer del GameObject en el que está este script
        public SpriteRenderer SpriteRenderer => gameObject.GetComponent<SpriteRenderer>();
    }
}
