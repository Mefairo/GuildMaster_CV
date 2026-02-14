using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BlankSlot_UI : InventorySlot_UI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private BlankSlot _blankSlot;

    public BlankSlot BlankSlot => _blankSlot;

    public ReforgeController ReforgeController { get; private set; }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnUISlotClick);

        ReforgeController = GetComponentInParent<ReforgeController>();

        //_backBlankSprite = null;
        ClearSlot();
    }

    public override void OnUISlotRightClick()
    {
        ReforgeController.RightClickSlot(this);
    }

    public override void OnUISlotClick()
    {
        ReforgeController?.ClickBlankSlot(this);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowBlankInfo(_blankSlot);
    }
}
