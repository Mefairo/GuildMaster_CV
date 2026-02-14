using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceCostAllShops_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefCost;
    [SerializeField] private List<ShopKeeper> _shopKeeperList;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        foreach (ShopKeeper shopKeeper in _shopKeeperList)
        {
            shopKeeper.ShopSystem.ChangeBuyMarkUp(-_coefCost / 100);
        }
    }
}
