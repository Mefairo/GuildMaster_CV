using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceShopCosts : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefCost;
    [SerializeField] private ShopKeeper _shopKeeper;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _shopKeeper.ShopSystem.ChangeBuyMarkUp(-_coefCost / 100);
    }
}
