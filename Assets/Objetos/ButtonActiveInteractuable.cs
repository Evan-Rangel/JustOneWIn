using Avocado.Weapons.Components;
using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonActiveInteractuable : NetworkBehaviour, IDamageable
{
    public UnityEvent Activate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
    }

    void IDamageable.Damage(float amount)
    {
        Activate.Invoke();
        //Debug.Log("Colision with Button");
       // throw new System.NotImplementedException();
    }
}
