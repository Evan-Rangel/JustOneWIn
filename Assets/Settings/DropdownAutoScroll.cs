using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(ScrollRect))]
public class DropdownAutoScroll : MonoBehaviour
{
    private ScrollRect scrollRect;
    private RectTransform contentRect;

    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;
    }

    private void Update()
    {
        // Solo actuamos si estamos dentro del dropdown abierto
        if (EventSystem.current == null) return;

        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null || !selected.transform.IsChildOf(contentRect)) return;

        RectTransform selectedRect = selected.GetComponent<RectTransform>();
        if (selectedRect == null) return;

        // Calcula la posición para centrar el item seleccionado
        ScrollToCenter(selectedRect);
    }

    private void ScrollToCenter(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();

        Vector2 contentWorldPos = contentRect.position;
        Vector2 targetLocalPos = contentRect.InverseTransformPoint(target.position);

        // Posición normalizada (0 = abajo, 1 = arriba en vertical)
        float targetNormalized = (contentRect.rect.height - (targetLocalPos.y + target.rect.height / 2f))
                                / (contentRect.rect.height - scrollRect.viewport.rect.height);

        scrollRect.verticalNormalizedPosition = Mathf.Clamp01(targetNormalized);
    }
}