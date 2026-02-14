using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AutoFillSlot_UI : MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _stacksize;
    [SerializeField] private Button _buttonSelf;
    [Header("Slot")]
    [SerializeField] private InventorySlot _autoFillSlot;
    [SerializeField] private int _activeStackSize;

    public InventorySlot AutoFillSlot => _autoFillSlot;
    public int ActiveStackSize => _activeStackSize;

    public AutofillSystem AutofillSystem { get; private set; }

    private void Awake()
    {
        //_buttonSelf?.onClick.RemoveAllListeners();
        _buttonSelf?.onClick.AddListener(ClickSlot);

        AutofillSystem = GetComponentInParent<AutofillSystem>();
    }

    public void Init(InventorySlot newSlot)
    {
        _autoFillSlot = newSlot;
        _activeStackSize = newSlot.StackSize;

        _icon.sprite = newSlot.BaseItemData.Icon;
        _stacksize.text = $"{_activeStackSize}";
    }

    public void AddToStack(int amount)
    {
        _activeStackSize += amount;

        _stacksize.text = $"{_activeStackSize}";
    }

    private void ClickSlot()
    {
        AutofillSystem.AutoFillItem(this);
    }
}
