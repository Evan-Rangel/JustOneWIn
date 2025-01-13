using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExpansiveExplosion_S : ShaderEffectCamera
{
    MaterialPropertyBlock propertyBlock;
    //CircleCollider2D collider2D;
    SpriteRenderer spr;
   // [SerializeField, Range(0.01f, 5)]float speed=2.5f;
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        propertyBlock = new MaterialPropertyBlock();
       // collider2D = GetComponent<CircleCollider2D>();
    }
    /*
    void Update()
    {


        timeValue+=Time.deltaTime*speed;
        //collider2D.radius=(timeValue/2);
        propertyBlock.SetFloat("_TimeValue", timeValue);
        spr.SetPropertyBlock(propertyBlock);
        if (timeValue>=1)
        {
            Destroy(transform.parent.gameObject);
        }
    }*/
}
