using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraHPHero_Talent : GuildTalentSlot_UI
{
    [Header("Talent Attributes")]
    [SerializeField] private int _extraHP;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private RecruitKeeper _recruitKeeper;

    public int ExtraHP => _extraHP;

    public override void RealizationTalent()
    {
        base.RealizationTalent();
        //_guildKeeper.ExtraHPHeroesByTalent(_extraHP);
        //_recruitKeeper.ExtraHPHeroesByTalent(_extraHP);
    }
}
