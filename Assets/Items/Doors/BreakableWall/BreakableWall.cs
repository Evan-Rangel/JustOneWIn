using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BreakableWall : DoorParent//, IDamageable
{
    [SerializeField] int hitsToBreak;
    [SerializeField] Collider2D playercoll;
    Animator anim;
    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        anim.SetInteger("Damage", hitsToBreak);
        playercoll = GetComponentInChildren<Collider2D>();
    }
    /*
    void IDamageable.Damage(float amount)
    {
        hitsToBreak--;
      
        anim.SetInteger("Damage", hitsToBreak);
        
    }
    */
}
