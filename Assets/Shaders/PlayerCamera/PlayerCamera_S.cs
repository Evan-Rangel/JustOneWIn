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



    private Texture2D playersPositionsTexture;
    [SerializeField] List<GameObject> players;
    
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
        //Items Texture Positions
        positionsTexture = new Texture2D(textureWidth, 3, TextureFormat.RGBAFloat, false);
        for (int i = 0; i < positionsTexture.width; i++)
        {
            for (int j = 0; j < positionsTexture.height; j++)
            { 
            positionsTexture.SetPixel(i, j, new Color(0, 0, 0, 0));

            }
        }

        //Players Texture Positions
        playersPositionsTexture = new Texture2D(16, 1, TextureFormat.RGBAFloat, false);
        for (int i = 0; i < playersPositionsTexture.width; i++)
        {
            for (int j = 0; j < playersPositionsTexture.height; j++)
            {
                playersPositionsTexture.SetPixel(i, j, new Color(0,0,0,0));
            }
        }


        propertyBlock = new MaterialPropertyBlock();
        propertyBlock.SetTexture("_PositionsTexture", positionsTexture);
        propertyBlock.SetTexture("_PlayersPositionsTexture", playersPositionsTexture);
        spr.SetPropertyBlock(propertyBlock);
        objectsToTrack = new List<ShaderEffectCamera>(30);
        spr.enabled=false;
    }
    public void AddPlayerToPool(GameObject _player)
    {
        spr.enabled = true;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i]==null)
            {
                players[i] = _player;
                return;
            }
        }
        players.Add(_player);
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
