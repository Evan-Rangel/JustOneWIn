using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
//ATENCION!!! Script de Evan-------------------------------
public class PlayerItemManager : MonoBehaviour
{
    [SerializeField] Image itemSprite;
    [SerializeField] GameObject currentItem;
    [SerializeField] ItemAction action;
    [SerializeField] Transform itemHolderFront;
    [SerializeField] Transform itemHolderBack;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] int actives;
    public PlayerInput GetPlayerInput() { return playerInput; }
    public Transform GetItemHolderFront (){ return itemHolderFront; }
    public Transform GetItemHolderBack (){ return itemHolderBack; }
    public DistanceJoint2D distanceJoint { get; private set; }
    public void ResetCurrentItem()
    { currentItem = null; }
    private void Awake()
    {
        distanceJoint = gameObject.GetComponent<DistanceJoint2D>();
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
        for (int i = 0; i < actives; i++)
        {
            currentItem = GameManager.instance.RequestRandomItem();
            action = currentItem.gameObject.GetComponent<ItemAction>();
            itemSprite.sprite = currentItem.GetComponent<Item>().GetSprite();
            StartCoroutine(ActiveItem());
            yield return new WaitUntil(() => currentItem==null);
        }
    }
    IEnumerator ActiveItem()
    {
        yield return new WaitUntil(() => playerInput.actions["Fire"].WasPressedThisFrame());
        action.Action(this);
        action = null;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Item") && currentItem==null)
        {
            StartCoroutine(RequestRandomItem());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(itemHolderFront.position, 0.2f);
        Gizmos.DrawWireSphere(itemHolderBack.position, 0.2f);

    }
}
