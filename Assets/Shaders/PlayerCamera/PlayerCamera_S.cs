using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[Serializable]
public enum EffectType
{ 
    Expansive,
    Absorption,
    Distortion
}

public class PlayerCamera_S : MonoBehaviour
{
    GameObject player;
    MaterialPropertyBlock propertyBlock;
    SpriteRenderer spr;
    [SerializeField]List <ShaderEffectCamera> objectsToTrack;
    int textureWidth = 10;
    Camera mainCamera;
    private Texture2D positionsTexture;
     static PlayerCamera_S instance;
    public static PlayerCamera_S Instance { get { return instance; } }
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        spr = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        positionsTexture = new Texture2D(textureWidth, 3, TextureFormat.RGBAFloat, false);
        for (int i = 0; i < positionsTexture.width; i++)
        {
            for (int j = 0; j < positionsTexture.height; j++)
            { 
            positionsTexture.SetPixel(i, j, new Color(0, 0, 0, 0));

            }
        }
        propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetTexture("_PositionsTexture", positionsTexture);
        spr.SetPropertyBlock(propertyBlock);
        objectsToTrack = new List<ShaderEffectCamera>(30);
        spr.enabled=false;
    }
    public void AddToPool(ShaderEffectCamera obj)
    {
        spr.enabled = true;

        for (int i = 0; i < objectsToTrack.Count; i++)
        {
            if (objectsToTrack[i] == null)
            {
                objectsToTrack[i] = obj;
                return;
            }
        }
        objectsToTrack.Add(obj);
    }
    public void RemoveFromPool(ShaderEffectCamera obj)
    {
        for (int i = 0; i < objectsToTrack.Count; i++)
        {
            if (objectsToTrack[i]!=obj)continue;
            
            positionsTexture.SetPixel(i, (int)objectsToTrack[i].effectType, new Color(0, 0, 0, 0));
            objectsToTrack[i] = null;
            break;
        }
        positionsTexture.Apply();
        propertyBlock.SetTexture("_PositionsTexture", positionsTexture);
        spr.SetPropertyBlock(propertyBlock);
        for (int i = 0; i < objectsToTrack.Count; i++)
        {
            if (objectsToTrack[i] != null)
                return;
        }
        spr.enabled = false;
    }
    private void Update()
    {
        if (player != null)
            transform.position = player.transform.position;
        if (player == null)
        { 
            player =GameObject.Find("LocalGamePlayer");
            if (player!=null)
            mainCamera= player.GetComponent<PCameraController>().mainCamera;
        }
        for (int i = 0; i < objectsToTrack.Count; i++)
        {
            if (objectsToTrack[i] == null)
                continue;
            Vector3 viewportPos = mainCamera.WorldToViewportPoint(objectsToTrack[i].transform.position);
            positionsTexture.SetPixel(i, (int)objectsToTrack[i].effectType, new Color(viewportPos.x, viewportPos.y, objectsToTrack[i].timeValue, 1));
        }
        positionsTexture.Apply();
        propertyBlock.SetTexture("_PositionsTexture", positionsTexture);
        spr.SetPropertyBlock(propertyBlock);
    }
}
