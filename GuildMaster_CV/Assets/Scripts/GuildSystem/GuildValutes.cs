using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GuildValutes : MonoBehaviour
{
    [Header("Controllers")]
    [SerializeField] private CalculationResultsQuest _calculationResultsQuest;
    [SerializeField] protected NextDayController _nextDayController;
    [SerializeField] protected GuildKeeper _guildKeeper;
    [Header("Reputation")]
    [SerializeField] private int _level;
    [SerializeField] private int _currentRep;
    [SerializeField] private int _requiredRep;
    [SerializeField] private int _addRep;
    [SerializeField] private float _coefRep;
    [Header("Points")]
    [SerializeField] private int _levelPoints;
    [Header("Gold")]
    [SerializeField] private int _gold;
    [SerializeField] private List<int> _coefExtraGold = new List<int>();
    [SerializeField] private bool _isGoldTalentTake = false;
    [Header("Hero Places")]
    [SerializeField] private int _currentHeroPlaces;
    [SerializeField] private int _maxHeroPlaces;
    [Header("Pay")]
    [SerializeField] private int _totalPay;
    [Header("Talent Properties")]
    [SerializeField] private int _maxPlacesExtend;

    public bool IsNotPaid = false;

    public event UnityAction<int> OnChangeLevel;
    public event UnityAction<int> OnChangeCurrentRep;
    public event UnityAction<int> OnChangeRequiredRep;
    public event UnityAction<int> OnChangeGold;
    public event UnityAction<int> OnChangeCurrentHeroPlaces;
    public event UnityAction<int> OnChangeMaxHeroPlaces;
    public static event UnityAction OnChangeGuildPoints;

    public int Level => _level;
    public int CurrentRep => _currentRep;
    public int RequiredRep => _requiredRep;
    public int AddRep => _addRep;
    public int TotalPay => _totalPay;
    public int LevelPoints
    {
        get => _levelPoints;
        private set
        {
            _levelPoints = value;
            OnChangeGuildPoints?.Invoke();
        }
    }

    public int Gold
    {
        get => _gold;
        private set
        {
            _gold = value;
            OnChangeGold?.Invoke(value);
        }
    }

    public int CurrentHeroPlaces
    {
        get => _currentHeroPlaces;
        private set
        {
            _currentHeroPlaces = value;
            OnChangeCurrentHeroPlaces?.Invoke(value);
        }
    }

    public int MaxHeroPlaces
    {
        get => _maxHeroPlaces;
        private set
        {
            _maxHeroPlaces = value;
            OnChangeMaxHeroPlaces?.Invoke(value);
        }
    }

    private void Awake()
    {
        //_level = 1;
        _currentRep = 0;
        CheckedRequiredGuildExperience();
    }

    private void OnEnable()
    {
        KeeperDisplay.OnHireNewHero += HireHero;
        WeeklyPaySystem.OnPayWeeklyHeroPaid += ChangeGold;
        HospitalUIController.OnPayForHeroHeal += ChangeGold;
        ExpeditionalCompany.OnPayExpeditional += ChangeGold;

        _calculationResultsQuest.OnCollectGoldForQuest += ChangeGold;
        _nextDayController.OnNextDay += DaylyPayHeroes;
        _nextDayController.OnNextWeek += AddGoldByWeek;
    }
    private void OnDisable()
    {
        KeeperDisplay.OnHireNewHero -= HireHero;
        WeeklyPaySystem.OnPayWeeklyHeroPaid -= ChangeGold;
        HospitalUIController.OnPayForHeroHeal -= ChangeGold;
        ExpeditionalCompany.OnPayExpeditional -= ChangeGold;

        _calculationResultsQuest.OnCollectGoldForQuest -= ChangeGold;
        _nextDayController.OnNextDay -= DaylyPayHeroes;
        _nextDayController.OnNextWeek -= AddGoldByWeek;
    }

    private void HireHero(Hero hero)
    {
        CurrentHeroPlaces++;
    }

    public void KickHero(int amount)
    {
        CurrentHeroPlaces -= amount;
    }

    public void ChangeGuildPoints(int points)
    {
        LevelPoints -= points;
    }

    public void ChangeReputationForKick(Hero hero)
    {
        int rep = CalculateReduceReputation(hero);
        GainGuildReputation(rep);
    }

    public (int, bool) GainGuildReputation(int valueRep)
    {
        bool isLevelUp = false;

        if (valueRep > 0)
        {
            _addRep = (int)(valueRep * _coefRep);
            _currentRep += _addRep;
        }

        else
        {
            _addRep = valueRep;
            _currentRep += _addRep;
        }

        while (_currentRep >= _requiredRep)
        {
            // Вычисляем остаток опыта после достижения текущего уровня
            _currentRep -= _requiredRep;
            _level++;
            _levelPoints += _level;
            //_requiredRep = 1000 + (_level * 100 - 100);
            isLevelUp = true;

            // Генерируем события обновления
            OnChangeLevel?.Invoke(_level);
            OnChangeCurrentRep?.Invoke(_currentRep);

            CheckedMaxHeroPlaces();
            CheckedRequiredGuildExperience();

            OnChangeRequiredRep?.Invoke(_requiredRep);
        }

        if (_currentRep < 0)
        {
            _currentRep = 0;
        }

        // Если опыта не хватило на следующий уровень, просто обновляем текущее количество опыта
        OnChangeCurrentRep?.Invoke(_currentRep);

        return (_level, isLevelUp);
    }

    public void ChangeGold(int valueGold)
    {
        Gold += valueGold;
    }

    private void DaylyPayHeroes()
    {
        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            hero.WeeklyPay += hero.DailyPay;

            if (!hero.IsSentOnQuest)
            {
                _totalPay += hero.DailyPay;

                if (_nextDayController.LeftWeekDays != _nextDayController.WeekDays)
                {
                    if (hero.WeekTracking != _nextDayController.WeekAmount)
                    {
                        CheckGoldForDepts();

                        if (IsNotPaid)
                        {
                            NotificationSystem.Instance.CheckNotifications();
                            Debug.Log("not paid");
                        }

                        else
                        {
                            ChangeGold(-hero.WeeklyPay);
                            //Gold -= hero.WeeklyPay;
                            hero.WeekTracking = _nextDayController.WeekAmount;
                            hero.WeeklyPay = 0;
                        }
                    }
                }

                else
                {
                    CheckGoldForWeeklyPay();

                    if (IsNotPaid)
                    {
                        NotificationSystem.Instance.CheckNotifications();
                        Debug.Log("not paid");
                    }

                    else
                    {
                        ChangeGold(-hero.WeeklyPay);
                        //_gold -= hero.WeeklyPay;
                        hero.WeekTracking++;
                        hero.WeeklyPay = 0;
                    }
                }
            }
        }
    }

    private bool CheckGoldForWeeklyPay()
    {
        if (IsNotPaid)
            return IsNotPaid;

        else
        {
            IsNotPaid = false;
            _totalPay = 0;

            foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
            {
                if (!hero.IsSentOnQuest)
                {
                    _totalPay += hero.WeeklyPay;
                }
            }

            if (_totalPay > _gold)
            {
                IsNotPaid = true;
                return IsNotPaid;
            }

            else
            {
                IsNotPaid = false;
                return IsNotPaid;
            }
        }

    }

    private bool CheckGoldForDepts()
    {
        if (IsNotPaid)
            return IsNotPaid;

        else
        {
            IsNotPaid = false;
            _totalPay = 0;

            foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
            {
                if (!hero.IsSentOnQuest)
                {
                    if (hero.WeeklyPay > (hero.DailyPay * _nextDayController.WeekDays))
                    {
                        _totalPay += hero.WeeklyPay;
                    }
                }
            }
        }

        if (_totalPay > _gold)
        {
            IsNotPaid = true;
            return IsNotPaid;
        }

        else
        {
            IsNotPaid = false;
            return IsNotPaid;
        }
    }

    public void ChangeTotalPay(int value)
    {
        _totalPay += value;
    }

    private void CheckedMaxHeroPlaces()
    {
        switch (_level)
        {
            case 1:
                _maxHeroPlaces = 12;
                break;

            case 2:
                _maxHeroPlaces = 16;
                break;

            case 3:
                _maxHeroPlaces = 20;
                break;

            case 4:
                _maxHeroPlaces = 24;
                break;
        }

        _maxHeroPlaces += _maxPlacesExtend * _level;
    }

    public void ExtendMaxPlaces(int extraPlaces)
    {
        _maxPlacesExtend = extraPlaces;
        _maxHeroPlaces += _maxPlacesExtend * _level;
    }

    private void CheckedRequiredGuildExperience()
    {
        switch (_level)
        {
            case 1:
                _requiredRep = 100;
                break;

            case 2:
                _requiredRep = 700;
                break;

            case 3:
                _requiredRep = 5000;
                break;

            case 4:
                _requiredRep = 34300;
                break;

            case 5:
                _requiredRep = 308700;
                break;
        }
    }

    private int CalculateReduceReputation(Hero hero)
    {
        int rep = 0;

        switch (hero.LevelSystem.Level)
        {
            case 1:
                rep = Random.Range(1, 5);
                break;

            case 2:
                rep = Random.Range(10, 35);
                break;

            case 3:
                rep = Random.Range(50, 300);
                break;

            case 4:
                rep = Random.Range(10000, 50000);
                break;
        }

        return -rep;
    }

    public void AddGoldByTalent()
    {
        _isGoldTalentTake = true;
    }

    private void AddGoldByWeek()
    {
        if (_isGoldTalentTake)
        {
            int addGold = 0;

            foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
                addGold += _coefExtraGold[hero.LevelSystem.Level - 1];

            ChangeGold(addGold);
        }
    }

    public void ChangeCoefReputationByTalent(float coefRep)
    {
        _coefRep += coefRep;
    }
}
