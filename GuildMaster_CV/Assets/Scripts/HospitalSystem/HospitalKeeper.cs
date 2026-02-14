using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HospitalKeeper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private HospitalUIController _hospitalUIController;
    [SerializeField] private NextDayController _nextDayController;
    [Header("UI")]
    //[SerializeField] protected Button _activeButton;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _namePanel;

    public static event UnityAction<HospitalUIController> OnHospitalSystemOpen;

    private void Start()
    {
        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    private void OpenHospitalWindow()
    {
        if (!_nextDayController.InTransition)
            OnHospitalSystemOpen?.Invoke(_hospitalUIController);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(true);
        _namePanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OpenHospitalWindow();
    }
}
