using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrappleHandler : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private float maxGrappleDistance = 15f;
    [SerializeField] private float lateralBoostForce = 5f;
    [SerializeField] private LineRenderer laserPointer;
    [SerializeField] private bool showLaserWhenIdle = true;
    [SerializeField] private GameObject hookHeadPrefab;

    private PlayerInputHandler inputHandler;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    [SerializeField]private Camera mainCamera;

    private Vector2 grapplePoint;
    private bool isGrappling;

    private GameObject hookHead;
    private bool isHookFlying;
    private float hookSpeed = 100f;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        //mainCamera = Camera.main;

        springJoint = gameObject.AddComponent<SpringJoint2D>();
        springJoint.enabled = false;
        springJoint.autoConfigureDistance = false;
        springJoint.frequency = 2f;
        springJoint.dampingRatio = 0.5f;

        lineRenderer.positionCount = 0;
        lineRenderer.enabled = false;
        lineRenderer.widthMultiplier = 0.05f;

        if (hookHeadPrefab != null)
        {
            hookHead = Instantiate(hookHeadPrefab);
            hookHead.SetActive(false);
        }
    }

    private void OnEnable()
    {
        inputHandler.OnGrappleInputChanged += HandleGrappleInput;
    }

    private void OnDisable()
    {
        inputHandler.OnGrappleInputChanged -= HandleGrappleInput;
    }

    private void HandleGrappleInput(bool isPressed)
    {
        if (isPressed)
            TryStartGrapple();
        else
            StopGrapple();
    }

    private void TryStartGrapple()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mouseWorldPos - (Vector2)firePoint.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, direction, maxGrappleDistance, grappleLayer);
        if (hit.collider != null)
        {
            grapplePoint = hit.point;

            // Lanzar gancho visual
            isHookFlying = true;
            hookHead.SetActive(true);
            hookHead.transform.position = firePoint.position;

            // Desactivamos el resorte hasta que llegue
            springJoint.enabled = false;

            // Apagamos láser mientras vuela
            laserPointer.enabled = false;
        }
    }

    private void StopGrapple()
    {
        isGrappling = false;
        isHookFlying = false;
        springJoint.enabled = false;
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;

        if (hookHead != null)
            hookHead.SetActive(false);

        if (laserPointer != null)
            laserPointer.enabled = true;
    }

    public void ForceStopGrapple()
    {
        if (!isGrappling) return;

        StopGrapple();
    }


    private void Update()
    {
        if (isHookFlying && hookHead != null)
        {
            // Mover el gancho visual hacia el punto de enganche
            hookHead.transform.position = Vector2.MoveTowards(hookHead.transform.position, grapplePoint, hookSpeed * Time.deltaTime);

            // Calcular dirección y rotación
            Vector2 direction = grapplePoint - (Vector2)hookHead.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Ajustar el ángulo si tu sprite apunta hacia arriba (90°)
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);

            // Rotar suavemente
            hookHead.transform.rotation = Quaternion.Lerp(hookHead.transform.rotation, targetRotation, Time.deltaTime * 15f);

            // Cuando llega al punto de enganche
            if (Vector2.Distance(hookHead.transform.position, grapplePoint) < 0.1f)
            {
                isHookFlying = false;
                isGrappling = true;

                springJoint.enabled = true;
                springJoint.connectedAnchor = grapplePoint;
                springJoint.distance = Vector2.Distance(firePoint.position, grapplePoint) * 0.8f;

                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;

                // Impulso lateral estilo SANABI
                Vector2 toHook = (grapplePoint - (Vector2)transform.position).normalized;
                Vector2 perpendicular = Vector2.Perpendicular(toHook);
                rb.AddForce(perpendicular * lateralBoostForce, ForceMode2D.Impulse);
            }
        }
    }

    private void LateUpdate()
    {
        if (isGrappling)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, grapplePoint);
        }

        if (showLaserWhenIdle && !isHookFlying && !isGrappling && laserPointer != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            laserPointer.positionCount = 2;
            laserPointer.SetPosition(0, firePoint.position);
            laserPointer.SetPosition(1, mousePos);
            laserPointer.enabled = true;
        }
    }
}
