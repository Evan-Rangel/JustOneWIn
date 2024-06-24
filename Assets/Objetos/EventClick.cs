using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EventClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] UnityEvent PointerDown;
    [SerializeField] UnityEvent PointerUp;
    [SerializeField] UnityEvent PointerClick;
    [SerializeField] UnityEvent PointerEnter;
    [SerializeField] UnityEvent PointerExit;
    public void OnPointerDown(PointerEventData eventData)
    {
        PointerDown.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        PointerUp.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        PointerClick.Invoke();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        PointerEnter.Invoke();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        PointerExit.Invoke();
    }
}
