using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CraftSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image _itemSprite;
    [SerializeField] private Image _backgroundSprite;
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemAmountCraft;
    [SerializeField] private Button _updatePreviewButton;
    [SerializeField] private CraftSlot _assignedItemSlot;
    //[SerializeField] private ItemsShowInfo _panelInfo;

    private ChangeColorBackgroundItem _backColorItem = new ChangeColorBackgroundItem();

    public CraftSlot AssignedItemSlot => _assignedItemSlot;


    public CraftKeeperDisplay CraftKeeperDisplay { get; private set; }


    private void Awake()
    {
        _itemSprite.sprite = null;
        _itemSprite.preserveAspect = true;
        _itemSprite.color = Color.clear;

        _backgroundSprite.sprite = null;
        _backgroundSprite.preserveAspect = true;
        _backgroundSprite.color = Color.clear;

        _itemName.text = "";
        _itemAmountCraft.text = "";

        _updatePreviewButton?.onClick.AddListener(UpdateItemPreview);

        CraftKeeperDisplay = GetComponentInParent<CraftKeeperDisplay>();
    }

    //private void Start()
    //{
    //    _panelInfo = UIManager.Instance.PanelInfo;

    //    if (_panelInfo != null)
    //        _panelInfo.gameObject.SetActive(false);
    //}

    public void Init(CraftSlot craftSlot, int amountCraftItem)
    {
        _assignedItemSlot = craftSlot;

        UpdateUISlot(amountCraftItem);
    }

    private void UpdateUISlot(int amountCraftItem)
    {
        if (_assignedItemSlot != null)
        {
            _itemSprite.sprite = _assignedItemSlot.CraftItemData.Icon;
            _itemSprite.color = Color.white;

            if (_assignedItemSlot.ItemData.IconBackground != null)
            {
                if (_assignedItemSlot.CraftItemData is EquipItemData equipItemData)
                {
                    _backgroundSprite.sprite = equipItemData.IconBackground;
                    _backgroundSprite.color = _backColorItem.ChangeColor(equipItemData.Tier);
                }
            }

            else
                _backgroundSprite.color = new Color(1, 1, 1, 0); // (R, G, B, A)
            //_backgroundSprite.color = _backgroundSprite.color.WithAlpha(0);


            _itemName.text = $"{AssignedItemSlot.ItemData.DisplayName}";

            if (amountCraftItem != 0)
                _itemAmountCraft.text = $"x{amountCraftItem}";

            else
                _itemAmountCraft.text = "";
        }
        else
        {
            _itemSprite.sprite = null;
            _itemSprite.color = Color.clear;

            //_backgroundSprite.sprite = null;
            //_backgroundSprite.color = Color.clear;

            _itemName.text = "";
            _itemAmountCraft.text = "";
        }
    }

    private void UpdateItemPreview()
    {
        CraftKeeperDisplay.UpdateItemPreview(this);
        CraftKeeperDisplay.SelectItemCraft(this);
        CraftKeeperDisplay.CanCraftItem(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //_panelInfo.ShowInfo(this.AssignedItemSlot.ItemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //_panelInfo.HideInfo();
    }

    //private void SelectSlot(CraftSlotUI craftSlotUI)
    //{
    //    ColorBlock colors = this._updatePreviewButton.colors;
    //    colors.normalColor = Color.white;
    //    colors.colorMultiplier = 5f;
    //    this._updatePreviewButton.colors = colors;
    //}

    //public void ResetSlotColor()
    //{
    //    ColorBlock colors = this._updatePreviewButton.colors;
    //    colors.normalColor = Color.white;
    //    colors.colorMultiplier = 1f;
    //    this._updatePreviewButton.colors = colors;
    //}

}
