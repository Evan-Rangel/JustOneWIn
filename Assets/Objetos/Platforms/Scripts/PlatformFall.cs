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
        if (!isServer)return;
        if (collision.transform.CompareTag("Player")&& !animator.GetBool("Activate") ) 
        {
            EnableEffector();
        }
    }
    [ClientRpc]
    public void DeactivatePlarformFall()
    {
        //if (!isServer) return;
        animator.SetBool("Activate", false);
        Debug.Log("Platform Dectivated");

    }
    [ClientRpc]
    void EnableEffector()
    {
        animator.SetBool("Activate", true);
        Debug.Log("Platform Activated");

    }
}
