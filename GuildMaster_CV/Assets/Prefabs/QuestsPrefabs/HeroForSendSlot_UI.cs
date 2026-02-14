using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroForSendSlot_UI : MonoBehaviour
{
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private Image _icon;
    [SerializeField] private TextMeshProUGUI _nameClass;
    [SerializeField] private TextMeshProUGUI _nameHero;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private Button _takeHero;
    [Header("Hero Parametres")]
    [SerializeField] private Hero _hero;

    public Hero Hero => _hero;
    public RecruitQuestKeeperController RecruitQuestKeeperController { get; private set; }

    private void Awake()
    {
        _buttonSelf?.onClick.AddListener(ClickedSlot);
        _takeHero?.onClick.AddListener(TakeHero);

        RecruitQuestKeeperController = GetComponentInParent<RecruitQuestKeeperController>();
    }

    public void Init(Hero hero)
    {
        _hero = hero;
        UpdateUISlot(hero);
    }

    private void UpdateUISlot(Hero hero)
    {
        _icon.sprite = hero.HeroData.Icon;
        _nameClass.text = hero.HeroData.ClassName;
        _nameHero.text = hero.HeroName;
        _level.text = $"Level: {hero.HeroLevel.ToString()}";
    }

    private void ClickedSlot()
    {
        if (RecruitQuestKeeperController != null)
        {
            RecruitQuestKeeperController.UpdateHeroInfo(this.Hero);
            RecruitQuestKeeperController.Selecthero(this);
        }
    }

    private void TakeHero()
    {
        if (RecruitQuestKeeperController != null)
        {
            if (RecruitQuestKeeperController.MainQuestKeeperDisplay.SelectedQuest != null)
                RecruitQuestKeeperController.TakeHero(this);

            else
                RecruitQuestKeeperController.MainQuestKeeperDisplay.SetNotificationText(RecruitQuestKeeperController.MainQuestKeeperDisplay.SelectQuestNotif);
        }

    }
}
