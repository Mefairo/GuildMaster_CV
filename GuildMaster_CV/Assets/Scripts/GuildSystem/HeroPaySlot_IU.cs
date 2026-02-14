using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeroPaySlot_IU : HeroSlot_UI
{
    [SerializeField] protected TextMeshProUGUI _pay;

    public WeeklyPaySystem WeeklyPaySystem { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        WeeklyPaySystem = GetComponentInParent<WeeklyPaySystem>();
    }

    public override void Init(Hero hero)
    {
        base.Init(hero);
    }

    public override void UpdateUISlot()
    {
        base.UpdateUISlot();

        _heroLevel.text = $"Level: {Hero.HeroLevel}";
        _pay.text = $"Pay: {_hero.WeeklyPay}";
    }

    public override void UpdateItemPreview()
    {
       if(WeeklyPaySystem != null)
        {
            WeeklyPaySystem.SelectHero(this);
        }
    }
}
