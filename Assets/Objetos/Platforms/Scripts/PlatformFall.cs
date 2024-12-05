using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : PlatformParent
{
    Collider2D coll;
    
    public override void Awake()
    {
        base.Awake();
        coll = GetComponent<Collider2D>();
        active = false;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")&& !coll.isTrigger &&!active) 
        {
            StartCoroutine(EnableEffector());
        }
    }
    IEnumerator EnableEffector()
    {
        active = true;
        yield return Helpers.GetWait(3);
        coll.isTrigger = !coll.isTrigger;
        yield return Helpers.GetWait(3);
        coll.isTrigger = !coll.isTrigger;
        active = false;


    }
}
