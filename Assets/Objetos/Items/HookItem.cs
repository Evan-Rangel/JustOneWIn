using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookItem : Item, ItemAction
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] DistanceJoint2D distanceJoint2D;
    [SerializeField] PlayerInput playerInput;
    private void Start()
    {
        distanceJoint2D.enabled = false;
    }
    public void Action(Transform _player)
    {
        Debug.Log("Hook Item");
        StartCoroutine(TrowHook(_player));
    }
    IEnumerator TrowHook(Transform _player)
    {
        while (playerInput.actions["Fire"].IsPressed()) 
        {
            Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0,mousePos);
            lineRenderer.SetPosition(1, _player.transform.position);
            distanceJoint2D.connectedAnchor = mousePos;
            distanceJoint2D.enabled= true;
            lineRenderer.enabled = true;
            yield return new WaitForEndOfFrame();
        }
        lineRenderer.enabled = false;
        distanceJoint2D.enabled= false;
        yield break;

    }

}
