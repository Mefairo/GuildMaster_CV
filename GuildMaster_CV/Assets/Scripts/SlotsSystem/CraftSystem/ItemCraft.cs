using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class ItemCraft : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private SlotData _itemData;
    [SerializeField] private TextMeshProUGUI _nameComponent;
    [SerializeField] private TextMeshProUGUI _amountComponent;
    [SerializeField] private Image _imageComponent;
    [SerializeField] private Image _imageBackgroundComponent;
    [SerializeField] private Button _updatePreviewButton;
    //[SerializeField] private ItemsShowInfo _panelInfo;

    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    public SlotData ItemData => _itemData;
    public TextMeshProUGUI NameComponent => _nameComponent;
    public TextMeshProUGUI AmountComponent => _amountComponent;
    public Image ImageComponent => _imageComponent;
    public Image ImageBackgroundComponent => _imageBackgroundComponent;
    public CraftKeeperDisplay CraftKeeperDisplay { get; private set; }
    public DrawingKeeperDisplay DrawingKeeperDisplay { get; private set; }

    private void Awake()
    {
        _updatePreviewButton?.onClick.AddListener(UpdateItemPreview);

        CraftKeeperDisplay = GetComponentInParent<CraftKeeperDisplay>();
        DrawingKeeperDisplay = GetComponentInParent<DrawingKeeperDisplay>();
    }

    //private void Start()
    //{
    //    _panelInfo = UIManager.Instance.PanelInfo;

    //    if (_panelInfo != null)
    //        _panelInfo.gameObject.SetActive(false);
    //}

    public void SetItemComponents(SlotData itemData, string newName, string newAmount, Sprite newImage, int heroAmountItems)
    {
        _itemData = itemData;
        _nameComponent.text = newName;
        _amountComponent.text = $"x{newAmount} ({heroAmountItems})";
        _imageComponent.sprite = newImage;

        if (itemData.IconBackground != null)
        {
            if (itemData is EquipItemData equipItemData)
            {
                _imageBackgroundComponent.sprite = equipItemData.IconBackground;
                _imageBackgroundComponent.color = _backColorItem.ChangeColor(equipItemData.Tier);
            }
        }

        if (itemData.IconBackground == null)
            _imageBackgroundComponent.color = new Color(1, 1, 1, 0); // (R, G, B, A)

        //_imageBackgroundComponent.color = _imageBackgroundComponent.color.WithAlpha(0);
    }

    public void SetItemComponents(SlotData itemData, int amount, int heroAmountItems)
    {
        _itemData = itemData;
        _nameComponent.text = itemData.DisplayName;
        _amountComponent.text = $"x{amount} ({heroAmountItems})";
        _imageComponent.sprite = itemData.Icon;

        if (itemData.IconBackground != null)
        {
            if (itemData is EquipItemData equipItemData)
            {
                _imageBackgroundComponent.sprite = equipItemData.IconBackground;
                _imageBackgroundComponent.color = _backColorItem.ChangeColor(equipItemData.Tier);
            }
        }
    }

    private void UpdateItemPreview()
    {
        if (CraftKeeperDisplay != null)
            CraftKeeperDisplay.UpdateItemPreview(this);

        if (DrawingKeeperDisplay != null)
            DrawingKeeperDisplay.UpdatePreviewItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //_panelInfo.ShowInfo(this.ItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_panelInfo.HideInfo();
    }
}
