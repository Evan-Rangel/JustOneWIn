using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TorretParent : Interactuable
{
    [Header("Torret Variables")]
    [SerializeField] Vector2 shootDirection;
    [SerializeField] float shootSpeed;
    [SerializeField] float shootRate;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform shootTarget;
    IEnumerator shootCoroutine;
    public override void Awake()
    {
        base.Awake();
        shootCoroutine = StartShooting();
    }
    public override void Activate()
    {
        base.Activate();
        StartCoroutine(shootCoroutine);
    }
    public override void Deactivate()
    {
        base.Deactivate();
        StopCoroutine(shootCoroutine);
    }
    IEnumerator StartShooting()
    {
        while (active)
        {
            yield return Helpers.GetWait(shootRate);
            if (shootTarget!=null)
            {
                shootDirection = (transform.position - shootTarget.position).normalized;
            }
            GameObject bullet =Instantiate(bulletPrefab, transform.position, Quaternion.identity, null);
            bullet.GetComponent<Bullet>().Shoot(shootDirection*shootSpeed);
        }
    }
}
