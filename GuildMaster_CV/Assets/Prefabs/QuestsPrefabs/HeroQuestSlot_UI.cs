using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class HeroQuestSlot_UI : MonoBehaviour
{
    [SerializeField] private int _slotIndex;
    [SerializeField] private bool _isSlotClicked = false;
    [SerializeField] private Hero _hero;
    [Header("UI Info")]
    [SerializeField] private Image _heroIcon;
    [SerializeField] private TextMeshProUGUI _powerPoints;
    [SerializeField] private TextMeshProUGUI _defencePoints;
    [SerializeField] private Button _deleteHero;
    [SerializeField] private Button _buttonSelf;

    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }
    public RecruitQuestKeeperController RecruitQuestKeeperController { get; private set; }
    public PrepareQuestUIController PrepareQuestUIController { get; private set; }
    public SendHeroesSlots SendHeroesSlots { get; private set; }
    public TakingQuestUIController TakingQuestUIController { get; private set; }

    public Hero Hero => _hero;
    public bool IsSlotClicked => _isSlotClicked;
    public int SlotIndex => _slotIndex;

    public static UnityAction OnDeleteHero;

    private void Awake()
    {
        MainQuestKeeperDisplay = GetComponentInParent<MainQuestKeeperDisplay>();
        RecruitQuestKeeperController = GetComponentInParent<RecruitQuestKeeperController>();
        PrepareQuestUIController = GetComponentInParent<PrepareQuestUIController>();
        SendHeroesSlots = GetComponentInParent<SendHeroesSlots>();
        TakingQuestUIController = GetComponentInParent<TakingQuestUIController>();

        _buttonSelf?.onClick.AddListener(ClickedSlot);
        _deleteHero?.onClick.AddListener(DeleteHero);
    }

    private void Start()
    {
        ClearSlot();
    }

    private void OnEnable()
    {
        if (RecruitQuestKeeperController != null)
            RecruitQuestKeeperController.TakeFreeHero += Init;

        if (TakingQuestUIController != null)
        {
            TakingQuestUIController.OnUpdatePower += UpdatePowerUI;
            TakingQuestUIController.OnUpdateDefence += UpdateDefenceUI;
        }

    }

    private void OnDisable()
    {
        if (RecruitQuestKeeperController != null)
            RecruitQuestKeeperController.TakeFreeHero -= Init;

        if (TakingQuestUIController != null)
        {
            TakingQuestUIController.OnUpdatePower -= UpdatePowerUI;
            TakingQuestUIController.OnUpdateDefence -= UpdateDefenceUI;
        }

        //ClearSlot();
        ClearChooseSlot();
    }

    public void SetSlotIndex(int index)
    {
        _slotIndex = index;
    }

    public void Init(Hero hero)
    {
        if (_hero != null && _hero.HeroData != null)
            return;

        else if (_isSlotClicked)
        {
            _hero = hero;
            UpdateIconSlot(hero);
            hero.ChangeSlotCondition(true);

            if (SendHeroesSlots != null)
            {
                SendHeroesSlots.AddHero();
            }

            if (RecruitQuestKeeperController != null)
                RecruitQuestKeeperController.HeroesCopy.Remove(hero);
        }

    }

    public void InitSlot(Hero hero)
    {
        if (hero != null && hero.HeroData != null)
        {
            _hero = hero;
            UpdateIconSlot(hero);
        }

        else
            ClearSlot();

        if (SendHeroesSlots != null)
            SendHeroesSlots.AddHero();
    }

    private void UpdateIconSlot(Hero hero)
    {
        _heroIcon.sprite = hero.HeroData.Icon;
        Color currentColor = _heroIcon.color;
        currentColor.a = 1f;
        _heroIcon.color = currentColor;
    }

    private void UpdatePowerUI(float power, int heroIndex)
    {
        //if (heroIndex == _slotIndex)
        //    _powerPoints.text = $"{power}";
    }

    private void UpdateDefenceUI(float defence, int heroIndex)
    {
        //if (heroIndex == _slotIndex)
        //    _defencePoints.text = $"{defence}";
    }

    public void UpdateUI(float power, float defence)
    {
        _powerPoints.text = $"{power}";
        _defencePoints.text = $"{defence}";
    }

    public void ClickedSlot()
    {
        if (PrepareQuestUIController != null)
        {
            PrepareQuestUIController.ClickedHero(this);
        }

        if (MainQuestKeeperDisplay != null)
        {
            if (MainQuestKeeperDisplay is TakingQuestUIController)
            {
                if (this != null && this._hero != null && this._hero.HeroData != null)
                {
                    MainQuestKeeperDisplay.SelectHero(this);
                    MainQuestKeeperDisplay.HeroesSlots.SelectSlot(this);
                }

                else
                {
                    return;
                }
            }

            else
            {
                MainQuestKeeperDisplay.SelectHero(this);
                MainQuestKeeperDisplay.HeroesSlots.SelectSlot(this);
            }
        }

        _isSlotClicked = true;
        HighlightSlot(true);
    }

    public void DeleteHero()
    {
        if (_hero != null && _hero.HeroData != null)
        {
            RecruitQuestKeeperController.HeroesCopy.Add(_hero);
            RecruitQuestKeeperController.RefreshDisplay();
            //QuestParametresSystem.DecreaseHeroResistance(MainQuestKeeperDisplay.SelectedQuest, false);
        }

        else
            return;

        _hero.ChangeSlotCondition(false);

        for (int i = 0; i < _hero.AbilityHolder.HeroAbilitySystem.AbilityList.Count; i++)
            _hero.AbilityHolder.HeroAbilitySystem.RemoveAbility(i);

        ClearSlot();
        MainQuestKeeperDisplay.SelectHero(this);

        if (SendHeroesSlots != null)
        {
            SendHeroesSlots.RemoveHero();
        }

        OnDeleteHero?.Invoke();
    }

    public void ClearSlot()
    {
        if (_hero != null)
        {
            //_hero.OnChangePowerPoints -= UpdatePowerUI;
            //_hero.OnChangeDefencePoints -= UpdateDefenceUI;
        }

        _hero = null;

        _heroIcon.sprite = null;
        Color currentColor = _heroIcon.color;
        currentColor.a = 0f;
        _heroIcon.color = currentColor;

        _powerPoints.text = "0";
        _defencePoints.text = "0";

        ClearChooseSlot();
    }

    public void ClearChooseSlot()
    {
        _isSlotClicked = false;
        _buttonSelf.image.color = Color.black;
    }

    private void HighlightSlot(bool highlight)
    {
        _buttonSelf.image.color = highlight ? Color.yellow : Color.black;
    }
}
