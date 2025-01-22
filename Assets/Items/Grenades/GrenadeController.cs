using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GrenadeController : NetworkBehaviour
{
    [SerializeField] float timeToExplote;
    [SerializeField] UnityEvent grenadeActivation;
    [field: SerializeField]public float grenadeForce { get; private set; }
    public void ActiveExpansiveWave() => grenadeActivation.Invoke();
    float timer = 0;
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > timeToExplote)
        {
            ActiveExpansiveWave();
        }
    }
}
