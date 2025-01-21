using System;
using UnityEngine;
using Mirror;
using System.Collections;
[Serializable]
public class Item : NetworkBehaviour, ICollidable
{
    SpriteRenderer sr;
    Collider2D coll;
    private void Awake()
    {
        coll = GetComponent<Collider2D>();
        sr=GetComponent<SpriteRenderer>();
    }
    [Server]
    public void OnCollision()
    {
        if (isServer) RpcNotifyClients(); 
    }
    [ClientRpc]
    private void RpcNotifyClients()
    {
        StartCoroutine(Deactivate());
    }
    private IEnumerator Deactivate()
    { 
        coll.enabled = false;
        sr.enabled = false;
        yield return Helpers.GetWait( GameManager.instance.itemTimeRespawn);
        coll.enabled =true;
        sr.enabled = true;
    }
}
