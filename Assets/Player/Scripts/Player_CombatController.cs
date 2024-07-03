using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_CombatController : MonoBehaviour
{
    //Player Combat
    [Header("Combat")]
    [SerializeField]
    private bool combatEnabled;
    private bool gotInput;
    private bool isAttacking;
    private bool isFirstAttack;
    [SerializeField]
    private float inputTimer;
    private float lastInputTime = Mathf.NegativeInfinity;//Store the last time we attempted to attack
    [SerializeField]
    private float attack1Radius;
    [SerializeField]
    private float attack1Damage;
    [SerializeField]
    private float stunDamageAmount = 1f;
    [SerializeField]
    private Transform attack1HitBoxPos;//This position the hit box where we want
    [SerializeField]
    private LayerMask whatIsDamageable;
    private AttackDetails attackDetails;//This is a struct

    //Player Combat Anim
    private Animator playerC_anim;

    //Player Reference
    private Player_Controller playerC;
    private Player_Stats playerS;

    //Start
    private void Start()
    {
        //Initialice Vars
        playerC_anim.SetBool("canAttack", combatEnabled);
    }

    //Awake
    private void Awake()
    {
        //Initialice Vars
        playerC_anim = GetComponent<Animator>();
        playerC = GetComponent<Player_Controller>();
        playerS = GetComponent<Player_Stats>();
    }

    //Update
    private void Update()
    {
        //Check the player Inputs to attack
        CheckCombatInput();
        //Check the player to make the attack happen
        CheckAttacks();
    }

    //Function CheckCombatInputs
    private void CheckCombatInput()
    {
        //Condition thats know when is press de attack button
        if (Input.GetKeyDown(KeyCode.U))//Change the attack button to somenting else
        {
            //Condition thats if is activated the combat mode, then can attack
            if (combatEnabled)
            {
                //Attempt combat
                gotInput = true;
                lastInputTime = Time.time;
            }
        }
    }

    //Funtion CheckAttacks
    private void CheckAttacks ()
    {
        if (gotInput)
        {
            //Attack1
            if (!isAttacking) 
            { 
                gotInput = false;
                isAttacking = true;
                isFirstAttack = !isFirstAttack;
                playerC_anim.SetBool("attack1", true);
                playerC_anim.SetBool("firstAttack", isFirstAttack);
                playerC_anim.SetBool("isAttacking", isAttacking);
            }
        }

        if (Time.time >= lastInputTime + inputTimer)
        {
            //Wait for new Input
            gotInput = false;
        }
    }

    //Funtion CheckAttackHitBox
    private void CheckAttackHitBox()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(attack1HitBoxPos.position, attack1Radius, whatIsDamageable);//This will detected a object that can be hit in a circle range

        //AttackDetails
        attackDetails.damageAmount = attack1Damage;
        attackDetails.position = transform.position;

        //Stun
        attackDetails.stunDamageAmount = stunDamageAmount;

        //Used to loop through all the objects in our detected objects array
        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attackDetails);//Used to call to a specific funtion (like Unreal xd)
            //Instantiate hit particle
        }
    }

    //Function FinishAttack1
    private void FinishAttack1()
    {
        isAttacking = false;
        playerC_anim.SetBool("isAttacking", isAttacking);
        playerC_anim.SetBool("attack1", false);
    }

    //Function Damage (This is used in "SendMessege")
    private void Damage(AttackDetails attackDetails)
    {
        //Condition that check if the player is dashing, if is dashing then will no applay damage
        if (!playerC.GetDashStatus())
        {
            int direction; //Determinate the direction supposed to knockback the player

            //Damage player here using attackDetails.damageAmount
            playerS.DecreaseHealth(attackDetails.damageAmount);

            //Condition that check th position of the attack to applay the direction knockback
            if (attackDetails.position.x < transform.position.x)
            {
                direction = 1;
            }
            else
            {
                direction = -1;
            }

            playerC.Knockback(direction);//Call function in Player_Controller
        } 
    }
    //OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);//This is the range that is affected to do damage
    }
}
