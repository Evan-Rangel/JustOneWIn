using Mirror;
using System;
using UnityEngine;
[Serializable]
public class ObjectToLoad
{ 
    public GameObject interactuable;
    public Transform point;
}
public class LevelLoadObjects : NetworkBehaviour
{
    [SerializeField] private ObjectToLoad[] objects;
    [SerializeField] private GameObject[] platforms;
    private void Start()
    {
       // if(isServer)
       // LoadObjects();
    }
    [Server]
    void LoadObjects()
    {
        foreach (GameObject obj in platforms)
        {
            NetworkServer.Spawn(obj);
        }
        foreach (ObjectToLoad obj in objects)
        {
            GameObject spawnedObject = Instantiate(obj.interactuable, obj.point.position, obj.point.rotation);
            NetworkServer.Spawn(spawnedObject);
        }
    }

}
