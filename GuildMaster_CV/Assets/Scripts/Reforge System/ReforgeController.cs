using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ReforgeController : MonoBehaviour
{
    [Header("Main Parametres")]
    [SerializeField] private Image _reforgePanel;
    [SerializeField] private BlankSlot_UI _blankSlot;
    [SerializeField] private List<RuneSlot_UI> _runeSlots = new List<RuneSlot_UI>();
    [SerializeField] private CatalystSlot_UI _catalystSlot;
    [SerializeField] private List<BlankSlot_UI> _disposalSlots = new List<BlankSlot_UI>();
    [SerializeField] private MouseItemData _mouseInventoryItem;
    [SerializeField] private PlayerInventoryHolder _playerInventoryHolder;
    [SerializeField] private DynamicInventoryDisplay _backPackInventory;
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;
    [SerializeField] private bool _isCanReforge;
    [SerializeField] private Button _reforgeButton;
    [SerializeField] private Button _disposalButton;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _blankCraftValue;
    [SerializeField] private TextMeshProUGUI _catalystCraftValue;
    [Header("Base UI")]
    [SerializeField] private TextMeshProUGUI _basePowerStatsName;
    [SerializeField] private TextMeshProUGUI _basePowerStatsValue;
    [SerializeField] private TextMeshProUGUI _baseDefenceStatsName;
    [SerializeField] private TextMeshProUGUI _baseDefenceStatsValue;
    [Header("Neutral UI")]
    [SerializeField] private TextMeshProUGUI _neutralPowerStatsName;
    [SerializeField] private TextMeshProUGUI _neutralPowerStatsValue;
    [SerializeField] private TextMeshProUGUI _neutralDefenceStatsName;
    [SerializeField] private TextMeshProUGUI _neutralDefenceStatsValue;
    [SerializeField] private Transform _neutralResCont;
    [SerializeField] private Transform _neutralDebuffsCont;
    [Header("Sealed UI")]
    [SerializeField] private TextMeshProUGUI _sealedPowerStatsName;
    [SerializeField] private TextMeshProUGUI _sealedPowerStatsValue;
    [SerializeField] private TextMeshProUGUI _sealedDefenceStatsName;
    [SerializeField] private TextMeshProUGUI _sealedDefenceStatsValue;
    [SerializeField] private Transform _sealedResCont;
    [SerializeField] private Transform _sealedDebuffsCont;
    [Header("Finish Item UI")]
    [SerializeField] private TextMeshProUGUI _finishPowerStats;
    [SerializeField] private TextMeshProUGUI _finishDefenceStats;
    [SerializeField] private Transform _finishResCont;
    [SerializeField] private Transform _finishDebuffsCont;

    private StatDisplayManager _statDisplayManager = new StatDisplayManager();

    public QuestResistanceSystem QuestResistanceSystem { get; private set; }

    public static UnityAction OnReforgePanelOpen;

    public Image ReforgePanel => _reforgePanel;

    private void Awake()
    {
        QuestResistanceSystem = GetComponentInParent<QuestResistanceSystem>();
        _reforgePanel.gameObject.SetActive(false);

        _reforgeButton?.onClick.AddListener(ReforgeItem);
        _disposalButton?.onClick.AddListener(DisposalItems);

        _blankCraftValue.text = "";
        _catalystCraftValue.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ReforgeWindowOpen();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!LearningGameManager.Instance.Panel.gameObject.activeInHierarchy)
                _reforgePanel.gameObject.SetActive(false);
        }
    }

    public void ReforgeWindowOpen()
    {
        _reforgePanel.gameObject.SetActive(true);
        OnReforgePanelOpen?.Invoke();

        ClearInfo();

        if (_blankSlot.BlankSlot != null && _blankSlot.BlankSlot.ItemData != null)
        {
            ShowBaseItemParametres();
            ShowNeutralItemParametres();
            ShowSealedItemParametres();

            _blankCraftValue.text = $"Blank Reforge Spirit: {_blankSlot.BlankSlot.CraftValue}";
        }

        if (_catalystSlot.CatalystSlot != null && _catalystSlot.CatalystSlot.CatalystSlotData != null)
            _catalystCraftValue.text = $"Catalyst Reforge Spirit: {_catalystSlot.CatalystSlot.CatalystSlotData.MinCraftValueSpend} - " +
                $"{_catalystSlot.CatalystSlot.CatalystSlotData.MaxCraftValueSpend}";
    }

    private void ReforgeItem()
    {
        if (_isCanReforge)
        {
            _catalystSlot.CatalystSlot.CatalystSlotData.ApplyCatalystEffect(_blankSlot, _runeSlots, _catalystSlot, this);
            ReforgeWindowOpen();

            _blankSlot.BlankSlot.ApplyReforgeEffect();
            _blankSlot.BlankSlot.AddUsedRunes(_runeSlots);
            _blankSlot.UpdateUISlot(_blankSlot.BlankSlot);
        }
    }

    private void DisposalItems()
    {
        foreach (BlankSlot_UI blankSlot in _disposalSlots)
        {
            if (blankSlot.BlankSlot != null && blankSlot.BlankSlot.BlankSlotData != null)
            {
                int randomRuneIndex = Random.Range(0, blankSlot.BlankSlot.UsedRuneSlots.Count);
                RuneSlot newRuneSlot = new RuneSlot(blankSlot.BlankSlot.UsedRuneSlots[randomRuneIndex], blankSlot.BlankSlot.UsedRuneSlots[randomRuneIndex].Tier);
                _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(newRuneSlot, 1);

                blankSlot.BlankSlot.ClearSlot();
                blankSlot.UpdateUISlot();
            }
        }
    }

    public void CheckSlots()
    {
        if (_blankSlot.BlankSlot.ItemData == null || _catalystSlot.CatalystSlot.ItemData == null || _runeSlots.All(slot => slot.RuneSlot.ItemData == null
        || _blankSlot.BlankSlot.CraftValue <= 0))
        {
            _isCanReforge = false;
            _reforgeButton.image.color = Color.red;
        }

        else
        {
            _isCanReforge = true;
            _reforgeButton.image.color = Color.white;
        }
    }

    private void ShowBaseItemParametres()
    {
        SetStatsInfo(_blankSlot.BlankSlot.BaseStats, _basePowerStatsName, _basePowerStatsValue, _baseDefenceStatsName, _baseDefenceStatsValue);
    }

    private void ShowNeutralItemParametres()
    {
        SetStatsInfo(_blankSlot.BlankSlot.NeutralStats, _neutralPowerStatsName, _neutralPowerStatsValue, _neutralDefenceStatsName, _neutralDefenceStatsValue);
        QuestResistanceSystem.ShowResistance(_blankSlot.BlankSlot.NeutralRes, _neutralResCont);
        QuestResistanceSystem.ShowDebuffs(_blankSlot.BlankSlot.NeutralDebuffs, _neutralDebuffsCont);

    }

    private void ShowSealedItemParametres()
    {
        SetStatsInfo(_blankSlot.BlankSlot.SealedStats, _sealedPowerStatsName, _sealedPowerStatsValue, _sealedDefenceStatsName, _sealedDefenceStatsValue);
        QuestResistanceSystem.ShowResistance(_blankSlot.BlankSlot.SealedRes, _sealedResCont);
        QuestResistanceSystem.ShowDebuffs(_blankSlot.BlankSlot.SealedDebuffs, _sealedDebuffsCont);
    }

    private void ClearInfo()
    {
        _basePowerStatsName.text = "";
        _basePowerStatsValue.text = "";
        _baseDefenceStatsName.text = "";
        _baseDefenceStatsValue.text = "";

        _neutralPowerStatsName.text = "";
        _neutralPowerStatsValue.text = "";
        _neutralDefenceStatsName.text = "";
        _neutralDefenceStatsValue.text = "";

        _sealedPowerStatsName.text = "";
        _sealedPowerStatsValue.text = "";
        _sealedDefenceStatsName.text = "";
        _sealedDefenceStatsValue.text = "";

        QuestResistanceSystem.ClearIcon(_neutralResCont);
        QuestResistanceSystem.ClearIcon(_neutralDebuffsCont);

        QuestResistanceSystem.ClearIcon(_sealedResCont);
        QuestResistanceSystem.ClearIcon(_sealedDebuffsCont);
    }

    private void SetStatsInfo(HeroStats stats, TextMeshProUGUI basePowerName, TextMeshProUGUI basePowerValue,
    TextMeshProUGUI baseDefenceName, TextMeshProUGUI baseDefenceValue)
    {
        (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(stats, false);

        basePowerName.text = statsText.Item1;
        basePowerValue.text = statsText.Item2;

        baseDefenceName.text = statsText.Item3;
        baseDefenceValue.text = statsText.Item4;
    }

    public void ClickBlankSlot(BlankSlot_UI clickedSlot_UI)
    {
        if (_mouseInventoryItem.AssignedInventorySlot.ItemData != null
            && _mouseInventoryItem.AssignedInventorySlot.ItemData.ItemPrimaryType == ItemPrimaryType.Blank)
        {
            // ≈—À» —ÀŒ“ œ”—“, ¿ Ã€ÿ‹ »Ã≈≈“ œ–≈ƒÃ≈“

            if (clickedSlot_UI.BlankSlot.ItemData == null)
            {
                clickedSlot_UI.BlankSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);
                _mouseInventoryItem.ClearSlot();
            }

            else
            {
                BlankSlot clonedSlot = new BlankSlot(clickedSlot_UI.BlankSlot, clickedSlot_UI.BlankSlot.Index);
                clickedSlot_UI.BlankSlot.ClearSlot();
                clickedSlot_UI.ClearSlot();
                clickedSlot_UI.BlankSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);

                _mouseInventoryItem.ClearSlot();
                _mouseInventoryItem.UpdateMouseSlot(clonedSlot);
            }
        }

        else if (clickedSlot_UI.BlankSlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            _mouseInventoryItem.UpdateMouseSlot(clickedSlot_UI.BlankSlot);
            clickedSlot_UI.BlankSlot.ClearSlot();
            clickedSlot_UI.ClearSlot();
        }

        ReforgeWindowOpen();
        CheckSlots();
    }

    public void ClickBlankSlot(BlankSlot blankSlot)
    {

        // ≈—À» —ÀŒ“ œ”—“, ¿ Ã€ÿ‹ »Ã≈≈“ œ–≈ƒÃ≈“

        if (_blankSlot.BlankSlot.BlankSlotData == null)
        {
            _blankSlot.BlankSlot.AssignItem(blankSlot);
            _blankSlot.UpdateUISlot(blankSlot);
            _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
    InventorySlots[blankSlot.Index], blankSlot.StackSize);
        }

        else
        {
            BlankSlot clonedSlot = new BlankSlot(_blankSlot.BlankSlot, _blankSlot.BlankSlot.Index);
            _blankSlot.BlankSlot.ClearSlot();
            _blankSlot.BlankSlot.AssignItem(blankSlot);
            _blankSlot.UpdateUISlot(blankSlot);
            _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
                InventorySlots[blankSlot.Index], blankSlot.StackSize);

            blankSlot.ClearSlot();
            blankSlot.AssignItem(clonedSlot);
            //_playerInventoryHolder.PrimaryInventorySystem.AddToInventory(blankSlot, 1);
        }

        ReforgeWindowOpen();
        CheckSlots();
    }

    public void ClickRuneSlot(RuneSlot runeSlot)
    {
        bool isAdded = false; // ‘Î‡„, ˜ÚÓ ÛÌ‡ ‰Ó·‡‚ÎÂÌ‡

        // 1. œÓ‚ÂˇÂÏ ÒÎÓÚ˚ Ò Ú‡ÍÓÈ ÊÂ ÛÌÓÈ
        for (int i = 0; i < _runeSlots.Count; i++)
        {
            if (_runeSlots[i].RuneSlot != null && _runeSlots[i].RuneSlot.RuneSlotData != null &&
                runeSlot.RuneSlotData == _runeSlots[i].RuneSlot.RuneSlotData)
            {
                if (_runeSlots[i].RuneSlot.RuneSlotData == runeSlot.RuneSlotData)
                {
                    int spaceLeftInClickedSlot = _runeSlots[i].RuneSlot.RuneSlotData.MaxStackSize - _runeSlots[i].RuneSlot.StackSize;

                    if (spaceLeftInClickedSlot >= runeSlot.StackSize)
                    {
                        _runeSlots[i].RuneSlot.AddToStack(runeSlot.StackSize);
                        _runeSlots[i].UpdateUISlot(_runeSlots[i].RuneSlot);

                        _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
        InventorySlots[runeSlot.Index], runeSlot.StackSize);
                    }

                    else
                    {
                        _runeSlots[i].RuneSlot.AddToStack(spaceLeftInClickedSlot);
                        _runeSlots[i].UpdateUISlot(_runeSlots[i].RuneSlot);

                        runeSlot.RemoveFromStack(spaceLeftInClickedSlot);
                    }

                    isAdded = true;
                }

                else
                {
                    RuneSlot clonedSlot = new RuneSlot(_runeSlots[i].RuneSlot, _runeSlots[i].RuneSlot.Index);
                    _runeSlots[i].RuneSlot.ClearSlot();
                    _runeSlots[i].RuneSlot.AssignItem(runeSlot);
                    _runeSlots[i].UpdateUISlot(runeSlot);
                    _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
                        InventorySlots[runeSlot.Index], runeSlot.StackSize);

                    runeSlot.ClearSlot();
                    runeSlot.AssignItem(clonedSlot);
                    isAdded = true;
                }
                // ≈ÒÎË ÂÒÚ¸ ÏÂÒÚÓ ‚ ÒÚ‡ÍÂ
                // if (_runeSlots[i].RuneSlot.StackSize < _runeSlots[i].RuneSlot.RuneSlotData.MaxStackSize)
                // {
                //     _runeSlots[i].RuneSlot.AddToStack(runeSlot.StackSize);
                //     _runeSlots[i].UpdateUISlot(_runeSlots[i].RuneSlot);
                //     isAdded = true;
                //     _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
                //InventorySlots[runeSlot.Index], runeSlot.StackSize);
                //     break; // ¬˚ıÓ‰ËÏ ËÁ ˆËÍÎ‡ ÔÓÒÎÂ ‰Ó·‡‚ÎÂÌËˇ
                // }
            }
        }

        // 2. ≈ÒÎË ÌÂ Ì‡¯ÎË ÔÓ‰ıÓ‰ˇ˘ËÈ ÒÚ‡Í, Ë˘ÂÏ ÔÛÒÚÓÈ ÒÎÓÚ
        if (!isAdded)
        {
            for (int i = 0; i < _runeSlots.Count; i++)
            {
                if (_runeSlots[i].RuneSlot == null || _runeSlots[i].RuneSlot.RuneSlotData == null)
                {
                    // ƒÓ·‡‚ÎˇÂÏ ÛÌÛ ‚ ÔÛÒÚÓÈ ÒÎÓÚ
                    _runeSlots[i].RuneSlot.AssignItem(runeSlot);
                    _runeSlots[i].UpdateUISlot(runeSlot);

                    _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
             InventorySlots[runeSlot.Index], runeSlot.StackSize);
                    isAdded = true;
                    break;
                }
            }
        }

        // 3. ≈ÒÎË ‚Ò∏ Á‡ÌˇÚÓ ó ‚˚‚Ó‰ËÏ ÔÂ‰ÛÔÂÊ‰ÂÌËÂ
        if (!isAdded)
        {
            Debug.LogWarning("ÕÂÚ Ò‚Ó·Ó‰Ì˚ı ÒÎÓÚÓ‚ ‰Îˇ ÛÌ˚!");
        }

        ReforgeWindowOpen();
        CheckSlots();
    }


    public void ClickRuneSlot(RuneSlot_UI clickedSlot_UI)
    {
        if (_mouseInventoryItem.AssignedInventorySlot.ItemData != null
            && _mouseInventoryItem.AssignedInventorySlot.ItemData.ItemPrimaryType == ItemPrimaryType.Rune)
        {
            // ≈—À» —ÀŒ“ œ”—“, ¿ Ã€ÿ‹ »Ã≈≈“ œ–≈ƒÃ≈“

            if (clickedSlot_UI.RuneSlot.ItemData == null)
            {
                clickedSlot_UI.RuneSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);
                _mouseInventoryItem.ClearSlot();
            }

            // ≈—À» —ÀŒ“ » Ã€ÿ‹ »Ã≈ﬁ“ –¿«Õ€≈ œ–≈ƒÃ≈“€

            else if (clickedSlot_UI.RuneSlot.ItemData != _mouseInventoryItem.AssignedInventorySlot.ItemData)
            {
                InventorySlot clonedSlot = new InventorySlot(clickedSlot_UI.RuneSlot.ItemData, clickedSlot_UI.RuneSlot.StackSize);
                clickedSlot_UI.RuneSlot.ClearSlot();
                clickedSlot_UI.ClearSlot();
                clickedSlot_UI.RuneSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);

                _mouseInventoryItem.ClearSlot();
                _mouseInventoryItem.UpdateMouseSlot(clonedSlot);
            }

            // ≈—À» —ÀŒ“ » Ã€ÿ‹ »Ã≈ﬁ“ Œƒ»Õ¿ Œ¬€≈ œ–≈ƒÃ≈“€

            else if (clickedSlot_UI.RuneSlot.ItemData == _mouseInventoryItem.AssignedInventorySlot.ItemData)
            {
                int totalAmountStack = clickedSlot_UI.RuneSlot.StackSize + _mouseInventoryItem.AssignedInventorySlot.StackSize;

                if (totalAmountStack < clickedSlot_UI.RuneSlot.ItemData.MaxStackSize)
                {
                    clickedSlot_UI.RuneSlot.AddToStack(_mouseInventoryItem.AssignedInventorySlot.StackSize);
                    clickedSlot_UI.UpdateUISlot(clickedSlot_UI.RuneSlot);

                    _mouseInventoryItem.ClearSlot();
                }

                else if (clickedSlot_UI.RuneSlot.StackSize == clickedSlot_UI.RuneSlot.ItemData.MaxStackSize)
                {
                    InventorySlot clonedSlot = new InventorySlot(clickedSlot_UI.RuneSlot.ItemData, clickedSlot_UI.RuneSlot.StackSize);
                    clickedSlot_UI.RuneSlot.ClearSlot();
                    clickedSlot_UI.ClearSlot();
                    clickedSlot_UI.RuneSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                    clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);

                    _mouseInventoryItem.ClearSlot();
                    _mouseInventoryItem.UpdateMouseSlot(clonedSlot);
                }

                else if (totalAmountStack >= clickedSlot_UI.RuneSlot.ItemData.MaxStackSize)
                {
                    int leftOnMouse = totalAmountStack - clickedSlot_UI.RuneSlot.ItemData.MaxStackSize;

                    clickedSlot_UI.RuneSlot.AddToStack(clickedSlot_UI.RuneSlot.ItemData.MaxStackSize - clickedSlot_UI.RuneSlot.StackSize);
                    clickedSlot_UI.UpdateUISlot(clickedSlot_UI.RuneSlot);

                    InventorySlot clonedSlot = new InventorySlot(clickedSlot_UI.RuneSlot.ItemData, leftOnMouse);
                    _mouseInventoryItem.ClearSlot();
                    _mouseInventoryItem.UpdateMouseSlot(clonedSlot);
                }
            }
        }

        else if (clickedSlot_UI.RuneSlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            _mouseInventoryItem.UpdateMouseSlot(clickedSlot_UI.RuneSlot);
            clickedSlot_UI.RuneSlot.ClearSlot();
            clickedSlot_UI.ClearSlot();
        }

        CheckSlots();
    }

    public void ClickCatalystSlot(CatalystSlot_UI clickedSlot_UI)
    {
        if (_mouseInventoryItem.AssignedInventorySlot.ItemData != null
            && _mouseInventoryItem.AssignedInventorySlot.ItemData.ItemPrimaryType == ItemPrimaryType.Catalyst)
        {
            // ≈—À» —ÀŒ“ œ”—“, ¿ Ã€ÿ‹ »Ã≈≈“ œ–≈ƒÃ≈“

            if (clickedSlot_UI.CatalystSlot.ItemData == null)
            {
                clickedSlot_UI.CatalystSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);
                _mouseInventoryItem.ClearSlot();
            }

            // ≈—À» —ÀŒ“ » Ã€ÿ‹ »Ã≈ﬁ“ –¿«Õ€≈ œ–≈ƒÃ≈“€

            else if (clickedSlot_UI.CatalystSlot.ItemData != _mouseInventoryItem.AssignedInventorySlot.ItemData)
            {
                InventorySlot clonedSlot = new InventorySlot(clickedSlot_UI.CatalystSlot.ItemData, clickedSlot_UI.CatalystSlot.StackSize);
                clickedSlot_UI.CatalystSlot.ClearSlot();
                clickedSlot_UI.ClearSlot();
                clickedSlot_UI.CatalystSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);

                _mouseInventoryItem.ClearSlot();
                _mouseInventoryItem.UpdateMouseSlot(clonedSlot);
            }

            // ≈—À» —ÀŒ“ » Ã€ÿ‹ »Ã≈ﬁ“ Œƒ»Õ¿ Œ¬€≈ œ–≈ƒÃ≈“€

            else if (clickedSlot_UI.CatalystSlot.ItemData == _mouseInventoryItem.AssignedInventorySlot.ItemData)
            {
                int totalAmountStack = clickedSlot_UI.CatalystSlot.StackSize + _mouseInventoryItem.AssignedInventorySlot.StackSize;

                if (totalAmountStack < clickedSlot_UI.CatalystSlot.ItemData.MaxStackSize)
                {
                    clickedSlot_UI.CatalystSlot.AddToStack(_mouseInventoryItem.AssignedInventorySlot.StackSize);
                    clickedSlot_UI.UpdateUISlot(clickedSlot_UI.CatalystSlot);

                    _mouseInventoryItem.ClearSlot();
                }

                else if (clickedSlot_UI.CatalystSlot.StackSize == clickedSlot_UI.CatalystSlot.ItemData.MaxStackSize)
                {
                    InventorySlot clonedSlot = new InventorySlot(clickedSlot_UI.CatalystSlot.ItemData, clickedSlot_UI.CatalystSlot.StackSize);
                    clickedSlot_UI.CatalystSlot.ClearSlot();
                    clickedSlot_UI.ClearSlot();
                    clickedSlot_UI.CatalystSlot.AssignItem(_mouseInventoryItem.AssignedInventorySlot);
                    clickedSlot_UI.UpdateUISlot(_mouseInventoryItem.AssignedInventorySlot);

                    _mouseInventoryItem.ClearSlot();
                    _mouseInventoryItem.UpdateMouseSlot(clonedSlot);
                }

                else if (totalAmountStack >= clickedSlot_UI.CatalystSlot.ItemData.MaxStackSize)
                {
                    int leftOnMouse = totalAmountStack - clickedSlot_UI.CatalystSlot.ItemData.MaxStackSize;

                    clickedSlot_UI.CatalystSlot.AddToStack(clickedSlot_UI.CatalystSlot.ItemData.MaxStackSize - clickedSlot_UI.CatalystSlot.StackSize);
                    clickedSlot_UI.UpdateUISlot(clickedSlot_UI.CatalystSlot);

                    InventorySlot clonedSlot = new InventorySlot(clickedSlot_UI.CatalystSlot.ItemData, leftOnMouse);
                    _mouseInventoryItem.ClearSlot();
                    _mouseInventoryItem.UpdateMouseSlot(clonedSlot);
                }
            }
        }

        else if (clickedSlot_UI.CatalystSlot.ItemData != null && _mouseInventoryItem.AssignedInventorySlot.ItemData == null)
        {
            _mouseInventoryItem.UpdateMouseSlot(clickedSlot_UI.CatalystSlot);
            clickedSlot_UI.CatalystSlot.ClearSlot();
            clickedSlot_UI.ClearSlot();
        }

        ReforgeWindowOpen();
        CheckSlots();
    }

    public void ClickCatalystSlot(CatalystSlot catalystSlot)
    {

        // ≈—À» —ÀŒ“ œ”—“, ¿ Ã€ÿ‹ »Ã≈≈“ œ–≈ƒÃ≈“

        if (_catalystSlot.CatalystSlot.CatalystSlotData == null)
        {
            _catalystSlot.CatalystSlot.AssignItem(catalystSlot);
            _catalystSlot.UpdateUISlot(catalystSlot);
            _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
    InventorySlots[catalystSlot.Index], catalystSlot.StackSize);
        }

        else
        {
            if (_catalystSlot.CatalystSlot.CatalystSlotData != catalystSlot.CatalystSlotData)
            {
                CatalystSlot clonedSlot = new CatalystSlot(_catalystSlot.CatalystSlot, _catalystSlot.CatalystSlot.Index);
                _catalystSlot.CatalystSlot.ClearSlot();
                _catalystSlot.CatalystSlot.AssignItem(catalystSlot);
                _catalystSlot.UpdateUISlot(catalystSlot);
                _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
                    InventorySlots[catalystSlot.Index], catalystSlot.StackSize);

                catalystSlot.ClearSlot();
                catalystSlot.AssignItem(clonedSlot);
            }

            else
            {
                int spaceLeftInClickedSlot = _catalystSlot.CatalystSlot.CatalystSlotData.MaxStackSize - _catalystSlot.CatalystSlot.StackSize;

                if (spaceLeftInClickedSlot >= catalystSlot.StackSize)
                {
                    _catalystSlot.CatalystSlot.AddToStack(catalystSlot.StackSize);
                    _catalystSlot.UpdateUISlot(_catalystSlot.CatalystSlot);

                    _playerInventoryHolder.PrimaryInventorySystem.RemoveItemsFromInventory(_playerInventoryHolder.PrimaryInventorySystem.
    InventorySlots[catalystSlot.Index], catalystSlot.StackSize);
                }

                else
                {
                    _catalystSlot.CatalystSlot.AddToStack(spaceLeftInClickedSlot);
                    _catalystSlot.UpdateUISlot(_catalystSlot.CatalystSlot);

                    catalystSlot.RemoveFromStack(spaceLeftInClickedSlot);
                }
            }

        }

        _backPackInventory.RefreshDynamicInventory(_playerInventoryHolder.PrimaryInventorySystem, 0);
        ReforgeWindowOpen();
        CheckSlots();
    }

    public void RightClickSlot(InventorySlot_UI slot)
    {
        if (slot is BlankSlot_UI blankSlotUI)
        {
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(blankSlotUI.BlankSlot, 1);
            blankSlotUI.BlankSlot.ClearSlot();
            blankSlotUI.UpdateUISlot();
        }

        else if (slot is RuneSlot_UI runeSlotUI)
        {
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(runeSlotUI.RuneSlot, runeSlotUI.RuneSlot.StackSize);
            runeSlotUI.RuneSlot.ClearSlot();
            runeSlotUI.UpdateUISlot();
        }

        else if (slot is CatalystSlot_UI catalystSlotUI)
        {
            _playerInventoryHolder.PrimaryInventorySystem.AddToInventory(catalystSlotUI.CatalystSlot, catalystSlotUI.CatalystSlot.StackSize);
            catalystSlotUI.CatalystSlot.ClearSlot();
            catalystSlotUI.UpdateUISlot();
        }

        _backPackInventory.RefreshDynamicInventory(_playerInventoryHolder.PrimaryInventorySystem, 0);
        ReforgeWindowOpen();
        CheckSlots();
    }

}
