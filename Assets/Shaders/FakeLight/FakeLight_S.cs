using System.Collections;
using UnityEngine;
public class FakeLight_S : MonoBehaviour
{
    [SerializeField] SpriteRenderer spr;
    [SerializeField] float speed;
    [SerializeField, Range(0, 5)] float fadeTime;
    MaterialPropertyBlock propertyBlock;
    GameObject player;
    public static FakeLight_S instance;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        propertyBlock = new MaterialPropertyBlock();
        spr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        spr.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_DarknessStrength", 0);
        propertyBlock.SetFloat("_RespawnEffect", 0);
        spr.SetPropertyBlock(propertyBlock);
    }
    private void Update()
    {

        if (player == null) player = GameObject.Find("LocalGamePlayer");

        if (player != null) transform.position = player.transform.position;

    }
    public void StartRespawnEffect()
    { 
        StartCoroutine(ActiveRespawn());
    }
    public IEnumerator ActiveRespawn()
    {
        float cTime = 0;
        float t=0;
        while (cTime<1)
        {
            yield return Helpers.GetWait(Time.deltaTime );
            t = Mathf.PingPong(cTime/ fadeTime, 1.0f);
            cTime += Time.deltaTime;
            propertyBlock.SetFloat("_RespawnEffect", t); 
            spr.SetPropertyBlock(propertyBlock);
            if (t>0.96f)
            {
                propertyBlock.SetFloat("_RespawnEffect", 1); 
                spr.SetPropertyBlock(propertyBlock);
                yield return Helpers.GetWait(0.15f);
            }
        }
        propertyBlock.SetFloat("_RespawnEffect", 0);
        spr.SetPropertyBlock(propertyBlock);
    }
    public void StartBlindEffect()
    { 
        StartCoroutine(ActiveFakeShadow());
    }
    public IEnumerator ActiveFakeShadow()
    {
        float maxDarknessValue=75;
        propertyBlock.SetFloat("_DarknessStrength", maxDarknessValue);
        float t = Time.deltaTime;
        while (propertyBlock.GetFloat("_DarknessStrength")>0)
        {
            yield return Helpers.GetWait(t);
            float decrement = t * Mathf.Lerp(1f, 50, 1f - (propertyBlock.GetFloat("_DarknessStrength") / 75));
            spr.SetPropertyBlock(propertyBlock);
            maxDarknessValue -= decrement;
            t = Time.deltaTime ;
            propertyBlock.SetFloat("_DarknessStrength", maxDarknessValue);
        }
        propertyBlock.SetFloat("_DarknessStrength", 0);
        spr.SetPropertyBlock(propertyBlock);
    }
}
