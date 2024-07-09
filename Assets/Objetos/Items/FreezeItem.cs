using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FreezeItem : Item, ItemAction
{

    public void Action(PlayerItemManager _playerItemManager)
    {
        transform.position= _playerItemManager.GetItemHolderFront().position;
        RaycastHit2D[] hits;
        Transform player = _playerItemManager.transform;
        Debug.DrawRay(transform.position, Vector2.right * 100, Color.black, 100);
        hits = Physics2D.RaycastAll(transform.position, Vector2.right, 50);
        Debug.Log(hits.Length);

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.transform.CompareTag("Enemy"))
                Debug.Log(hit.transform.name);
        }
       // StartCoroutine(Freeze(_playerItemManager));
    }
    /*IEnumerator Freeze(PlayerItemManager _playerItemManager)
    {
        RaycastHit2D[] hits;
        Transform player = _playerItemManager.transform;
        Debug.DrawRay(transform.position, Vector2.right * 100, Color.black, 100);
        hits = Physics2D.RaycastAll(transform.position, Vector2.right, 50);
        Debug.Log(hits.Length);

        foreach (RaycastHit2D hit in hits)
        {
            if(hit.collider.transform.CompareTag("Enemy"))
            Debug.Log(hit.transform.name);
        }
            

    }*/
}
