using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTestDummy : MonoBehaviour, IDamageable
{
    #region References
    [SerializeField] private GameObject hitParticles;
    #endregion

    #region Integers
    #endregion

    #region Floats
    #endregion

    #region Flags
    #endregion

    #region Vectors
    #endregion

    #region Components
    private Animator anim;
    #endregion

    #region Unity CallBack Functions
    private void Awake()
    {
        //Initialize Aniamtor
        anim = GetComponent<Animator>();
    }
    #endregion

    #region Interface Functions
    public void Damage(float amount)
    {
        Debug.Log(amount + " Damage Taken");

        Instantiate(hitParticles, transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));
        anim.SetTrigger("damage");

    }
    #endregion

    #region Other Functions

    #endregion
}
