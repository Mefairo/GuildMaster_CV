using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class StatsUpSystem : MonoBehaviour
{
    [SerializeField] private GuildKeeper _guildKeeper;
    [Header("Buttons")]
    [Space]
    [SerializeField] private List<Button> _listPowerStats = new List<Button>();
    [SerializeField] private List<Button> _listDefenceStats = new List<Button>();

    public KeeperDisplay KeeperDisplay { get; private set; }

    public static event UnityAction OnOverStatPoints;

    private void Awake()
    {
        KeeperDisplay = GetComponent<KeeperDisplay>();

        for (int i = 0; i < _listPowerStats.Count; i++)
        {
            int index = i; // Создаем локальную копию, чтобы захватить текущее значение
            _listPowerStats[i]?.onClick.AddListener(() => UpPowerStats(index));
        }

        for (int i = 0; i < _listDefenceStats.Count; i++)
        {
            int index = i; // Создаем локальную копию, чтобы захватить текущее значение
            _listDefenceStats[i]?.onClick.AddListener(() => UpDefenceStats(index));
        }

        DisableButtons();
    }

    public void SelectHero(HeroSlot_UI heroSlot)
    {
        if (heroSlot.Hero.LevelSystem.StatPoints > 0)
            EnableButtons();

        else
            DisableButtons();
    }

    private Hero FindHero()
    {
        Hero hero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(i => i == KeeperDisplay.SelectedHero.Hero);
        return hero;
    }

    private void EnableButtons()
    {
        for (int i = 0; i < _listPowerStats.Count; i++)
        {
            _listPowerStats[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < _listPowerStats.Count; i++)
        {
            _listDefenceStats[i].gameObject.SetActive(true);
        }
    }

    private void DisableButtons()
    {
        for (int i = 0; i < _listPowerStats.Count; i++)
        {
            _listPowerStats[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < _listPowerStats.Count; i++)
        {
            _listDefenceStats[i].gameObject.SetActive(false);
        }
    }

    private void CheckPoints(Hero hero)
    {
        if (hero.LevelSystem.StatPoints == 0)
        {
            DisableButtons();
            OnOverStatPoints?.Invoke();
            NotificationSystem.Instance.CheckNotifications();
        }
    }

    private void UpPowerStats(int index)
    {
        Hero hero = FindHero();

        switch (index)
        {
            case 0:
                hero.LevelUpHeroStats.CritMulti++;
                break;

            case 1:
                hero.LevelUpHeroStats.HasteMulti++;
                break;

            case 2:
                hero.LevelUpHeroStats.AccuracyMulti++;
                break;
        }

        hero.HeroPowerPoints.SumPower(hero, KeeperDisplay.QuestParametresSystemFix);
        hero.LevelSystem.ChangeStatPoints(-1);
        KeeperDisplay.UpdateUIInfo(hero);
        CheckPoints(hero);
    }

    private void UpDefenceStats(int index)
    {
        Hero hero = FindHero();

        switch (index)
        {
            case 0:
                hero.LevelUpHeroStats.DurabilityMulti++;
                break;

            case 1:
                hero.LevelUpHeroStats.AdaptabilityMulti++;
                break;

            case 2:
                hero.LevelUpHeroStats.SuppressionMulti++;
                break;
        }

        hero.HeroDefencePoints.SumDefence(hero, KeeperDisplay.QuestParametresSystemFix);
        hero.LevelSystem.ChangeStatPoints(-1);
        KeeperDisplay.UpdateUIInfo(hero);
        CheckPoints(hero);
    }
}
