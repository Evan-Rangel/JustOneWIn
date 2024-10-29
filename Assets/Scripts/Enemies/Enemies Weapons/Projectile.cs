using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    
    //Attack Details Reference
    //private AttackDetails attackDetails;

    //Arow Stats
    private float speed;
    private float travelDistance;
    private float xStartPos;
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float damageRadius;

    //Arrows Flags
    private bool isGravityOn;
    private bool hasHitGround;

    //Arrows Layers
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private LayerMask whatIsPlayer;

    //Arrow Components
    private Rigidbody2D rb;

    //Arrow Transform
    [SerializeField]
    private Transform damagePosition;

    //Start
    private void Start()
    {
        //Initialize
        rb.gravityScale = 0.0f;
        rb.velocity = transform.right * speed;

        isGravityOn = false;

        xStartPos = transform.position.x;
    }

    //Awake
    private void Awake()
    {
        //Initialize
        rb = GetComponent<Rigidbody2D>();
    }

    //Update
    private void Update()
    {
        //Condition that check if hit the ground
        if(!hasHitGround)
        {
            //attackDetails.position = transform.position;

            if(isGravityOn)
            {
                float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
    }

    //FixedUpdate
    private void FixedUpdate()
    {
        //Condition that check  if the arrow still in the air
        if(!hasHitGround)
        {
            //Hit Player
            Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
            //Hit Ground
            Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);

            //Condition that check if hit the player
            if(damageHit)
            {
                //Damage Player
                //damageHit.transform.SendMessage("Damage", attackDetails);
                Destroy(gameObject);
            }

            //Condition that check if hit the ground
            if(groundHit)
            {
                hasHitGround = true;
                rb.gravityScale = 0f;
                rb.velocity = Vector2.zero;
            }

            //Condition that check in time the distance of the arrow to turn on the gravity
            if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
            {
                isGravityOn = true;
                rb.gravityScale = gravity;
            }
        }
    }

    //Function FireProjectil
    public void FireProjectil(float speed, float travelDistance, float damage)
    {
        this.speed = speed;
        this.travelDistance = travelDistance;
        //attackDetails.damageAmount = damage;
    }

    //Function OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    }
}
