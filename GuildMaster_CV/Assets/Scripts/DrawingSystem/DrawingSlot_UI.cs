using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawingSlot_UI : MonoBehaviour
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private Image _backgroundSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private Button _updatePreviewButton;
    [SerializeField] private CraftItemData _assignedSlotData;

    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    public DrawingKeeperDisplay DrawingKeeperDisplay { get; private set; }

    private void Awake()
    {
        ClearInfo();

        _updatePreviewButton?.onClick.AddListener(ClickSlot);

        DrawingKeeperDisplay = GetComponentInParent<DrawingKeeperDisplay>();
    }

    public void Init(CraftItemData itemData)
    {
        _assignedSlotData = itemData;
        UpdateUISlot();
    }

    private void UpdateUISlot()
    {
        if (_assignedSlotData != null)
        {
            _itemSprite.sprite = _assignedSlotData.Icon;
            _itemSprite.color = Color.white;

            if (_assignedSlotData.IconBackground != null)
            {
                if (_assignedSlotData is EquipItemData equipItemData)
                {
                    _backgroundSprite.sprite = equipItemData.IconBackground;
                    _backgroundSprite.color = _backColorItem.ChangeColor(equipItemData.Tier);
                }
            }

            else
                _backgroundSprite.color = new Color(1, 1, 1, 0); // (R, G, B, A)
            //_backgroundSprite.color = _backgroundSprite.color.WithAlpha(0);


            _itemName.text = $"{_assignedSlotData.DisplayName}";
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.color = Color.clear;

            //_backgroundSprite.sprite = null;
            //_backgroundSprite.color = Color.clear;

            _itemName.text = "";
        }
    }

    private void ClickSlot()
    {
        UpdateUISlot();
        DrawingKeeperDisplay.SelectItemSlot(_assignedSlotData);
    }

    private void ClearInfo()
    {
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;

        _backgroundSprite.sprite = null;
        _backgroundSprite.preserveAspect = true;
        _backgroundSprite.color = Color.clear;

        _itemName.text = "";
    }
}
