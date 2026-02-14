using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventQuestResist : WorldEvent
{
    public EventQuestResist(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem) : base(worldEventData, worldEventApplySystem)
    {
    }

    public override void ApplyEvent(WorldEventData data)
    {
        if (data is EventQuestResistData resData)
        {
            foreach (TypeDamage res in resData.eventQuestResists)
            {
                if (!_worldEventApplySystem.WorldEventSystem.EventResList.Contains(res))
                    _worldEventApplySystem.WorldEventSystem.EventResList.Add(res);
            }
        }
    }

    public override void CancelEvent(WorldEventData data)
    {
        if (data is EventQuestResistData resData)
        {
            foreach (TypeDamage res in resData.eventQuestResists)
                _worldEventApplySystem.WorldEventSystem.EventResList.Remove(res);
        }
    }
}
