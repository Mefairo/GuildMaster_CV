using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCrisis : WorldEvent
{
    private ShopKeeper _shopKeeper;

    public EventCrisis(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem) : base(worldEventData, worldEventApplySystem)
    {

    }

    public EventCrisis(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem, ShopKeeper shopKeeper) : base(worldEventData, worldEventApplySystem)
    {
        _shopKeeper = shopKeeper;
    }

    public override void ApplyEvent(WorldEventData data)
    {
        if (data is EventCrisisData crisisData)
        {
            _shopKeeper.ShopSystem.ChangeBuyMarkUp(crisisData.CoefBuyCost / 100);
            _shopKeeper.ShopSystem.ChangeSellMarkUp(crisisData.CoefSellCost / 100);

            _shopKeeper.ChangeCoefAmountItems(-crisisData.CoefAmountItems / 100);
        }
    }

    public override void CancelEvent(WorldEventData data)
    {
        if (data is EventCrisisData crisisData)
        {
            _shopKeeper.ShopSystem.ChangeBuyMarkUp(-crisisData.CoefBuyCost / 100);
            _shopKeeper.ShopSystem.ChangeSellMarkUp(-crisisData.CoefSellCost / 100);

            _shopKeeper.ChangeCoefAmountItems(crisisData.CoefAmountItems / 100);
        }
    }
}
