using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WeeklyPaySystem : MonoBehaviour
{
    [SerializeField] private Image _panelWindow;
    [SerializeField] private HeroPaySlot_IU _prefabHeroSlot;
    [Header("Controllers")]
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private NextDayController _nextDayController;
    [SerializeField] private RecruitQuestKeeperController _recruitQuestKeeperController;
    [SerializeField] private SendHeroesSlots _heroesQuestSlots;
    [Header("Buttons")]
    [SerializeField] private Button _openWindowButton;
    [SerializeField] private Button _closeWindowButton;
    [SerializeField] private Button _payButton;
    [SerializeField] private Button _kickButton;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _requiredGold;
    [SerializeField] private TextMeshProUGUI _guildGold;
    [SerializeField] private TextMeshProUGUI _infoText;
    [Header("List Heroes")]
    [SerializeField] private GameObject _heroesList;

    private HeroPaySlot_IU _heroSlot;

    public static event UnityAction<int> OnPayWeeklyHeroPaid;

    private void Awake()
    {
        _payButton?.onClick.AddListener(PayForWeeklyHeroPaid);
        _kickButton?.onClick.AddListener(KickHero);

        _panelWindow.gameObject.SetActive(false);
    }

    public void ShowDisplay()
    {
        _panelWindow.gameObject.SetActive(true);

        ClearInfo();
        ClearHeroSlots();

        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            CreateHeroSlot(hero);
        }

        UpdateUI();
    }

    private void CreateHeroSlot(Hero hero)
    {
        HeroPaySlot_IU heroSlot = Instantiate(_prefabHeroSlot, _heroesList.transform);
        heroSlot.Init(hero);
    }

    private void UpdateUI()
    {
        int totalPay = 0;

        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            if (!hero.IsSentOnQuest)
                totalPay += hero.WeeklyPay;
        }

        _requiredGold.text = $"Required: {totalPay}";
        _guildGold.text = $"Guild: {_guildValutes.Gold}";
    }

    private void ClearInfo()
    {
        _requiredGold.text = $"Required:";
        _guildGold.text = $"Guild:";
        _infoText.text = $"";
    }

    private void ClearHeroSlots()
    {
        foreach (Transform hero in _heroesList.transform.Cast<Transform>())
        {
            Destroy(hero.gameObject);
        }
    }

    public void SelectHero(HeroPaySlot_IU heroSlot)
    {
        _heroSlot = heroSlot;
    }

    private void PayForWeeklyHeroPaid()
    {
        if (_heroSlot != null && _heroSlot.Hero != null)
        {
            if (_heroSlot.Hero.WeeklyPay <= _guildValutes.Gold)
            {
                OnPayWeeklyHeroPaid?.Invoke(-_heroSlot.Hero.WeeklyPay);

                Hero foundHero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(hero => hero == _heroSlot.Hero);

                if (foundHero != null)
                {
                    // ≈сли герой найден, установить ему WeeklyPay = 0
                    _guildValutes.ChangeTotalPay(-foundHero.WeeklyPay);
                    foundHero.WeeklyPay = 0;
                    foundHero.WeekTracking = _nextDayController.WeekAmount;
                    NotificationSystem.Instance.CheckNotifications();
                }


                ClearInfo();
                ClearHeroSlots();

                foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
                {
                    CreateHeroSlot(hero);

                    bool allHeroesPaid = _guildKeeper.RecruitSystem.HeroSlots.All(hero => hero.WeeklyPay == 0);

                    if (allHeroesPaid)
                    {
                        // ≈сли у всех героев WeeklyPay равен 0, выполнить нужное действие
                        _guildValutes.IsNotPaid = false;
                        NotificationSystem.Instance.CheckNotifications();
                    }
                }

                UpdateUI();
            }

            else
                _infoText.text = $"Not enough gold";
        }
    }

    private void KickHero()
    {
        if (_heroSlot != null && _heroSlot.Hero != null)
        {
            if (_heroSlot.Hero.IsSentOnQuest)
                return;

            Hero foundHero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(hero => hero == _heroSlot.Hero);
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

            ClearInfo();
            ClearHeroSlots();

            if (_guildKeeper.RecruitSystem.HeroSlots.Count != 0)
            {
                foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
                {
                    CreateHeroSlot(hero);

                    bool allHeroesPaid = _guildKeeper.RecruitSystem.HeroSlots.All(hero => hero.WeeklyPay == 0);

                    if (allHeroesPaid)
                    {
                        // ≈сли у всех героев WeeklyPay равен 0, выполнить нужное действие
                        _guildValutes.IsNotPaid = false;
                    }
                }
            }

            else
            {
                _guildValutes.IsNotPaid = false;
            }

            UpdateUI();
            NotificationSystem.Instance.CheckNotifications();
        }
    }
}
