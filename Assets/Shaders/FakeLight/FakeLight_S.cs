using System.Collections;
using UnityEngine;
public class FakeLight_S : MonoBehaviour
{
    [SerializeField] SpriteRenderer spr;
    [SerializeField] float speed;
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
        spr.SetPropertyBlock(propertyBlock);
    }
    private void Update()
    {

        if (player == null) player=GameObject.Find("LocalGamePlayer");

        if (player != null) transform.position = player.transform.position;

    }
    public void StartShadowEffect()
    { 
        StartCoroutine(ActiveFakeShadow());
    }
    public IEnumerator ActiveFakeShadow()
    {
        float maxDarknessValue=75;
        propertyBlock.SetFloat("_DarknessStrength", maxDarknessValue);
        float t = Time.deltaTime * speed;
        while (propertyBlock.GetFloat("_DarknessStrength")>0)
        {
            yield return Helpers.GetWait(t);
            float decrement = t * Mathf.Lerp(1f, 50, 1f - (propertyBlock.GetFloat("_DarknessStrength") / 75));
            spr.SetPropertyBlock(propertyBlock);
            maxDarknessValue -= decrement;
            t = Time.deltaTime * speed;
            propertyBlock.SetFloat("_DarknessStrength", maxDarknessValue);
        }
        propertyBlock.SetFloat("_DarknessStrength", 0);
        spr.SetPropertyBlock(propertyBlock);
    }
}
