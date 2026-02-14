using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HospitalSlot_UI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button _cancelHeal;
    [SerializeField] private Image _heroIcon;
    [SerializeField] private TextMeshProUGUI _daysLeft;
    [SerializeField] private Hero _hero;
    [Header("Other")]
    public bool IsFreeSlot = true;
    public int Days = -1;

    public Hero Hero => _hero;

    public HospitalUIController HospitalUIController { get; private set; }


    private void Awake()
    {
        ClearInfo();

        HospitalUIController = GetComponentInParent<HospitalUIController>();
        _cancelHeal?.onClick.AddListener(CancelHeal);
    }

    public void ClearInfo()
    {
        _heroIcon.sprite = null;
        Color currentColor = _heroIcon.color;
        currentColor.a = 0f;
        _heroIcon.color = currentColor;

        _daysLeft.text = "";
        Days = -1;
        _hero = null;
    }

    public void Init(Hero hero)
    {
        _hero = hero;
        _heroIcon.sprite = hero.HeroData.Icon;
        Color currentColor = _heroIcon.color;
        currentColor.a = 1f;
        _heroIcon.color = currentColor;

        switch (hero.WoundType)
        {
            case WoundsType.None:
                break;

            case WoundsType.Light:
                Days = HospitalUIController.LightWoundDay;
                break;

            case WoundsType.Medium:
                Days = HospitalUIController.MediumWoundDay;
                break;

            case WoundsType.Heavy:
                Days = HospitalUIController.HeavyWoundDay;
                break;
        }

        _daysLeft.text = $"Days: {Days}";
    }

    public void NextDay()
    {
        if (Days > 0)
            Days--;
    }

    public void UpdateUI()
    {
        if (Days < 0)
            _daysLeft.text = "Free";

        else
            _daysLeft.text = $"Days: {Days}";
    }

    private void CancelHeal()
    {
        Hero foundHero = HospitalUIController.GuildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(hero => hero == _hero);

        if (foundHero != null)
        {
            foundHero.IsHealing = false;
        }

        ClearInfo();
        IsFreeSlot = true;

        HospitalUIController.ShowDisplay();
    }
}
