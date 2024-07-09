using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireItem : Item, ItemAction
{
    [SerializeField] GameObject bullet;
    [SerializeField] float forceSpeed;
    public void Action(PlayerItemManager _playerItemManager)
    {
        Transform originTransform = _playerItemManager.GetItemHolderFront();
        Transform player = _playerItemManager.transform;
        Vector2 direction =(originTransform.position.x< player.position.x)?Vector2.left : Vector2.right;
        GameObject _bullet = Instantiate(bullet, originTransform.position,Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().AddForce((GameManager.instance.cursor.position - originTransform.position).normalized*forceSpeed);
        Destroy( _bullet,10 );
        Destroy(gameObject);
    }
}
