using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[Serializable]
public class Item : MonoBehaviour
{
    [SerializeField] Sprite sprite;
    public Sprite GetSprite (){ return sprite; }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")) GameManager.instance.StartCoroutine(GameManager.instance.ReEnableItem(gameObject));
    }
}
