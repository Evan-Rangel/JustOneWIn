using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class GrappleHandler : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask grappleLayer;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float maxGrappleDistance = 15f;
    [SerializeField] private float lateralBoostForce = 5f;
    [SerializeField] private LineRenderer laserPointer;
    [SerializeField] private bool showLaserWhenIdle = true;
    [SerializeField] private GameObject hookHeadPrefab;

    [Header("Configuración Gancho")]
    [SerializeField] private float hookSpeed = 100f;
    [SerializeField] private float hookReturnSpeed = 80f;
    [SerializeField] private float bounceForce = 2f;

    private PlayerInputHandler inputHandler;
    private Rigidbody2D rb;
    private LineRenderer lineRenderer;
    private SpringJoint2D springJoint;
    private Camera mainCamera;

    private Vector2 grapplePoint;
    private Vector2 targetPoint;
    private bool isGrappling;

    private GameObject hookHead;
    private bool isHookFlying;
    private bool isHookReturning;
    private Vector2 hookDirection;

    // Estados del gancho
    private enum HookState
    {
        Idle,
        Flying,
        Attached,
        Returning
    }
    private HookState currentHookState = HookState.Idle;

    private void Awake()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        rb = GetComponent<Rigidbody2D>();
        lineRenderer = GetComponent<LineRenderer>();
        mainCamera = Camera.main;

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
        if (isPressed && currentHookState == HookState.Idle)
        {
            LaunchHook();
        }
        else if (!isPressed && currentHookState == HookState.Attached)
        {
            // Solo desenganchamos si estaba enganchado y soltamos la tecla
            StartHookReturn();
        }
        // Si está volando o regresando, ignoramos el input hasta que termine
    }

    private void LaunchHook()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        hookDirection = (mouseWorldPos - (Vector2)firePoint.position).normalized;

        // Calculamos el punto objetivo (máxima distancia en esa dirección)
        targetPoint = (Vector2)firePoint.position + hookDirection * maxGrappleDistance;

        // Activamos el gancho visual
        currentHookState = HookState.Flying;
        hookHead.SetActive(true);
        hookHead.transform.position = firePoint.position;

        // Apagamos láser mientras vuela
        if (laserPointer != null)
            laserPointer.enabled = false;

        // Activamos la línea
        lineRenderer.enabled = true;
        lineRenderer.positionCount = 2;
    }

    private void StopGrapple()
    {
        isGrappling = false;
        currentHookState = HookState.Idle;
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
        switch (currentHookState)
        {
            case HookState.Flying:
                HandleHookFlying();
                break;
            case HookState.Attached:
                HandleHookAttached();
                break;
            case HookState.Returning:
                HandleHookReturning();
                break;
        }
    }

    private void HandleHookFlying()
    {
        if (hookHead == null) return;

        Vector2 currentPos = hookHead.transform.position;
        Vector2 nextPos = Vector2.MoveTowards(currentPos, targetPoint, hookSpeed * Time.deltaTime);

        // Rotar el gancho hacia donde va
        Vector2 direction = targetPoint - currentPos;
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, angle - 90f);
            hookHead.transform.rotation = Quaternion.Lerp(hookHead.transform.rotation, targetRotation, Time.deltaTime * 15f);
        }

        // Verificar colisión con superficies enganchables
        RaycastHit2D grappleHit = Physics2D.Raycast(currentPos, (nextPos - currentPos).normalized,
            Vector2.Distance(currentPos, nextPos), grappleLayer);

        // Verificar colisión con el suelo/paredes
        RaycastHit2D groundHit = Physics2D.Raycast(currentPos, (nextPos - currentPos).normalized,
            Vector2.Distance(currentPos, nextPos), groundLayer);

        if (grappleHit.collider != null)
        {
            // ¡Encontramos superficie enganchable!
            grapplePoint = grappleHit.point;
            hookHead.transform.position = grapplePoint;
            AttachHook();
        }
        else if (groundHit.collider != null)
        {
            // Chocamos con una pared/suelo - rebotamos y regresamos
            hookHead.transform.position = groundHit.point;
            StartHookReturn();
        }
        else
        {
            hookHead.transform.position = nextPos;

            // Si llegamos al punto objetivo sin enganchar, rebotamos y regresamos
            if (Vector2.Distance(nextPos, targetPoint) < 0.1f)
            {
                StartHookReturn();
            }
        }
    }

    private void AttachHook()
    {
        currentHookState = HookState.Attached;
        isGrappling = true;

        springJoint.enabled = true;
        springJoint.connectedAnchor = grapplePoint;
        springJoint.distance = Vector2.Distance(firePoint.position, grapplePoint) * 0.8f;

        // Impulso lateral estilo SANABI
        Vector2 toHook = (grapplePoint - (Vector2)transform.position).normalized;
        Vector2 perpendicular = Vector2.Perpendicular(toHook);
        rb.AddForce(perpendicular * lateralBoostForce, ForceMode2D.Impulse);

        // Verificar si el jugador ya soltó la tecla para retornar inmediatamente
        if (!inputHandler.GrappleInput)
        {
            StartHookReturn();
        }
    }

    private void StartHookReturn()
    {
        currentHookState = HookState.Returning;
        isGrappling = false;
        springJoint.enabled = false;

        // Pequeño rebote visual
        Vector2 bounceDirection = -hookDirection;
        hookHead.transform.position = (Vector2)hookHead.transform.position + bounceDirection * bounceForce;

        // Cambiar rotación para que apunte de regreso
        Vector2 returnDirection = (Vector2)firePoint.position - (Vector2)hookHead.transform.position;
        if (returnDirection != Vector2.zero)
        {
            float angle = Mathf.Atan2(returnDirection.y, returnDirection.x) * Mathf.Rad2Deg;
            hookHead.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    private void HandleHookReturning()
    {
        if (hookHead == null) return;

        Vector2 currentPos = hookHead.transform.position;
        Vector2 nextPos = Vector2.MoveTowards(currentPos, firePoint.position, hookReturnSpeed * Time.deltaTime);

        hookHead.transform.position = nextPos;

        // Cuando regresa al jugador
        if (Vector2.Distance(nextPos, firePoint.position) < 0.5f)
        {
            StopGrapple();
        }
    }

    private void HandleHookAttached()
    {
        // Aquí puedes agregar lógica adicional mientras está enganchado
        // Como efectos de partículas, sonidos, etc.
    }

    private void LateUpdate()
    {
        // Actualizar línea visual
        if (currentHookState != HookState.Idle && hookHead != null)
        {
            lineRenderer.SetPosition(0, firePoint.position);
            lineRenderer.SetPosition(1, hookHead.transform.position);
        }

        // Mostrar láser cuando está inactivo
        if (showLaserWhenIdle && currentHookState == HookState.Idle && laserPointer != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            laserPointer.positionCount = 2;
            laserPointer.SetPosition(0, firePoint.position);
            laserPointer.SetPosition(1, mousePos);
            laserPointer.enabled = true;
        }
    }
}