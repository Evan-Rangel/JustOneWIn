using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
//ATENCION!!! Script de Evan-------------------------------
public class PlayerItemManager : MonoBehaviour
{
    [SerializeField] Image itemSprite;
    [SerializeField] GameObject currentItem;
    [SerializeField] ItemAction action;
    public void UseItem() 
    { 
        currentItem = null;
    }
    public void ShowItemImage()
    { 
        itemSprite.enabled = true;

    }
    IEnumerator RequestRandomItem()
    {
        itemSprite.enabled = true;
        for (int i = 0; i < 10; i++)
        {
            itemSprite.sprite=GameManager.instance.itemsSprites[Random.Range(0, GameManager.instance.itemsSprites.Length)];
            yield return Helpers.GetWait(0.3f);
        }
        currentItem= GameManager.instance.GetRandomItem();
        action=currentItem.gameObject.GetComponent<ItemAction>();
        itemSprite.sprite=currentItem.GetComponent<Item>().GetSprite();
        StartCoroutine(ActiveItem());
    }
    IEnumerator ActiveItem()
    {
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        action.Action(transform);
        action = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Item"))
        {
            Debug.Log("Item");
            StartCoroutine(RequestRandomItem());
        }
    }
}
