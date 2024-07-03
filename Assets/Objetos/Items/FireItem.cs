using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireItem : Item, ItemAction
{
    [SerializeField] GameObject bullet;
    [SerializeField] float forceSpeed;
    public void Action(Transform _player)
    {
        GameObject _bullet = Instantiate(bullet, _player.position,Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().velocity=(GameManager.instance.cursor.position - _player.position)*forceSpeed;
        Debug.Log("Fire Item");
    }
}
