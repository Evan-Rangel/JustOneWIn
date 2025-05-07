/*---------------------------------------------------------------------------------------------
Este script define un enum llamado AnimationWindows que se utiliza para etiquetar distintas 
ventanas de acción dentro de las animaciones de combate. Las animaciones pueden tener eventos 
que abren o cierran estas "ventanas", permitiendo mecánicas como bloqueo (Block) o parry (desvío)
durante momentos específicos del movimiento del personaje.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{  
    public enum AnimationWindows
    {
        Block,  
        Parry   
    }
}
