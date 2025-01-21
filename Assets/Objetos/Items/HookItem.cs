using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class HookItem : Item
{
    /*
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float hookTrowSpeed;
    [SerializeField] float hookGrabSpeed;
    [SerializeField] float endForce;
    PlayerInput playerInput;
  
    public override void Action(PlayerItemManager _playerItemManager)
    {
        //Debug.Log("Hook Item");
        playerInput= _playerItemManager.GetPlayerInput();
        StartCoroutine(TrowHook(_playerItemManager.GetItemHolderFront(), _playerItemManager));
    }
    IEnumerator TrowHook(Transform _origingPos, PlayerItemManager _playerItemManager)
    {
        GameObject player = _playerItemManager.gameObject;
        DistanceJoint2D distanceJoint2D = _playerItemManager.distanceJoint;
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mousePos - (Vector2)_origingPos.position).normalized;
        RaycastHit2D hit= new RaycastHit2D();
        float dist = 0;
        Vector2 target= Vector2.zero;
        while (playerInput.actions["Fire"].IsPressed()) 
        {
            hit = Physics2D.Raycast(_origingPos.position, direction,dist * hookTrowSpeed);
            transform.position = _origingPos.position;
            lineRenderer.SetPosition(0, _origingPos.position);
            lineRenderer.SetPosition(1,(Vector2)_origingPos.position+(direction*dist* hookTrowSpeed));
            lineRenderer.enabled = true;
            dist += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            if (hit.collider!=null)
            {
                target = hit.point;
                transform.parent = player.transform;
                distanceJoint2D.enabled = true;
                distanceJoint2D.distance = Vector2.Distance(target, _origingPos.position);
                distanceJoint2D.connectedAnchor = target;

                break;
            }
        }
        if (hit.collider != null)
        {
            if (hit.transform.CompareTag("Enemy"))
            {
                Vector2 forceDir= (hit.transform.position.x>player.transform.position.x)? Vector2.left: Vector2.right;
                float forceSpeed=200;
                hit.transform.GetComponent<Rigidbody2D>().AddForce(forceDir*forceSpeed+Vector2.up*50);
                while (Vector2.Distance(transform.position, target) > 1)
                {
                    distanceJoint2D.distance -= Time.deltaTime * hookGrabSpeed;
                    lineRenderer.SetPosition(0, transform.position);
                    lineRenderer.SetPosition(1, hit.transform.position);

                    yield return Helpers.GetWaitForEndOfFrame();
                    if (playerInput.actions["Fire"].WasReleasedThisFrame())
                    {
                        Debug.Log(direction * endForce);
                       // player.GetComponent<Rigidbody2D>().AddForce(direction * endForce); ;
                        break;
                    }
                }
                yield return new WaitUntil(() => playerInput.actions["Fire"].WasReleasedThisFrame());
            }
            else 
            {
                while (!playerInput.actions["Fire"].WasReleasedThisFrame())
                {
                    lineRenderer.SetPosition(0, transform.position);
                    yield return Helpers.GetWaitForEndOfFrame();
                }
            }
            transform.parent = null;
            distanceJoint2D.enabled = false;
        }
        Destroy(gameObject);
        yield break;
    }*/
}
