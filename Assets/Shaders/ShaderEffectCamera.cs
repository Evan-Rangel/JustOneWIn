using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShaderEffectCamera : MonoBehaviour
{
    public EffectType effectType;
    public float timeValue { private set;  get; }
    [SerializeField, Range(0, 20)] float timeMax;
    [SerializeField, Range(0, 5)] float effectSpeed;
    private void Start()
    {
        PlayerCamera_S.Instance.AddToPool(this);
        //timeMax = 1;
        timeValue= 0;
        transform.parent.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
    }
    int i = 1;
    private void Update()
    {
        switch (effectType)
        {
            case EffectType.Expansive:
                if (timeValue >= timeMax)
                {
                    //return;
                    PlayerCamera_S.Instance.RemoveFromPool(this);
                    Destroy(transform.parent.gameObject);
                }
                timeValue += Time.deltaTime;
                return;

            case EffectType.Absorption:

                timeValue += Time.deltaTime* effectSpeed*i;
                if (timeValue <= 0)
                {
                   // return;
                    PlayerCamera_S.Instance.RemoveFromPool(this);
                    Destroy(transform.parent.gameObject);
                }
                if (timeValue >= timeMax)
                {
                    i = -1;
                    effectSpeed *= 20;
                    GetComponent<Collider2D>().enabled = false;
                }
                return;
        }
    }
}
