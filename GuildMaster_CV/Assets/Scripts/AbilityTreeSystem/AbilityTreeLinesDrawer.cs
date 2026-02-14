using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AbilityTreeLinesDrawer : MonoBehaviour
{
    [SerializeField] private RectTransform _linePrefab; // Prefab для линий
    [SerializeField] private List<AbilityTreeSlot_UI> _slots; // Список всех слотов
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private RectTransform _linesContent;

    private List<RectTransform> _lines = new List<RectTransform>(); // Линии между слотами
    private AbilityTree _abilityTree;

    public RectTransform MainCanvas => _canvas;
    public RectTransform LinePrefab => _linePrefab;
    public List<AbilityTreeSlot_UI> Slots => _slots;



    private void Awake()
    {
        _abilityTree = GetComponent<AbilityTree>();
    }

    public void DrawLinesForPrimarySlot()
    {
        // Получаем список всех слотов
        _slots = new List<AbilityTreeSlot_UI>(GetComponentsInChildren<AbilityTreeSlot_UI>());

        // Принудительно обновляем Canvas и LayoutGroup
        Canvas.ForceUpdateCanvases();

        foreach (Transform slot in _abilityTree.PrimaryContent.transform)
        {
            AbilityTreeSlot_UI childSlot = slot.GetComponent<AbilityTreeSlot_UI>();
            if (childSlot != null)
            {
                CreateLine(_abilityTree.PrimaryAbility, childSlot);
            }
        }
    }

    public void DrawLinesForChildrens()
    {
        // Принудительно обновляем Canvas и LayoutGroup
        Canvas.ForceUpdateCanvases();

        // Рисуем линии от каждого слота к его дочерним слотам
        foreach (AbilityTreeSlot_UI slot in _slots)
        {
            foreach (Transform childTransform in slot.HLGObject.transform)
            {
                AbilityTreeSlot_UI childSlot = childTransform.GetComponent<AbilityTreeSlot_UI>();
                if (childSlot != null)
                {
                    CreateLine(slot, childSlot);
                }
            }
        }
    }

    private void CreateLine(AbilityTreeSlot_UI startSlot, AbilityTreeSlot_UI endSlot)
    {
        // Получаем RectTransform обоих слотов
        RectTransform startSlotTransform = startSlot.GetComponent<RectTransform>();
        RectTransform endSlotTransform = endSlot.GetComponent<RectTransform>();

        // Преобразуем позиции слотов в мировые координаты
        Vector3 worldStart = startSlotTransform.position;
        Vector3 worldEnd = endSlotTransform.position;

        // Преобразуем мировые координаты в локальные координаты относительно Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas,
            RectTransformUtility.WorldToScreenPoint(Camera.main, worldStart),
            Camera.main, out Vector2 localPointStart);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas,
            RectTransformUtility.WorldToScreenPoint(Camera.main, worldEnd),
            Camera.main, out Vector2 localPointEnd);

        // Добавляем смещение к начальной точке
        localPointStart.y -= 41f;
        localPointEnd.y += 41f;

        // Вычисляем параметры для линии
        Vector2 direction = localPointEnd - localPointStart; // Направление
        float distance = direction.magnitude;               // Длина линии

        // Создаем экземпляр линии
        RectTransform line = Instantiate(_linePrefab, _linesContent);
        endSlot.SetDrawLine(line, startSlot);
        //_lines.Add(line);

        // Устанавливаем параметры линии
        line.anchoredPosition = localPointStart + direction / 2; // Центр линии
        line.sizeDelta = new Vector2(distance, 10f);             // Длина и толщина линии
        line.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg); // Угол линии
    }

    public void ClearLines()
    {
        foreach (Transform line in _linesContent.Cast<Transform>())
        {
            Destroy(line.gameObject);
        }

       _slots.Clear();
    }
}
