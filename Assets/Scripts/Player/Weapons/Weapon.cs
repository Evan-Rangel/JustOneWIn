using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    //---Weapon Vars---//
    #region References
    [Header("Weapon Data")]
    [SerializeField] 
    private SO_WeaponData weaponData;
    protected PlayerAttackState state;
    #endregion

    #region Values
    protected int attackCounter;
    #endregion

    #region Animators
    protected Animator baseAnimator;
    protected Animator weaponAnimator;
    #endregion

    #region Virtual Functions
    protected virtual void Start()
    {
        //Get Animators
        baseAnimator = transform.Find("Base").GetComponent<Animator>();
        weaponAnimator = transform.Find("Weapon").GetComponent<Animator>();

        //If we don't use the weapon, we don't need that in the background doing nothing, so set to inActive at Start, in "EnterWeapon" to "true" and "ExitWeapon" to "false"
        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        //Condition that check how many diferent attacks have the weapon
        if(attackCounter >= weaponData.movementSpeed.Length)
        {
            attackCounter = 0;
        }

        //Set Animations bools to "true"
        baseAnimator.SetBool("attack", true);
        weaponAnimator.SetBool("attack", true);

        //Set Animations ints to "attackCounter"
        baseAnimator.SetInteger("attackCounter", attackCounter);
        weaponAnimator.SetInteger("attackCounter", attackCounter);
    }

    public virtual void ExitWeapon()
    {
        //Set Animations to "false"
        baseAnimator.SetBool("attack", false);
        weaponAnimator.SetBool("attack", false);

        //Plus the counter to do another attack
        attackCounter++;

        gameObject.SetActive(false);
    }
    #endregion

    #region Animation Triggers
    public virtual void AnimationFinishTrigger()
    {
        state.AnimationFinishTrigger();
    }

    public virtual void AnimationStartMovementTrigger()
    {
        state.SetPlayerVelocity(weaponData.movementSpeed[attackCounter]);
    }

    public virtual void AnimationStopMovementTrigger()
    {
        state.SetPlayerVelocity(0f);
    }

    public virtual void AnimationTurnOnFlipTrigger()
    {
        state.SetFlipCheck(true);
    }

    public virtual void AnimationTurnOffFlipTrigger()
    {
        state.SetFlipCheck(false);
    }
    #endregion

    #region Other Functions
    public void InitializeWeapon(PlayerAttackState state)
    {
        this.state = state;
    }
    #endregion
}
