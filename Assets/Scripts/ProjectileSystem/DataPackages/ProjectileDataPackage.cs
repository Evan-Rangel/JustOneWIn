/*---------------------------------------------------------------------------------------------
Los datos de los componentes del proyectil no siempre están definidos en el prefab desde el principio,
sino que pueden variar dependiendo del arma que lo dispare.
Por ejemplo: muchas arcos pueden compartir el mismo prefab de flecha, pero cada uno inflige distinto daño.
Por eso, el arma necesita una forma de comunicar ese daño al proyectil al momento de generarlo.
Esta clase sirve como clase base para todos los paquetes de datos que un proyectil puede recibir.
Usando una función de la clase base `Projectile`, el arma puede enviar datos específicos que luego
cada componente del proyectil puede interpretar según le corresponda.
---------------------------------------------------------------------------------------------*/

namespace Avocado.ProjectileSystem.DataPackages
{
    public abstract class ProjectileDataPackage
    {
        // No contiene nada directamente porque actúa como una interfaz genérica para herencia.
        // Las clases hijas (como DamageDataPackage, KnockBackDataPackage, etc.) contendrán la información específica.
    }
}
