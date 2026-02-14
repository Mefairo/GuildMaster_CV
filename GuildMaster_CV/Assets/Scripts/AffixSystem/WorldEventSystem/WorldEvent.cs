using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class WorldEvent
{
    [SerializeField] protected WorldEventData _eventData;
    [SerializeField] protected int _amountDays;
    [SerializeField] protected int _leftDays;
    [SerializeField] protected float _value_1;
    [SerializeField] protected bool _isAffix = false;

    protected WorldEventApplySystem _worldEventApplySystem;

    public WorldEventData EventData => _eventData;
    public int AmountDays => _amountDays;
    public int LeftDays => _leftDays;
    public float Value_1 => _value_1;
    public bool IsAffix => _isAffix;


    public WorldEvent(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem)
    {
        _eventData = worldEventData;
        _amountDays = worldEventData.AmountDays;
        _leftDays = _amountDays;
        _value_1 = worldEventData.Value_1;
        _isAffix = worldEventData.IsAffix;

        _worldEventApplySystem = worldEventApplySystem;
    }

    public void NextDay()
    {
        _leftDays--;
    }

    public abstract void ApplyEvent(WorldEventData data);

    public abstract void CancelEvent(WorldEventData data);
}
