using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PCameraController : NetworkBehaviour
{
    [SerializeField] GameObject cameraHolder;
    public Camera mainCamera;
    public override void OnStartAuthority()
    {
        cameraHolder.SetActive(true);
        cameraHolder.transform.parent = null;
        DontDestroyOnLoad(cameraHolder.transform);
    }

    private void LateUpdate()
    {
        cameraHolder.transform.position = transform.position + Vector3.back * 10;

        //StartCoroutine(CamDelay(transform));
    }
    IEnumerator CamDelay(Transform _pos)
    {
        yield return Helpers.GetWait(1);
        cameraHolder.transform.position = _pos.position + Vector3.back * 10;

    }
}

