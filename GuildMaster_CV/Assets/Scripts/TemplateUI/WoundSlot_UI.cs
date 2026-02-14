using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WoundSlot_UI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private WoundsType _woundType;
    [SerializeField] private Image _mainIcon;
    [SerializeField] private Image _frame;

    [SerializeField] private Sprite _healthyIcon;
    [SerializeField] private Sprite _lightIcon;
    [SerializeField] private Sprite _mediumIcon;
    [SerializeField] private Sprite _heavyIcon;
    [SerializeField] private Sprite _deadIcon;

    public Image MainIcon => _mainIcon;

    public void SetImage(WoundsType woundType)
    {
        _woundType = woundType;

        ClearIcon();

        switch(woundType)
        {
            case WoundsType.None:
               
                break;

            case WoundsType.Healthy:
                _mainIcon.sprite = _healthyIcon;
                break;

            case WoundsType.Light:
                _mainIcon.sprite = _lightIcon;
                break;

            case WoundsType.Medium:
                _mainIcon.sprite = _mediumIcon;
                break;

            case WoundsType.Heavy:
                _mainIcon.sprite = _heavyIcon;
                break;

            case WoundsType.Dead:
                _mainIcon.sprite = _deadIcon;
                break;
        }
    }

    private void ClearIcon()
    {
        _mainIcon.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowWoundImageInfo(_woundType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelUI.Instance.HideInfo();
    }
}
