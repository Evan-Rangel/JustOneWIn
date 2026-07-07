using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FakeLight_S : MonoBehaviour
{
    [SerializeField] SpriteRenderer spr;
    [SerializeField] float speed;
    [SerializeField, Range(0, 5)] float fadeTime;
    MaterialPropertyBlock propertyBlock;
    [SerializeField] GameObject player;
    public static FakeLight_S instance;
    IEnumerator unShadeEffect;
    IEnumerator shadeEffect;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += SceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneLoaded;
    }
    void SceneLoaded(Scene scene, LoadSceneMode mode)
    {

        spr.GetPropertyBlock(propertyBlock);
        propertyBlock.SetFloat("_DarknessStrength", 0);
        propertyBlock.SetFloat("_RespawnEffect", 1);
        spr.SetPropertyBlock(propertyBlock);
        Invoke(nameof(UnShadeEffect), 1.5f);
    }
    private void Awake()
    {
        instance = this;
        propertyBlock = new MaterialPropertyBlock();
        spr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {

        if (player == null) player = GameObject.Find("LocalGamePlayer");

        if (player != null) transform.position = player.transform.position;
    }
    public void UnShadeEffect()
    {
        if (unShadeEffect != null)
        {
            StopCoroutine(unShadeEffect);
        }
        if (shadeEffect != null)
            StopCoroutine(shadeEffect);

        unShadeEffect = IUnShadeEffect();
        StartCoroutine(unShadeEffect);
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
    event Action shaderEffectSuccess;
    public void ShadeEffect(Action _shaderEffectSuccess)
    {
       // Debug.Log("StartShade");
        shaderEffectSuccess = _shaderEffectSuccess;
        if (shadeEffect != null)
            StopCoroutine(shadeEffect);

        if (unShadeEffect!=null)
            StopCoroutine(unShadeEffect);

        shadeEffect = IShadeEffect();
        StartCoroutine(shadeEffect);

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
        yield return Helpers.GetWait(0.05f);
        shaderEffectSuccess?.Invoke();
        shaderEffectSuccess = null;
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
