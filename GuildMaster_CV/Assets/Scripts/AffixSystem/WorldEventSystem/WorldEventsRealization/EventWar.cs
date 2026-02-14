using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWar : WorldEvent
{
    public EventWar(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem) : base(worldEventData, worldEventApplySystem)
    {

    }

    public override void ApplyEvent(WorldEventData data)
    {
        if (data is EventWarData warData)
        {
            _worldEventApplySystem.RecruitmentObject.ChangeCoefHire(-warData.CoefHire);
        }
    }

    public override void CancelEvent(WorldEventData data)
    {
        if (data is EventWarData warData)
        {
            _worldEventApplySystem.RecruitmentObject.ChangeCoefHire(warData.CoefHire);
        }
    }
}
