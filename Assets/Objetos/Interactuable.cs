using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Interactuable : MonoBehaviour
{
    [SerializeField] LoadCircles[] circles;
    float timeToActive;
    [SerializeField] UnityEvent onFinish;
    [SerializeField] UnityEvent onStart;
    [SerializeField] CirclesColors activateColor;
    [SerializeField] CirclesColors deactivateColor;
    bool inAction = false;
    [SerializeField] bool active;
    [SerializeField] bool loadInstant;

    private void Awake()
    {
        circles = GetComponentsInChildren<LoadCircles>();
    }
    private void Start()
    {
        onStart.Invoke();

    }
    [Tooltip("Al presionar el boton, el estado sera estado = !estado")]
    public void ActivateAndDeactivate()
    {
        if (active)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }
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
        yield return new WaitForSeconds(0.05f);
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
            circles[i].ChangeColor(timeToActive / circles.Length, deactivateColor, loadInstant);
            yield return new WaitForSeconds(timeToActive / circles.Length);
        }
        inAction = false;
        Deactivate();
    }
    IEnumerator ActivateByTime()
    {
        foreach (var circle in circles)
        {
            circle.ChangeColor(timeToActive / circles.Length, activateColor, loadInstant);
            yield return new WaitForSeconds(timeToActive / circles.Length);
        }
        inAction = false;
        Activate();
    }
}
