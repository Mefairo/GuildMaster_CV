using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EquipSlot : InventorySlot
{
    //[SerializeField] public EquipType EquipType;
    [SerializeField] private EquipItemData _equipItemData;
    [SerializeField] protected List<HeroStats> _itemStats = new List<HeroStats>();
    [SerializeField] protected List<ResistanceList> _itemDefRes = new List<ResistanceList>();
    [SerializeField] protected List<DebuffList> _itemDefDebuffs = new List<DebuffList>();
    [SerializeField] protected List<Debuff> _itemDefDebuffsData = new List<Debuff>();
    [SerializeField] protected List<AffixList> _itemDefAffix = new List<AffixList>();
    [SerializeField] protected List<BuffList> _itemBuffs = new List<BuffList>();
    [SerializeField] protected int _equipIndex = -1;

    public override SlotData BaseItemData => _equipItemData;

    public EquipItemData EquipItemData => _equipItemData;
    public List<HeroStats> ItemStats => _itemStats;
    public List<ResistanceList> ItemDefRes => _itemDefRes;
    public List<DebuffList> ItemDefDebuffs => _itemDefDebuffs;
    public List<Debuff> ItemDefDebuffsData => _itemDefDebuffsData;
    public List<AffixList> ItemDefAffix => _itemDefAffix;
    public List<BuffList> ItemBuffs => _itemBuffs;
    public int EquipIndex
    {
        get => _equipIndex;
        set
        {
            if (_equipIndex != value)
            {
                Debug.Log($"HasteValue изменяется с {_equipIndex} на {value}. " +
                         $"Вызов из: {GetCallerInfo()}");
                _equipIndex = value;
            }
        }

    }

    public event UnityAction OnClearSlot;

    private string GetCallerInfo()
    {
        var stackTrace = new System.Diagnostics.StackTrace();
        if (stackTrace.FrameCount > 1)
        {
            var frame = stackTrace.GetFrame(2); // Берем предыдущий кадр
            var method = frame.GetMethod();
            return $"{method.DeclaringType.Name}.{method.Name} (строка: {frame.GetFileLineNumber()})";
        }
        return "unknown";
    }

    public EquipSlot(EquipItemData equipItemData)
    {
        _equipItemData = equipItemData;
        _stackSize = 1;

        _itemStats = new List<HeroStats>();
        _itemDefRes = new List<ResistanceList>();
        _itemDefDebuffs = new List<DebuffList>();
        _itemDefDebuffsData = new List<Debuff>();
        _itemDefAffix = new List<AffixList>();
        _itemBuffs = new List<BuffList>();

        _itemStats = DeepCopyStats(equipItemData.Stats);
        _itemDefRes = DeepCopyResistances(equipItemData.Resistance);
        _itemDefDebuffs = DeepCopyDebuffs(equipItemData.NeutralizeDebuffs);
        _itemDefDebuffsData = DeepCopyDebuffs(equipItemData.NeutralizeDebuffsData);
        _itemBuffs = DeepCopyBuffs(equipItemData.Buffs);
        _itemDefAffix = DeepCopyAffixes(equipItemData.NeutralizeAffixes);

        _itemData = equipItemData;
        _itemID = equipItemData.ID;

        _iconSlot = equipItemData.Icon;
    }

    public EquipSlot(EquipSlot equipSlot, int index)
    {
        _equipItemData = equipSlot.EquipItemData;
        _stackSize = equipSlot.StackSize;
        _index = index;

        if (equipSlot == null)
            Debug.Log("null 1");

        if (equipSlot.EquipItemData == null)
            Debug.Log("null 2");

        if (equipSlot.EquipItemData.Stats == null)
            Debug.Log("null 3");

        _itemStats = DeepCopyStats(equipSlot.EquipItemData.Stats);
        _itemDefRes = DeepCopyResistances(equipSlot.EquipItemData.Resistance);
        _itemDefDebuffs = DeepCopyDebuffs(equipSlot.EquipItemData.NeutralizeDebuffs);
        _itemDefDebuffsData = DeepCopyDebuffs(equipSlot.EquipItemData.NeutralizeDebuffsData);
        _itemBuffs = DeepCopyBuffs(equipSlot.EquipItemData.Buffs);
        _itemDefAffix = DeepCopyAffixes(equipSlot.EquipItemData.NeutralizeAffixes);

        //_equipIndex = equipSlot.EquipIndex;

        _itemData = equipSlot.BaseItemData;
        _itemID = equipSlot.BaseItemData.ID;

        _iconSlot = equipSlot.IconSlot;
    }

    public EquipSlot(int equipIndex)
    {
        _equipIndex = equipIndex;
    }

    public EquipSlot() { }

    public void SetEquipIndex(int equipIndex)
    {
        _equipIndex = equipIndex;
    }

    public void SetEquipItemData(EquipItemData data)
    {
        _equipItemData = data;

        //if (data is BlankSlotData blankData)
        //{
        //    Debug.Log("set Data 2");
        //    _equipItemData = blankData;
        //}

        //else
        //{
        //    Debug.Log("set Data 3");
        //    _equipItemData = data;
        //}
    }

    public override void AssignItem(InventorySlot slot)
    {
        //base.AssignItem(slot);
        _itemData = slot.BaseItemData;
        _itemID = slot.BaseItemData.ID;
        _stackSize = slot.StackSize;

        _iconSlot = slot.IconSlot;

        if (slot is EquipSlot equip)
        {
            _equipItemData = equip.EquipItemData;
            _itemStats = DeepCopyStats(equip.ItemStats);
            _itemDefRes = DeepCopyResistances(equip._itemDefRes);
            _itemDefDebuffs = DeepCopyDebuffs(equip._itemDefDebuffs);
            _itemDefDebuffsData = DeepCopyDebuffs(equip._itemDefDebuffsData);
            _itemBuffs = DeepCopyBuffs(equip._itemBuffs);
            _itemDefAffix = DeepCopyAffixes(equip._itemDefAffix);

            //_equipIndex = equip.EquipIndex;
        }

        else
            _equipItemData = null;
    }

    public override void AssignItem(SlotData data, int amount)
    {
        _itemData = data;
        _itemID = data.ID;
        _stackSize = 0;
        AddToStack(amount);
    }

    public override void ClearSlot()
    {
        base.ClearSlot();

        _equipItemData = null;
        _itemStats = new List<HeroStats>();
        _itemDefRes = new List<ResistanceList>();
        _itemDefDebuffs = new List<DebuffList>();
        _itemDefDebuffsData = new List<Debuff>();
        _itemBuffs = new List<BuffList>();
        _itemDefAffix = new List<AffixList>();

        //OnClearSlot?.Invoke();
    }

    private List<HeroStats> DeepCopyStats(List<HeroStats> original)
    {
        List<HeroStats> copy = new List<HeroStats>();

        foreach (HeroStats stat in original)
        {
            HeroStats newHeroStats = new HeroStats(stat);
            copy.Add(newHeroStats);
        }

        return copy;
    }

    private List<ResistanceList> DeepCopyResistances(List<ResistanceList> original)
    {
        List<ResistanceList> copy = new List<ResistanceList>();
        foreach (ResistanceList resistance in original)
        {
            ResistanceList newResistance = new ResistanceList(resistance);
            copy.Add(newResistance);
        }
        return copy;
    }

    private List<DebuffList> DeepCopyDebuffs(List<DebuffList> original)
    {
        List<DebuffList> copy = new List<DebuffList>();

        foreach (DebuffList debuff in original)
            copy.Add(debuff);

        return copy;
    }

    private List<Debuff> DeepCopyDebuffs(List<Debuff> original)
    {
        List<Debuff> copy = new List<Debuff>();

        foreach (Debuff debuff in original)
            copy.Add(debuff);

        return copy;
    }

    private List<BuffList> DeepCopyBuffs(List<BuffList> original)
    {
        List<BuffList> copy = new List<BuffList>();

        foreach (BuffList buff in original)
            copy.Add(buff);

        return copy;
    }

    private List<AffixList> DeepCopyAffixes(List<AffixList> original)
    {
        List<AffixList> copy = new List<AffixList>();

        foreach (AffixList buff in original)
            copy.Add(buff);

        return copy;
    }
}
