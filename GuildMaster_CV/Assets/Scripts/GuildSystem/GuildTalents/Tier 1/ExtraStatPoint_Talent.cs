using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraStatPoint_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private int _extraStat;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private RecruitKeeper _recruitKeeper;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        //_guildKeeper.ExtraStatPointForLevelByTalent(_extraStat);
        //_recruitKeeper.ExtraStatPointForLevelByTalent(_extraStat);
    }
}
