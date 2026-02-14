using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceHealCost_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefCost;
    [SerializeField] private HospitalUIController _hospitalController;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _hospitalController.ChangeHealCostTalent(-_coefCost / 100);
    }
}
