using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreRecruitHeroes_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private float _coefHire;
    [SerializeField] private RecruitKeeper _recruitKeeper;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        _recruitKeeper.ChangeCoefHire(_coefHire / 100);
    }
}
