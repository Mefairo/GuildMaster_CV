using UnityEngine;

public abstract class Slot
{

    [SerializeField] protected SlotData _itemData;
    [SerializeField] protected int _stackSize;
    [SerializeField] protected int _itemID = -1;

    [SerializeField] protected Sprite _iconSlot;
    //[SerializeField] protected EquipSlotData _equipSlot;

    public SlotData ItemData => _itemData;
    //public EquipSlotData EquipSlot => _equipSlot;
    public int StackSize => _stackSize;
    public Sprite IconSlot => _iconSlot;

    public virtual void ClearSlot()
    {
        _itemData = null;
        _itemID = -1;
        _stackSize = -1;
        _iconSlot = null;
    }

    public virtual void AssignItem(InventorySlot invSlot)
    {
        if (_itemData == invSlot.ItemData)
            AddToStack(invSlot._stackSize);

        else
        {
            _itemData = invSlot._itemData;
            _itemID = _itemData.ID;
            _stackSize = 0;

            SetIconSlot(invSlot.IconSlot);

            AddToStack(invSlot._stackSize);
        }
    }

    public virtual void AssignItem(SlotData data, int amount)
    {
        if (_itemData == data)
            AddToStack(amount);

        else
        {
            _itemData = data;
            _itemID = data.ID;
            _stackSize = 0;
            AddToStack(amount);

            SetIconSlot(data.Icon);
        }
    }

    public void AddToStack(int amount)
    {
        _stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        _stackSize -= amount;
        if (_stackSize <= 0)
            ClearSlot();
    }

    public void SwapStack(int amount)
    {
        _stackSize = amount;
    }

    public void SplitStackSize(float coef)
    {
        _stackSize = (int)(_stackSize * coef);
    }

    public void SetIconSlot(Sprite icon)
    {
        _iconSlot = icon;
    }


    //public void OnBeforeSerialize()
    //{

    //}

    //public void OnAfterDeserialize()
    //{
    //    //if (_itemID == -1)
    //    //    return;

    //    //var db = Resources.Load<Database>("Database");
    //    //itemData = db.GetItem(_itemID);
    //}


}
