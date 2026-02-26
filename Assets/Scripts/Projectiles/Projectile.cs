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
        string initSound;
        string hitSound;
        [SerializeField] private float gravity;         
        [SerializeField] private float damageRadius;
        [SerializeField] float knockbackForce = 1;
        [SerializeField] float rotationSpeed = 1000;
        private Rigidbody2D rb;
        private Animator anim;

        private bool isGravityOn;     
        private bool hasHitGround;    

        [SerializeField] private LayerMask whatIsGround;
        [SerializeField] private LayerMask whatIsPlayer;
        [SerializeField] private Transform damagePosition; // Punto desde el cual se detecta daño
        private void Awake()
        {
            TryGetComponent<Animator>(out anim);
            rb = GetComponent<Rigidbody2D>();

            rb.gravityScale = 0.0f; // Desactiva la gravedad al principio
            //isGravityOn = false;
            //xStartPos = transform.position.x;
        }
        private void Start()
        {
//            rb.velocity = transform.right * speed; // Lo lanza en dirección local derecha

        
        }
        private void OnEnable()
        {
            xStartPos = transform.position.x;
            hasHitGround = false;
            isGravityOn = false;
            rb.gravityScale = 0.0f;
        }
        private void OnDisable()
        {
            // Reinicia el estado del proyectil cada vez que se activa
            //rb.velocity = transform.right * speed;
        }
        float angle = 0;
        private void Update()
        {
            // Mientras no haya impactado el suelo
            if (!hasHitGround)
            {
                // Si la gravedad está activada, rota el proyectil en dirección a su movimiento
                if (isGravityOn)
                {
                    //float angle=Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                    if (rb.velocity.x < 0)
                        angle += Time.deltaTime * rotationSpeed;
                    else
                        angle -= Time.deltaTime * rotationSpeed;

                    Debug.Log("Angle: " + angle);
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

                    IDamageable damageable = damageHit.transform.root.GetComponentInChildren<IDamageable>();//GetComponent<IDamageable>();
                    if (damageable != null)
                    {
                        DamageData damageData = new DamageData(damage, gameObject); // Ejemplo de daño
                        damageable.Damage(damageData);
                    }
                    IKnockBackable knockBackable = damageHit.transform.root.GetComponentInChildren<IKnockBackable>();
                    if (knockBackable != null)
                    {
                        int direction = transform.position.x - damageHit.transform.position.x >= 0 ? -1 : 1;
                        //knockBackable.KnockBack(new KnockBackData(Vector2.one, 10*knockbackForce, transform.right.x >= 0 ? 1 : -1, null));
                        knockBackable.KnockBack(new KnockBackData(Vector2.one, 10*knockbackForce, direction, gameObject));
                    }



                    //if (anim)
                    //  Invoke("DelayPlayerStop", 0.05f);
                    // else
                    DelayPlayerStop();
                    //Destroy(gameObject);
                }

                if (groundHit)
                {
                    AudioManager.instance.PlaySFXSound(hitSound, transform);
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
               /* if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
                {

                    isGravityOn = true;
                    rb.gravityScale = gravity;
                }*/
            }
        }
        void DelayPlayerStop()
        {
            CancelInvoke("DelayPlayerStop");
            hasHitGround = true;
            anim.SetTrigger("Hit");

            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;
        }
        public void DestroyProjectile()
        {
            gameObject.SetActive(false);
        }

        // Inicializa los parámetros del proyectil desde fuera
        public void FireProjectile(float speed, float travelDistance, float damage, string _initSound,string _hitSound)
        {
            this.speed = speed;
            this.travelDistance = travelDistance;
            this.damage = damage;
            this.initSound = _initSound;
            this.hitSound = _hitSound;
            rb.velocity = transform.right * speed;
        }
         public void FireProjectileWithAngle(float speed, float travelDistance, float damage, string _initSound, string _hitSound)
        {
            this.speed = speed;
            this.travelDistance = travelDistance;
            this.damage = damage;
            this.initSound = _initSound;
            this.hitSound = _hitSound;
            rb.velocity = transform.right * speed;
        }
         public void FireProjectileWithDirection(float speed, float travelDistance, float damage, string _initSound, string _hitSound)
        {
            this.speed = speed;
            this.travelDistance = travelDistance;
            this.damage = damage;
            this.initSound = _initSound;
            this.hitSound = _hitSound;
            rb.velocity = Vector2.right * speed;
            if (gravity!=0)
            {
                rb.gravityScale = gravity;
                isGravityOn = true;
                Invoke("DelayPlayerStop", 1.5f);
            }
        }

        // Dibuja el área de daño en la escena para depuración
        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
        }
    }
}
