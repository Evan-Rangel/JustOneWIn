using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxItem :Item, ItemAction
{
    [SerializeField]BoxCollider2D coll;

    public void Action(PlayerItemManager _playerItemManager)
    {
        transform.position = _playerItemManager.GetItemHolderBack().position;
        _playerItemManager.ResetCurrentItem();
        coll.enabled = true;
        Debug.Log("Box Item");
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            collision.transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
