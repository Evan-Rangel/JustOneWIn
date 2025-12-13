using Avocado.Combat.Damage;
using Avocado.Combat.KnockBack;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script controla el comportamiento de un proyectil físico:
-Se lanza hacia la derecha con una velocidad inicial y sin gravedad.
-Cuando recorre cierta distancia (travelDistance), activa la gravedad, lo que permite efectos como caída parabólica.
-Detecta colisiones tanto con el jugador (para aplicar daño y destruirse) como con el suelo (para detenerse).
-Puede mostrar en la escena el radio de daño con Gizmos.
-El método FireProjectile() sirve para inicializarlo desde otro objeto, como un arma o una habilidad.
---------------------------------------------------------------------------------------------*/

namespace Avocado.Projectiles
{
    public class Projectile : MonoBehaviour
    {
        private float speed;
        private float travelDistance; // Distancia máxima antes de que se active la gravedad
        private float xStartPos;      // Posición inicial en X
        private float damage;
        //KnockBackData knockbackData;
        [SerializeField] GameObject explosiveEffect;

        [SerializeField] private float gravity;         
        [SerializeField] private float damageRadius;    

        private Rigidbody2D rb;
        private Animator anim;

        private bool isGravityOn;     
        private bool hasHitGround;    

        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private Transform damagePosition; // Punto desde el cual se detecta daño
        private void Start()
        {
            TryGetComponent<Animator>(out anim);   
            rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 0.0f; // Desactiva la gravedad al principio
            rb.velocity = transform.right * speed; // Lo lanza en dirección local derecha

            isGravityOn = false;
            xStartPos = transform.position.x;
        }

        private void Update()
        {
            // Mientras no haya impactado el suelo
            if (!hasHitGround)
            {
                // Si la gravedad está activada, rota el proyectil en dirección a su movimiento
                if (isGravityOn)
                {
                    float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }

        private void FixedUpdate()
        {
            if (!hasHitGround)
            {
                // Detecta colisiones en el radio de daño con jugador o suelo
                Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
                Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

                if (damageHit)
                {
                    hasHitGround = true;


                    // Si golpea a un jugador, se destruye (aquí iría lógica de daño)
                    //damageHit.transform.SendMessage("Damage", attackDetails);

                    IDamageable damageable = damageHit.transform.root.GetComponentInChildren<IDamageable>();//GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        DamageData damageData = new DamageData(damage, null); // Ejemplo de daño
                        damageable.Damage(damageData);
                    }
                    IKnockBackable knockBackable = damageHit.transform.root.GetComponentInChildren<IKnockBackable>();
                    if (knockBackable != null)
                    {
                        knockBackable.KnockBack(new KnockBackData(Vector2.one, 10, transform.right.x >= 0 ? 1 : -1, null));
                    }

                    if (anim)
                        Invoke("DelayPlayerStop", 0.05f);
                    else
                        Destroy(gameObject);
                }

                if (groundHit)
                {
                  
                    if (anim)
                    {
                        anim.SetTrigger("Hit");
                    }
                    // Si golpea el suelo, se detiene
                    hasHitGround = true;
                    rb.gravityScale = 0f;
                    rb.velocity = Vector2.zero;
                }

                // Si ya ha recorrido la distancia establecida y aún no tiene gravedad
                if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
                {
                    isGravityOn = true;
                    rb.gravityScale = gravity;
                }
            }
        }
        void DelayPlayerStop()
        {
            explosiveEffect.SetActive(true);
            anim.SetTrigger("Hit");

            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
        public void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        // Inicializa los parámetros del proyectil desde fuera
        public void FireProjectile(float speed, float travelDistance, float damage)
        {
            this.speed = speed;
            this.travelDistance = travelDistance;
            this.damage = damage;
        }

        // Dibuja el área de daño en la escena para depuración
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
        }
    }
}
