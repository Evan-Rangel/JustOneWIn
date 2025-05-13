using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*---------------------------------------------------------------------------------------------
Este script implementa un pooling system para las after images del jugador (sombras de 
movimiento tipo dash). Utiliza una cola FIFO (Queue) para manejar la reutilización de objetos.
Incluye un singleton para acceso desde cualquier parte del juego. Sistema de pool para las 
"after images" del jugador (efectos visuales de sombra que siguen al personaje).
---------------------------------------------------------------------------------------------*/

public class PlayerAfterImagePool : MonoBehaviour
{
    // Prefab del objeto que será clonado en el pool
    [SerializeField]
    private GameObject afterImagePrefab;

    private Queue<GameObject> availableObjects = new Queue<GameObject>();

    public static PlayerAfterImagePool Instance { get; private set; }

    // Inicializa el singleton y genera el pool inicial
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);

        GrowPool();
    }
    // Crea una cantidad inicial de objetos y los agrega al pool
    private void GrowPool()
    {
            Debug.Log("GrowPool");

        if (afterImagePrefab!=null)
        {
            Debug.Log("NO NULL");
            
        }
        for (int i = 0; i < 10; i++)
        {
            
            var instanceToAdd = Instantiate(afterImagePrefab);
            instanceToAdd.transform.SetParent(transform); // Los agrupa en jerarquía para mantener orden
            AddToPool(instanceToAdd);
        }
    }

    // Desactiva el objeto y lo agrega a la cola de disponibles
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    // Devuelve un objeto activo del pool; si no hay disponibles, genera más
    public GameObject GetFromPool()
    {
        if (availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
