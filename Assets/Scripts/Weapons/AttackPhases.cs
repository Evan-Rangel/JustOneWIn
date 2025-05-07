/*---------------------------------------------------------------------------------------------
El enum AttackPhases define estados dentro del flujo de un ataque, permitiendo organizar la 
lógica del combate por fases. Esto es útil para controlar qué puede hacer el jugador o enemigo 
en cada momento del ataque (cargar, golpear, cancelar o defenderse), facilitando así 
animaciones y lógica más coherentes y modulares.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Weapons
{
    // Define las distintas fases posibles durante un ataque
    public enum AttackPhases
    {
        Anticipation, // El jugador o enemigo se prepara para atacar (por ejemplo, cargando un golpe)
        Idle,         // Fase de reposo o espera, sin realizar acciones
        Action,       // El ataque se ejecuta (por ejemplo, la espada corta o el disparo se lanza)
        Cancel,       // La animación o ataque se cancela (por ejemplo, al esquivar o interrumpir)
        Break,        // El ataque falla o es interrumpido (por ejemplo, por un bloqueo enemigo)
        Parry         // Se entra en modo de parry (contraataque o defensa precisa)
    }
}
