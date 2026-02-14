using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovePlayerInventory : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform _rectTransform;
    private Canvas _canvas;
    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();

        // Найти канву в родителях
        _canvas = GetComponentInParent<Canvas>();
        if (_canvas == null)
        {
            Debug.LogError("Canvas not found in parent hierarchy.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 0.6f; // Изменение прозрачности во время перетаскивания
            _canvasGroup.blocksRaycasts = false; // Отключить блокировку лучей для событий UI
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_canvas != null)
        {
            _rectTransform.anchoredPosition += eventData.delta / _canvas.scaleFactor; // Перемещение элемента
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_canvasGroup != null)
        {
            _canvasGroup.alpha = 1f; // Восстановление прозрачности
            _canvasGroup.blocksRaycasts = true; // Включить блокировку лучей для событий UI
        }
    }
}
