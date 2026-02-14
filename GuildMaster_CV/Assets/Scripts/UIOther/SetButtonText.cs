using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetButtonText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string _text;

    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowText(_text);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelUI.Instance.HideInfo();
    }
}
