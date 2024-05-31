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
    private Transform attack1HitBoxPos;//This position the hit box where we want
    [SerializeField]
    private LayerMask whatIsDamageable;

    //Player Combat Anim
    private Animator playerC_anim;

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

        //Used to loop through all the objects in our detected objects array
        foreach (Collider2D collider in detectedObjects)
        {
            collider.transform.parent.SendMessage("Damage", attack1Damage);//Used to call to a specific funtion (like Unreal xd)
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

    //OnDrawGizmos
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attack1HitBoxPos.position, attack1Radius);//This is the range that is affected to do damage
    }
}
