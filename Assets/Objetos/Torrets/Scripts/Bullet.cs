using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 10);
    }

    public void Shoot(Vector2 _dir)
    {
        rb.velocity = _dir;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Choque");
        Destroy(gameObject);
    }
}
