using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class IDraggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector2 offset;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetComponent<UIElementFunctions>().CanBeMoved)
        {
            canvasGroup.blocksRaycasts = false;
            offset = eventData.position - rectTransform.anchoredPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GetComponent<UIElementFunctions>().CanBeMoved)
        {
            rectTransform.anchoredPosition = eventData.position - offset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GetComponent<UIElementFunctions>().CanBeMoved)
        {
            canvasGroup.blocksRaycasts = true;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.transform.SetAsLastSibling();
    }
}