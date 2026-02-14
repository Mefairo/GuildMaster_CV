using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HospitalUIController : MonoBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private HeroHospitalSlot_UI _prefabHeroSlot;
    [SerializeField] private GameObject _heroesList;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private GuildValutes _guildValutes;
    [SerializeField] private NextDayController _nextDayController;
    [Header("Wound Pays")]
    [SerializeField] private int _lightWoundPay;
    [SerializeField] private int _mediumWoundPay;
    [SerializeField] private int _heavyWoundPay;
    [SerializeField] private float _coefCostHeal;
    [Header("Wound Days For Heal")]
    [SerializeField] private int _lightWoundDay;
    [SerializeField] private int _mediumWoundDay;
    [SerializeField] private int _heavyWoundDay;
    [SerializeField] private bool _isInstantHeal = false;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _infoText;
    [Header("Heal Slots")]
    [SerializeField] private List<HospitalSlot_UI> _healSlots;

    private HeroHospitalSlot_UI _heroSlot;

    public static event UnityAction<int> OnPayForHeroHeal;

    public GuildKeeper GuildKeeper => _guildKeeper;

    public int LightWoundPay => _lightWoundPay;
    public int MediumWoundPay => _mediumWoundPay;
    public int HeavyWoundPay => _heavyWoundPay;
    public int LightWoundDay => _lightWoundDay;
    public int MediumWoundDay => _mediumWoundDay;
    public int HeavyWoundDay => _heavyWoundDay;

    private void Awake()
    {
        _panel.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        _nextDayController.OnNextDay += NextDay;
    }

    private void OnDisable()
    {
        _nextDayController.OnNextDay -= NextDay;
    }

    public void ShowDisplay()
    {
        if (!_panel.gameObject.activeSelf)
            _panel.gameObject.SetActive(true);

        ClearSlots();

        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            if (hero.WoundType == WoundsType.Healthy || hero.IsHealing)
                continue;

            else
            {
                CreateHeroSlot(hero);
            }
        }

        UpdateUI();
    }

    private void CreateHeroSlot(Hero hero)
    {
        HeroHospitalSlot_UI heroSlot = Instantiate(_prefabHeroSlot, _heroesList.transform);
        heroSlot.Init(hero);
    }

    private void ClearSlots()
    {
        foreach (Transform hero in _heroesList.transform.Cast<Transform>())
        {
            Destroy(hero.gameObject);
        }

        _infoText.text = "";
    }

    private void UpdateUI()
    {
        foreach (HospitalSlot_UI healSlot in _healSlots)
        {
            healSlot.UpdateUI();
        }
    }

    public void SelectHeroSlot(HeroHospitalSlot_UI heroSlot)
    {
        _heroSlot = heroSlot;
    }

    public void SendHeroForHeal(HeroHospitalSlot_UI heroSlot)
    {
        if (heroSlot.Hero.WoundType == WoundsType.Light && _isInstantHeal)
            InstantHeal(heroSlot);

        else
        {
            HospitalSlot_UI freeSlot = _healSlots.FirstOrDefault(slot => slot.IsFreeSlot);
            int woundPay = 0;

            if (freeSlot != null)
            {
                switch (heroSlot.Hero.WoundType)
                {
                    case WoundsType.None: break;

                    case WoundsType.Light:
                        woundPay = heroSlot.Hero.HeroLevel * _lightWoundPay;
                        break;

                    case WoundsType.Medium:
                        woundPay = heroSlot.Hero.HeroLevel * _mediumWoundPay;
                        break;

                    case WoundsType.Heavy:
                        woundPay = heroSlot.Hero.HeroLevel * _heavyWoundPay;
                        break;
                }

                woundPay = (int)(woundPay * _coefCostHeal);

                if (_guildValutes.Gold >= woundPay)
                {
                    OnPayForHeroHeal?.Invoke(-woundPay);
                    freeSlot.Init(heroSlot.Hero);
                    heroSlot.Hero.IsHealing = true;
                    freeSlot.IsFreeSlot = false; // Например, помечаем слот как занятый
                    ShowDisplay();
                }

                else
                {
                    _infoText.text = "Not enough gold";
                    return;
                }
            }
            else
            {
                Debug.Log("Свободных слотов нет.");
            }
        }
    }

    private void NextDay()
    {
        foreach (HospitalSlot_UI slot in _healSlots)
        {
            slot.NextDay();

            if (slot.Days == 0)
            {
                Hero foundHero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(hero => hero == slot.Hero);

                if (foundHero != null)
                {
                    foundHero.WoundType = WoundsType.Healthy;
                    foundHero.IsHealing = false;
                }

                slot.ClearInfo();
                slot.IsFreeSlot = true;
            }
        }
    }

    private void InstantHeal(HeroHospitalSlot_UI heroSlot)
    {
        int woundPay = 0;

        woundPay = heroSlot.Hero.HeroLevel * _lightWoundPay;

        woundPay = (int)(woundPay * _coefCostHeal);

        if (_guildValutes.Gold >= woundPay)
        {
            OnPayForHeroHeal?.Invoke(-woundPay);
            heroSlot.Hero.WoundType = WoundsType.Healthy;
            ShowDisplay();
        }

        else
        {
            _infoText.text = "Not enough gold";
            return;
        }
    }

    public void ChangeHealCostTalent(float coefCost)
    {
        _coefCostHeal += coefCost;
    }

    public void InstantHealByTalent()
    {
        _isInstantHeal = true;
    }
}
