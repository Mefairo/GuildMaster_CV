using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroHospitalSlot_UI : HeroPaySlot_IU
{
    [SerializeField] private WoundSlot_UI _wound;
    [SerializeField] private Button _healButton;

   public HospitalUIController HospitalUIController { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        HospitalUIController = GetComponentInParent<HospitalUIController>();
        _healButton?.onClick.AddListener(SendHeroToHeal);
    }

    public override void Init(Hero hero)
    {
       base.Init(hero);


    }

    public override void UpdateUISlot()
    {
        base.UpdateUISlot();

        _wound.SetImage(_hero.WoundType);

        switch(_hero.WoundType)
        {
            case WoundsType.None:
                break;

            case WoundsType.Healthy:

                break;

            case WoundsType.Light:
                _pay.text = $"{HospitalUIController.LightWoundPay * _hero.LevelSystem.Level}";
                break;

            case WoundsType.Medium:
                _pay.text = $"{HospitalUIController.MediumWoundPay * _hero.LevelSystem.Level}";
                break;

            case WoundsType.Heavy:
                _pay.text = $"{HospitalUIController.HeavyWoundPay * _hero.LevelSystem.Level}";
                break;

            case WoundsType.Dead:

                break;
        }
    }

    public override void UpdateItemPreview()
    {
        if (HospitalUIController != null)
        {
            HospitalUIController.SelectHeroSlot(this);
        }
    }

    private void SendHeroToHeal()
    {
        if (HospitalUIController != null)
        {
            HospitalUIController.SendHeroForHeal(this);
        }
    }
}
