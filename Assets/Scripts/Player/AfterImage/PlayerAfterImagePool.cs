using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerAfterImagePool : MonoBehaviour 
{
    //Prefab Var
    [Header("Player Dash Image Pool")]
    [SerializeField]
    private GameObject afterImagePrefab;

    private Queue<GameObject> availableObject = new Queue<GameObject>();//Store objects that are not currently active

    //Singleton
    public static PlayerAfterImagePool Instance { get; private set; }

    //Awake
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        DontDestroyOnLoad(this);
        GrowPool();
    }

    //Function GrowPool
    private void GrowPool()
    {
        //Create more game Objects to the pool
        for(int i = 0; i < 10; i++)
        {
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform);//Make the game object create a child of the game
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObject.Enqueue(instance);//Add to the Queue
    }

    public GameObject GetFromPool()
    {
        //Condition that if we trying to get an afterimage object to spawn and they are non available, make some more
        if(availableObject.Count == 0)
        {
            GrowPool();
        }

        var instance = availableObject.Dequeue();//This take the object from the queue
        instance.SetActive(true);
        return instance;
    }
}
