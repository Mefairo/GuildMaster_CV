using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WorldEventSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _daysText;
    [SerializeField] private WorldEvent _worldEvent;

    public WorldEvent WorldEvent => _worldEvent;

    public void Init(WorldEvent worldEvent)
    {
        _worldEvent = worldEvent;

        _icon.sprite = worldEvent.EventData.Icon;
        _daysText.text = $"{worldEvent.LeftDays}";
    }

    public void NextDay()
    {
        _daysText.text = $"{_worldEvent.LeftDays}";
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowWorldEventInfo(_worldEvent);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelUI.Instance.HideInfo();
    }
}
