using Mirror;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Interactuable : NetworkBehaviour
{
    [Header("Parent")]
    [SerializeField] Transform parentTransform;

    [Header("Carge Circles")]
    [SerializeField, Tooltip("Se cargan al iniciar si se ponen de hijo a este objeto")] LoadCircles[] circles;
    [SerializeField, Tooltip("Para que los circulos se activen gradualmente o al instante")] bool loadInstant;
    [SerializeField] protected GameObject circlesHolder;
    [Header("Colors")]
    [SerializeField] CirclesColors activateColor;
    [SerializeField] CirclesColors deactivateColor;

    [Header("Events")]
    [SerializeField] UnityEvent onFinish;
    [SerializeField] UnityEvent onStart;

    bool inAction = false;
    float timeToActive;
    protected bool active;

    public virtual void Awake()
    {
        if (circlesHolder != null)
        {
            circles = circlesHolder.GetComponentsInChildren<LoadCircles>();
            foreach (LoadCircles circle in circles)
            {
                circle.gameObject.transform.parent = parentTransform;
            }
        }

    }
    public virtual void Start()
    {
        onStart.Invoke();
    }
    [Tooltip("Al activar el trigger, el estado sera estado = !estado")]
    public void ActivateAndDeactivate()
    {
        if (active) Deactivate();
        else Activate();
    }
    [Tooltip("Activa el objeto")]
    public virtual void Activate()
    {
        if (!inAction)
        {
            StartCoroutine(ChangeCircleColors(activateColor, "Activate", "ActivateWithTime"));
            active = true;
        }
    }
    public virtual void Deactivate()
    {
        if (!inAction)
        {
            StartCoroutine( ChangeCircleColors(deactivateColor, "Deactivate", "DeactivateWithTime"));
            active = false;
        }
    }
    public void ActivateWithTime(float _time)
    {
        if (!inAction)
        {
            inAction = true;
            timeToActive = _time;
            StartCoroutine(ActivateByTime());
        }
    }
    public void DeactivateWithTime(float _time)
    {
        if (!inAction)
        {
            inAction = true;
            timeToActive = _time;
            StartCoroutine(DeactivateByTime());
        }
    }

    IEnumerator ChangeCircleColors(CirclesColors _color, string _functionName1, string _functionName2)
    {
        yield return Helpers.GetWait(0.05f);
        foreach (var circle in circles)
        {
            circle.ChangeColor(timeToActive / circles.Length, _color, false);
        }
        bool _onFinish = true;
        for (int i = 0; i < onFinish.GetPersistentEventCount(); i++)
        {
            if (onFinish.GetPersistentMethodName(i) == _functionName1 || onFinish.GetPersistentMethodName(i) == _functionName2)
            {
                _onFinish = false;
                break;
            }
        }
        if (_onFinish) onFinish.Invoke();
        yield break;
    }
    IEnumerator DeactivateByTime()
    {
        //For para que sea inverso
        for (int i = circles.Length-1; i >= 0; i--)
        {
            yield return new WaitForSeconds(timeToActive / circles.Length);
            circles[i].ChangeColor(timeToActive / circles.Length, deactivateColor, !loadInstant);
        }
        inAction = false;
        Deactivate();
    }
    IEnumerator ActivateByTime()
    {
        foreach (var circle in circles)
        {
            yield return new WaitForSeconds(timeToActive / circles.Length);
            circle.ChangeColor(timeToActive / circles.Length, activateColor, !loadInstant);
        }
        inAction = false;
        Activate();
    }
}
