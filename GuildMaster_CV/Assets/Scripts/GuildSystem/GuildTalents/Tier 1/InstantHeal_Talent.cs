using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantHeal_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private HospitalUIController _hospitalController;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _hospitalController.InstantHealByTalent();
    }
}
