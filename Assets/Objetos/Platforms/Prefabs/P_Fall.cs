using Mirror;
using UnityEngine;
using Steamworks;
using System.Collections;
public class P_Fall : NetworkBehaviour, ICollidable
{
    Animator m_Animator;
    Collider2D m_Collider;
    SpriteRenderer spr;
    [SerializeField] Color m_color, f_color;
    //[Server]
    private void Awake()
    {
        spr = GetComponent<SpriteRenderer>();
        m_Animator = GetComponent<Animator>();
        m_Collider = GetComponent<Collider2D>();
    }
    [Server]
    public void OnCollision(GameObject obj)
    {
        if(isServer)
        RpcNotifyClients();
    }

    [ClientRpc]
    private void RpcNotifyClients()
    {
        spr.color = m_color;
        StartCoroutine(ChangeColor());
    }
    private IEnumerator ChangeColor()
    {
        yield return Helpers.GetWait(3);
        spr.color = f_color;
    }

}
