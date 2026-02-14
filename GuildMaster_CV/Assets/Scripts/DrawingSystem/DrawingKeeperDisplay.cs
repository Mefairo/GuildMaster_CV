using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DrawingKeeperDisplay : MonoBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private DrawingSlot_UI _drawingSlotPrefab;
    [SerializeField] private ItemCraft _requiredSlotPrefab;
    [SerializeField] private GameObject _slotsContent;
    [SerializeField] private GameObject _requiredItemsContent;
    [SerializeField] private List<CraftItemData> _newItemsList = new List<CraftItemData>();
    [SerializeField] private List<CraftItemData> _checkedItemsList = new List<CraftItemData>();
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _descriptionItemText;
    [SerializeField] private TextMeshProUGUI _itemPrimaryType;
    [SerializeField] private TextMeshProUGUI _itemSecondaryType;
    [SerializeField] private TextMeshProUGUI _powerStatsNameText;
    [SerializeField] private TextMeshProUGUI _powerStatsValueText;
    [SerializeField] private TextMeshProUGUI _defenceStatsNameText;
    [SerializeField] private TextMeshProUGUI _defenceStatsValueText;

    private CraftItemData _selectItemData;
    private StatDisplayManager _statDisplayManager = new StatDisplayManager();
    private QuestResistanceSystem _questResistanceSystem;

    public List<CraftItemData> NewItemsList => _newItemsList;
    public List<CraftItemData> CheckedItemsList => _checkedItemsList;


    public bool IsWindowChecked = true;

    private void Awake()
    {
        _questResistanceSystem = GetComponent<QuestResistanceSystem>();

        _panel.gameObject.SetActive(false);

        ClearSlots();
        ClearRequiredSlot();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_panel.gameObject.activeSelf)
                CloseDrawingWindow();
        }
    }

    public void OpenDrawingWindow()
    {
        _panel.gameObject.SetActive(true);
        _questResistanceSystem.ClearIcons();
        ClearStatsInfo();

        for (int i = _newItemsList.Count - 1; i >= 0; i--)
        {
            CreateDrawingSlot(_newItemsList[i]);
            _checkedItemsList.Add(_newItemsList[i]);
            _newItemsList.Remove(_newItemsList[i]);
            IsWindowChecked = false;
        }
    }

    private void CloseDrawingWindow()
    {
        _panel.gameObject.SetActive(false);

        ClearSlots();
        ClearRequiredSlot();

        IsWindowChecked = true;

        NotificationSystem.Instance.CheckNotifications();
    }

    private void CreateDrawingSlot(CraftItemData itemData)
    {
        DrawingSlot_UI drawingSlot = Instantiate(_drawingSlotPrefab, _slotsContent.transform);
        drawingSlot.Init(itemData);
    }

    private void CreateRequiredSlot(SlotData itemData, int amount)
    {
        ItemCraft drawingSlot = Instantiate(_requiredSlotPrefab, _requiredItemsContent.transform);
        int heroAmountItems = CheckHeroAmountItems(itemData);
        drawingSlot.SetItemComponents(itemData, amount, heroAmountItems);
    }

    private int CheckHeroAmountItems(SlotData data)
    {
        int heroAmountItems = 0;

        foreach (InventorySlot invSlot in _playerInventoryHolder.PrimaryInventorySystem.InventorySlots)
        {
            if (data == invSlot.BaseItemData)
                heroAmountItems += invSlot.StackSize;
        }

        return heroAmountItems;
    }

    private void UpdatePreviewItem()
    {
        if (_selectItemData != null)
        {
            ClearStatsInfo();
            _descriptionItemText.text = _selectItemData.Description;

            if (_selectItemData is EquipItemData equiptemData)
            {
                foreach (HeroStats stat in equiptemData.Stats)
                {
                    if (stat != null)
                    {
                        (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(stat, false);

                        _powerStatsNameText.text = statsText.Item1;
                        _powerStatsValueText.text = statsText.Item2;

                        _defenceStatsNameText.text = statsText.Item3;
                        _defenceStatsValueText.text = statsText.Item4;

                        _itemPrimaryType.text = equiptemData.ItemPrimaryType.ToString();
                        _itemSecondaryType.text = equiptemData.EquipType.ToString();

                        if (equiptemData.EquipType == EquipType.Trinket)
                        {
                            _itemSecondaryType.text = equiptemData.ItemSecondaryType.ToString();
                        }
                    }
                }
            }
        }

        if (_selectItemData is DrawingItemData drawingItemData)
            _itemSecondaryType.text = "Recipe";

        UpdateResistance();
    }

    private void UpdateResistance()
    {
        if (_selectItemData is EquipItemData equiptemData)
        {
            List<Resistance> resistances = equiptemData.Resistance[0].Resistances;

            _questResistanceSystem.ShowResistance(resistances);
        }
    }

    public void UpdatePreviewItem(ItemCraft item)
    {
        _selectItemData = (CraftItemData)item.ItemData;

        UpdatePreviewItem();
    }

    public void SelectItemSlot(CraftItemData itemData)
    {
        _selectItemData = itemData;

        ClearRequiredSlot();

        for (int i = 0; i < itemData.Recipes[0].RequiredItems.Count; i++)
        {
            CreateRequiredSlot(itemData.Recipes[0].RequiredItems[i], itemData.Recipes[0].AmountResources[i]);
        }

        UpdatePreviewItem();
    }

    private void ClearSlots()
    {
        foreach (Transform item in _slotsContent.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void ClearRequiredSlot()
    {
        foreach (Transform item in _requiredItemsContent.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    private void ClearStatsInfo()
    {
        _descriptionItemText.text = "";

        _itemPrimaryType.text = "";
        _itemSecondaryType.text = "";

        _powerStatsNameText.text = "";
        _powerStatsValueText.text = "";

        _defenceStatsNameText.text = "";
        _defenceStatsValueText.text = "";
    }
}
