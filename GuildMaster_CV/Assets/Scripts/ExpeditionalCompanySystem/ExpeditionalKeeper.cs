using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ExpeditionalKeeper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private ExpeditionalCompany _expeditionalCompany;
    [SerializeField] private NextDayController _nextDayController;
    [Header("UI")]
    //[SerializeField] protected Button _activeButton;
    [SerializeField] private SpriteRenderer _glow;
    [SerializeField] private SpriteRenderer _namePanel;

    public static event UnityAction OnExpeditionalCompanyOpen;

    private void Start()
    {
        _glow.gameObject.SetActive(false);
        _namePanel.gameObject.SetActive(false);
    }

    private void OpenExpeditionWindow()
    {
        if (!_nextDayController.InTransition)
            OnExpeditionalCompanyOpen?.Invoke();
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
        OpenExpeditionWindow();
    }
}
