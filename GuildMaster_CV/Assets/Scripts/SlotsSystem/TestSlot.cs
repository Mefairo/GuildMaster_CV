using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestSlot : Button
{
    [SerializeField] private RectTransform _canvas; // Canvas для работы с UI
    [SerializeField] private Image _imagePrefab; // Префаб Image
    [SerializeField] private RectTransform _linePrefab; // Префаб для линии (UI)

    private RectTransform _firstImage; // Первый объект
    private RectTransform _lastImage; // Последний созданный объект

    //private void Start()
    //{
    //    // Создаем первый объект в случайной позиции
    //    Vector2 randomPosition = GetRandomCanvasPosition();
    //    _firstImage = Instantiate(_imagePrefab, _canvas).rectTransform;
    //    _firstImage.anchoredPosition = randomPosition;

    //    _lastImage = _firstImage; // Изначально первый объект — последний
    //}

    private void Update()
    {
        //if (Input.GetMouseButtonDown(0)) // ЛКМ
        //{
        //    // Создаем новый объект в позиции клика
        //    Vector2 clickPosition = GetMouseCanvasPosition();
        //    RectTransform newImage = Instantiate(_imagePrefab, _canvas).rectTransform;
        //    newImage.anchoredPosition = clickPosition;

        //    // Рисуем линию между последним объектом и новым
        //    DrawLine(_lastImage, newImage);

        //    _lastImage = newImage; // Обновляем последний объект
        //}
    }

    // Метод для получения случайной позиции внутри Canvas
    //private Vector2 GetRandomCanvasPosition()
    //{
    //    float x = Random.Range(-_canvas.rect.width / 2, _canvas.rect.width / 2);
    //    float y = Random.Range(-_canvas.rect.height / 2, _canvas.rect.height / 2);
    //    return new Vector2(x, y);
    //}

    // Метод для преобразования позиции мыши в координаты Canvas
    //private Vector2 GetMouseCanvasPosition()
    //{
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, Input.mousePosition, null, out Vector2 localPoint);
    //    return localPoint;
    //}

    // Метод для рисования линии между двумя объектами
    private void DrawLine(RectTransform start, RectTransform end)
    {
        //Создаем линию
        RectTransform line = Instantiate(_linePrefab, _canvas);

        //Рассчитываем позицию и угол поворота
       Vector2 startPos = start.anchoredPosition;
        Vector2 endPos = end.anchoredPosition;

        Vector2 direction = endPos - startPos;
        float distance = direction.magnitude;

        line.anchoredPosition = startPos + direction / 2; // Центр линии
        line.sizeDelta = new Vector2(distance, 10f); // Длина линии и толщина
        line.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg);
    }

    [SerializeField] private RectTransform _element; // Целевой элемент
    [SerializeField] private RectTransform _canvas1; // Canvas, на который нужно перевести координаты


    private Vector2 GetCanvasPosition(RectTransform element, RectTransform canvas)
    {
        // Преобразуем мировые координаты элемента в экранные координаты
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, element.position);

        // Преобразуем экранные координаты в локальные координаты Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, screenPoint, Camera.main, out Vector2 localPoint);

        return localPoint;
    }
}
