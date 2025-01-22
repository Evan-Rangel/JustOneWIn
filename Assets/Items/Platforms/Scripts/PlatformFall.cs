using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : PlatformParent
{
    Animator animator;
    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Name: " + collision.transform.name);
      //  if (!isServer)return;
        Debug.Log("Enter to collision");
        if (collision.transform.CompareTag("Player")&& !animator.GetBool("Activate") ) 
        {
            ActivatePlarformFall();
        }
    }
    //[ClientRpc]

    public void DeactivatePlarformFall()
    {
        //if (!isServer) return;
        animator.SetBool("Activate", false);
       // Debug.Log("Platform Dectivated");

    }
    [ClientRpc]
    void ActivatePlarformFall()
    {
        animator.SetBool("Activate", true);
        //Debug.Log("Platform Activated");

    }
}
