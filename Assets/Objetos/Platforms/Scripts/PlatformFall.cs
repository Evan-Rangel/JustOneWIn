using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFall : PlatformParent
{
    PlatformEffector2D effector;

    public override void Awake()
    {
        base.Awake();
        effector = GetComponent<PlatformEffector2D>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")&& !effector.enabled) 
        {
            StartCoroutine(EnableEffector());
        }
    }
    IEnumerator EnableEffector()
    {
        yield return Helpers.GetWait(3);
        Debug.Log(effector.enabled);
        effector.enabled = !effector.enabled;
        yield return Helpers.GetWait(3);
        Debug.Log(effector.enabled);
        effector.enabled = !effector.enabled;

    }
}
