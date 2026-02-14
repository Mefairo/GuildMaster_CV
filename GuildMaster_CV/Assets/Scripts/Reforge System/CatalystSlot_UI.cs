using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CatalystSlot_UI : InventorySlot_UI
{
    [SerializeField] private CatalystSlot _catalystSlot;

    public CatalystSlot CatalystSlot => _catalystSlot;

    public ReforgeController ReforgeController { get; private set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnUISlotClick);

        ReforgeController = GetComponentInParent<ReforgeController>();

        ClearSlot();
    }

    public override void OnUISlotRightClick()
    {
        ReforgeController.RightClickSlot(this);
    }

    public override void OnUISlotClick()
    {
        ReforgeController?.ClickCatalystSlot(this);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowCatalystInfo(_catalystSlot);
    }
}
