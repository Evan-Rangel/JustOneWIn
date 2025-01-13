using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Absorption_S : MonoBehaviour
{
    [SerializeField, Range(0, 0.1f)] float maxValue;
    [SerializeField, Range(0, 1)] float speed;

    MaterialPropertyBlock propertyBlock;
    SpriteRenderer spriteRenderer;
    float absorptionStrength = 0;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    
        propertyBlock=new MaterialPropertyBlock();
    }
    void Update()
    {
        propertyBlock.SetFloat("_AbsorptionStrength", absorptionStrength);
        spriteRenderer.SetPropertyBlock(propertyBlock);
        absorptionStrength += Time.deltaTime * speed;
        if (absorptionStrength > maxValue) { absorptionStrength = 0; }
    }
}
