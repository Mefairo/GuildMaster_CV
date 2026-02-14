using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceCostExpedition_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefCost;
    [SerializeField] private ExpeditionalCompany _expeditionalCompany;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _expeditionalCompany.ChangeCostExpedionalTalent(-_coefCost / 100);
    }
}
