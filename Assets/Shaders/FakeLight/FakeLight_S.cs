using System.Collections;
using UnityEngine;
public class FakeLight_S : MonoBehaviour
{
    [SerializeField] SpriteRenderer spr;
    [SerializeField] float speed;
    [SerializeField, Range(0, 5)] float fadeTime;
    MaterialPropertyBlock propertyBlock;
    [SerializeField] GameObject player;
    public static FakeLight_S instance;
    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this);
        propertyBlock = new MaterialPropertyBlock();
        spr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        spr.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_DarknessStrength", 0);
        propertyBlock.SetFloat("_RespawnEffect", 1);
        spr.SetPropertyBlock(propertyBlock);
        Invoke("UnShadeEffect", 1);
        //UnShadeEffect();
    }
    private void Update()
    {

        if (player == null) player = GameObject.Find("LocalGamePlayer");

        if (player != null) transform.position = player.transform.position;
    }
    public void UnShadeEffect()
    {


        StartCoroutine(IUnShadeEffect());

    }
    IEnumerator IUnShadeEffect()
    {
        propertyBlock.SetFloat("_RespawnEffect", 1);
        spr.SetPropertyBlock(propertyBlock);
        float t = 1;
        while (t >0)
        {
            yield return Helpers.GetWait(Time.deltaTime);

            //t = Mathf.PingPong(cTime / fadeTime, 1.0f);
            t -= Time.deltaTime*2;

            //if (t > 0.99f) t = 1;

            propertyBlock.SetFloat("_RespawnEffect", t);
            spr.SetPropertyBlock(propertyBlock);

            //if (t == 1) yield return Helpers.GetWait(.3f);
        }
        propertyBlock.SetFloat("_RespawnEffect", 0);
        spr.SetPropertyBlock(propertyBlock);
    }
    public void ShadeEffect()
    { 
        StartCoroutine(IShadeEffect());
    }
    IEnumerator IShadeEffect()
    {
        //float cTime = 0;
        float t = 0;
        while (t < 1 )
        {
            yield return Helpers.GetWait(Time.deltaTime);

            //t = Mathf.PingPong(cTime / fadeTime, 1.0f);
            t += Time.deltaTime*2;

            //if (t > 0.99f) t = 1;

            propertyBlock.SetFloat("_RespawnEffect", t);
            spr.SetPropertyBlock(propertyBlock);

            //if (t == 1) yield return Helpers.GetWait(.3f);
        }
        propertyBlock.SetFloat("_RespawnEffect", 1);
        spr.SetPropertyBlock(propertyBlock);


        //propertyBlock.SetFloat("_RespawnEffect", 0);
        //spr.SetPropertyBlock(propertyBlock);
    }
    public void StartRespawnEffect()
    { 
        StartCoroutine(ActiveRespawn());
    }
    public IEnumerator ActiveRespawn()
    {
        float cTime = 0;
        float t=0;
        while (cTime<fadeTime*2)
        {
            yield return Helpers.GetWait(Time.deltaTime);
        
            t = Mathf.PingPong(cTime/ fadeTime, 1.0f);
            cTime += Time.deltaTime;

            if (t > 0.99f)  t = 1; 

            propertyBlock.SetFloat("_RespawnEffect", t); 
            spr.SetPropertyBlock(propertyBlock);
            
            if(t==1) yield return Helpers.GetWait(.3f);
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
