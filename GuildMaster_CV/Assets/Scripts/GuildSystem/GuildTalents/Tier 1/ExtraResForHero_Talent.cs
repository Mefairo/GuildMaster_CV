using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraResForHero_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private int _resValue;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private RecruitKeeper _recruitKeeper;

    public int ResValue => _resValue;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        //_guildKeeper.ExtraResForHeroesByTalent(_resValue);
        //_recruitKeeper.ChangeResValueCoefByTalent(_resValue);
    }
}
