using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuneSlot_UI : InventorySlot_UI
{
    [SerializeField] private RuneSlot _runeSlot;

    public RuneSlot RuneSlot => _runeSlot;

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
        ReforgeController?.ClickRuneSlot(this);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelUI.Instance.ShowRuneInfo(_runeSlot);
    }
}
