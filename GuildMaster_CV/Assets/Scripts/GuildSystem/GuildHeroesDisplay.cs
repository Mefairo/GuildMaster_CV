using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuildHeroesDisplay : KeeperDisplay
{
    [SerializeField] protected Button _kickHeroButton;
    [SerializeField] protected SendHeroesSlots _heroesQuestSlots;
    [SerializeField] protected RecruitQuestKeeperController _recruitQuestKeeperController;
    [Header("HeroExp")]
    [SerializeField] private SliderBar _expSliderBar;

    public StatsUpSystem StatsUpSystem { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StatsUpSystem = GetComponent<StatsUpSystem>();
        _kickHeroButton?.onClick.AddListener(KickHero);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        ClearExpInfo();
    }

    public override void SelectHero(HeroSlot_UI heroSlot)
    {
        base.SelectHero(heroSlot);

        if (StatsUpSystem == null)
            Debug.Log("null");

        else
        {
            StatsUpSystem.SelectHero(heroSlot);
            UpdateHeroExp(heroSlot.Hero);
        }
    }

    private void KickHero()
    {
        if (_selectedHero != null && _selectedHero.Hero != null)
        {
            if (_selectedHero.Hero.IsSentOnQuest)
                return;

            Hero foundHero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(hero => hero == _selectedHero.Hero);
            foundHero.UnsubEvents();

            _guildKeeper.RecruitSystem.HeroSlots.Remove(foundHero);

            _guildValutes.ChangeReputationForKick(foundHero);
            _guildValutes.KickHero(1);

            foreach (HeroQuestSlot_UI heroSlot in _heroesQuestSlots.Slots)
            {
                heroSlot.DeleteHero();
            }

            _recruitQuestKeeperController.UpdateHeroList();
            _heroesQuestSlots.ClearAmountActiveSlots();

            //RefreshDisplay();
            RefreshInfo();
            DisplayHeroes();

            //UpdateUI();
            NotificationSystem.Instance.CheckNotifications();
        }
    }

    protected void UpdateHeroExp(Hero hero)
    {
        _expSliderBar.gameObject.SetActive(true);

        _expSliderBar.MainSlider.maxValue = hero.LevelSystem.RequiredExp;
        _expSliderBar.MainSlider.value = hero.LevelSystem.CurrentExp;

        _expSliderBar.TextPlace.text = $"Exp: {hero.LevelSystem.CurrentExp} / {hero.LevelSystem.RequiredExp}";
    }

    protected void ClearExpInfo()
    {
        if (_expSliderBar != null)
        {
            _expSliderBar.gameObject.SetActive(false);
            _expSliderBar.MainSlider.value = 0;
            _expSliderBar.MainSlider.enabled = false;
        }
    }
}
