using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformParent : Interactuable
{
    [SerializeField] Transform[] targets;
    [SerializeField] float speed;
    [SerializeField] float restTime;
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        transform.position = targets[0].position;
        StartCoroutine(MovePlatform(0));
    }

    IEnumerator MovePlatform(int _target)
    {
        rb.velocity = ( targets[_target].position- transform.position).normalized*speed;
        yield return new WaitUntil(() => Vector2.Distance(transform.position, targets[_target].position) < 1);
        yield return Helpers.GetWait(restTime);
        StartCoroutine(MovePlatform((_target < targets.Length-1) ? _target+1 : 0));
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < targets.Length-1; i++)
        {
            Gizmos.DrawLine(targets[i].position, targets[i + 1].position);
        }
        Gizmos.DrawLine(targets[0].position, targets[targets.Length-1].position);

    }
}
