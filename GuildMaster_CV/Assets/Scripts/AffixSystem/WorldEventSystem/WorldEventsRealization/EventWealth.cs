using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventWealth : WorldEvent
{
    private ShopKeeper _shopKeeper;

    public EventWealth(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem) : base(worldEventData, worldEventApplySystem)
    {

    }

    public EventWealth(WorldEventData worldEventData, WorldEventApplySystem worldEventApplySystem, ShopKeeper shopKeeper) : base(worldEventData, worldEventApplySystem)
    {
        _shopKeeper = shopKeeper;
    }

    public override void ApplyEvent(WorldEventData data)
    {
        if (data is EventWealthData wealthData)
        {
            _shopKeeper.ShopSystem.ChangeBuyMarkUp(-wealthData.CoefBuyCost / 100);
            _shopKeeper.ShopSystem.ChangeSellMarkUp(-wealthData.CoefSellCost / 100);

            _shopKeeper.ChangeCoefAmountItems(wealthData.CoefAmountItems / 100);
        }
    }

    public override void CancelEvent(WorldEventData data)
    {
        if (data is EventWealthData wealthData)
        {
            _shopKeeper.ShopSystem.ChangeBuyMarkUp(wealthData.CoefBuyCost / 100);
            _shopKeeper.ShopSystem.ChangeSellMarkUp(wealthData.CoefSellCost / 100);

            _shopKeeper.ChangeCoefAmountItems(-wealthData.CoefAmountItems / 100);
        }
    }
}
