using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using TMPro;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Events;

public class PrepareQuestUIController : MonoBehaviour, ILearning
{
    [SerializeField] private GameObject _preparationWindow;
    [SerializeField] private DynamicEquipDisplay _equipDisplay;
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private HeroAbilitiesInfo _abilityInfo;
    [Header("Stats UI")]
    [SerializeField] private TextMeshProUGUI _nameHero;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _powerPoints;
    [SerializeField] private TextMeshProUGUI _defencePoints;
    [SerializeField] private TextMeshProUGUI _powerStatsNameText;
    [SerializeField] private TextMeshProUGUI _powerStatsMultiText;
    [SerializeField] private TextMeshProUGUI _powerStatsValueText;
    [SerializeField] private TextMeshProUGUI _defenceStatsNameText;
    [SerializeField] private TextMeshProUGUI _defenceStatsMultiText;
    [SerializeField] private TextMeshProUGUI _defenceStatsValueText;
    [Header("Resistance")]
    [SerializeField] private ResImage_UI[] _resImages;
    [Header("HeroExp")]
    [SerializeField] private SliderBar _expSliderBar;
    [Header("Datas")]
    [SerializeField] private GameObject _heroInventory;
    [SerializeField] private GameObject _heroAbilities;
    [Header("Buttons")]
    [SerializeField] private Button _inventoryButton;
    [SerializeField] private Button _abilitiesButton;
    [Header("Ability System")]
    [SerializeField] private DynamicHeroAbilityDisplay _abilityDisplay;
    [SerializeField] private AbilitySlot_UI _abilitySlotPrefab;
    [SerializeField] private GameObject _abilityList;
    [SerializeField] private TextMeshProUGUI _nameAbility;
    [SerializeField] private TextMeshProUGUI _primaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _secondaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _manaCost;
    [SerializeField] private TextMeshProUGUI _descriptionAbility;
    [SerializeField] private TextMeshProUGUI _powerStatsName;
    [SerializeField] private TextMeshProUGUI _powerStatsValue;
    [SerializeField] private TextMeshProUGUI _defenceStatsName;
    [SerializeField] private TextMeshProUGUI _defenceStatsValue;
    [Header("Learning Properties")]
    [SerializeField] private bool _learnCheck = false;
    [SerializeField] private List<StringListContainer> _abiltyInfoHelp = new List<StringListContainer>();

    private HeroQuestSlot_UI _selectedHero;
    private StatDisplayManager _statDisplayManager = new StatDisplayManager();
    private TextHighlighter _textHighlighter = new TextHighlighter();

    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }
    public QuestResistanceSystem QuestResistanceSystem { get; private set; }
    public QuestParametresSystemFix QuestParametresSystemFix { get; private set; }

    public HeroQuestSlot_UI SelectedHero => _selectedHero;
    public GuildKeeper GuildKeeper => _guildKeeper;

    public event UnityAction OnTakeAbility;
    public static UnityAction OnAbilityButtonClick;

    private void Awake()
    {
        MainQuestKeeperDisplay = GetComponent<MainQuestKeeperDisplay>();
        QuestResistanceSystem = GetComponent<QuestResistanceSystem>();
        QuestParametresSystemFix = GetComponent<QuestParametresSystemFix>();

        _inventoryButton?.onClick.AddListener(ShowHeroInventory);
        _abilitiesButton?.onClick.AddListener(ShowHeroAbilities);
    }

    private void OnEnable()
    {
        MainQuestKeeperDisplay.OnFillHeroSlotClick += SelectHero;
        MainQuestKeeperDisplay.OnFillHeroSlotClick += ShowPreparationWindow;

        if (_selectedHero != null && _selectedHero.Hero != null)
        {
            _selectedHero.Hero.OnChangeResistance += UpdateResistance;
        }
    }

    private void OnDisable()
    {
        MainQuestKeeperDisplay.OnFillHeroSlotClick -= ShowPreparationWindow;
        MainQuestKeeperDisplay.OnFillHeroSlotClick -= SelectHero;

        _selectedHero = null;

        ClearUI();
    }

    private void RefreshInfo()
    {
        _heroInventory.gameObject.SetActive(false);
        _heroAbilities.gameObject.SetActive(false);
    }

    private void UpdateInfo()
    {
        if (!_heroInventory.activeSelf && !_heroAbilities.activeSelf)
            ShowHeroInventory();

        else if (_heroInventory.activeSelf)
            ShowHeroInventory();

        else if (_heroAbilities.activeSelf)
            ShowHeroAbilities();
    }

    private void SelectHero(HeroQuestSlot_UI heroSlot)
    {
        _selectedHero = heroSlot;
    }

    private void ShowPreparationWindow(HeroQuestSlot_UI heroSlot)
    {
        _preparationWindow.gameObject.SetActive(true);

        UpdateUIInfo(heroSlot.Hero);
        UpdateResistance(heroSlot.Hero);
        UpdateInfo();
        UpdateHeroExp(heroSlot.Hero);
    }

    private void ClearUI()
    {
        if (_nameHero != null) _nameHero.text = "";
        if (_level != null) _level.text = "Level: ";
        if (_powerPoints != null) _powerPoints.text = "Power: ";
        if (_defencePoints != null) _defencePoints.text = "Defence: ";

        if (_powerStatsNameText != null) _powerStatsNameText.text = "";
        if (_powerStatsMultiText != null) _powerStatsMultiText.text = "";
        if (_powerStatsValueText != null) _powerStatsValueText.text = "";

        if (_defenceStatsNameText != null) _defenceStatsNameText.text = "";
        if (_defenceStatsMultiText != null) _defenceStatsMultiText.text = "";
        if (_defenceStatsValueText != null) _defenceStatsValueText.text = "";

        //if (_expText != null) _expText.text = "0 / 0";

        _expSliderBar.MainSlider.value = 0;
        _expSliderBar.MainSlider.enabled = false;

        _expSliderBar.TextPlace.text = $"Exp: 0 / 0";

        //if (_expSlider != null)
        //{
        //    _expSlider.value = 0;
        //    _expSlider.enabled = false;
        //}
    }

    private void UpdateHeroExp(Hero hero)
    {
        _expSliderBar.MainSlider.maxValue = hero.LevelSystem.RequiredExp;
        _expSliderBar.MainSlider.value = hero.LevelSystem.CurrentExp;

        _expSliderBar.TextPlace.text = $"Exp: {hero.LevelSystem.CurrentExp} / {hero.LevelSystem.RequiredExp}";


        //_expSlider.maxValue = hero.LevelSystem.RequiredExp;
        //_expSlider.value = hero.LevelSystem.CurrentExp;

        //_expText.text = $"{hero.LevelSystem.CurrentExp} / {hero.LevelSystem.RequiredExp}";
    }

    public void UpdateUIInfo(Hero hero)
    {
        _nameHero.text = $"{hero.HeroName}";
        _level.text = $"Level: {hero.HeroLevel}";
        _powerPoints.text = $"Power points: {Mathf.FloorToInt(hero.HeroPowerPoints.AllPower * hero.HeroPowerPoints.AllCoefPower)}";
        _defencePoints.text = $"Defence Points: {Mathf.FloorToInt(hero.HeroDefencePoints.AllDefence * hero.HeroDefencePoints.AllCoefDefence)}";

        (string, string, string, string) multiText = _statDisplayManager.SetStatsMultiText(hero.VisibleHeroStats);
        (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(hero.VisibleHeroStats, true);

        _powerStatsNameText.text = statsText.Item1;
        _powerStatsMultiText.text = multiText.Item2;
        _powerStatsValueText.text = statsText.Item2;

        _defenceStatsNameText.text = statsText.Item3;
        _defenceStatsMultiText.text = multiText.Item4;
        _defenceStatsValueText.text = statsText.Item4;

        UpdateResistance(hero);
    }

    private void UpdateResistance(Hero hero)
    {
        for (int i = 0; i < _resImages.Length; i++)
        {
            _resImages[i].SetTypeDamage(hero.ResistanceSystem.AllRes[i].TypeDamage);
            _resImages[i].Percent.text = $"{hero.ResistanceSystem.AllRes[i].ValueResistance.ToString()}%";
        }
    }

    private void UpdateResistance()
    {
        if (_selectedHero != null && _selectedHero.Hero != null)
        {
            Hero hero = _selectedHero.Hero;

            for (int i = 0; i < _resImages.Length; i++)
            {
                _resImages[i].SetTypeDamage(hero.ResistanceSystem.AllRes[i].TypeDamage);
                _resImages[i].Percent.text = $"{hero.ResistanceSystem.AllRes[i].ValueResistance.ToString()}%";
            }
        }
    }

    private void ShowHeroInventory()
    {
        RefreshInfo();
        _heroInventory.gameObject.SetActive(true);

        EquipSystem heroEquip = _selectedHero.Hero.EquipHolder.EquipSystem;
        _equipDisplay.RefreshDynamicInventory(heroEquip, 0);
    }

    public void ShowHeroAbilities()
    {
        RefreshInfo();
        _heroAbilities.gameObject.SetActive(true);

        HeroAbilitySystem heroAbilities = MainQuestKeeperDisplay.SelectedHero.Hero.AbilityHolder.HeroAbilitySystem;
        _abilityDisplay.RefreshDynamicAbilities(heroAbilities);

        ClearAbilitiesSlots();

        foreach (var ability in MainQuestKeeperDisplay.SelectedHero.Hero.ListAbilities)
            CreateAbilitySlot(ability);

        MainQuestKeeperDisplay.SelectedHero.Hero.AbilityHolder.ShowAbilities();

        if (MainQuestKeeperDisplay is not TakingQuestUIController)
            SetLearningTextFirst();
        //OnAbilityButtonClick?.Invoke();
    }

    private void ClearAbilitiesSlots()
    {
        foreach (Transform ability in _abilityList.transform.Cast<Transform>())
        {
            Destroy(ability.gameObject);
        }

        _nameAbility.text = "";
        _primaryTypeAbility.text = "";
        _secondaryTypeAbility.text = "";
        _manaCost.text = "";
        _descriptionAbility.text = "";

        _powerStatsName.text = "";
        _powerStatsValue.text = "";

        _defenceStatsName.text = "";
        _defenceStatsValue.text = "";
    }

    private void CreateAbilitySlot(Ability ability)
    {
        AbilitySlot_UI abilitySlot = Instantiate(_abilitySlotPrefab, _abilityList.transform);
        abilitySlot.Init(ability);
    }

    public void UpdateAbilityInfo(AbilitySlot_UI abilitySlot_UI)
    {
        _nameAbility.text = abilitySlot_UI.Ability.AbilityData.Name;
        _descriptionAbility.text = _textHighlighter.ChangeColorText(abilitySlot_UI.Ability.AbilityData.Description);
        //_descriptionAbility.text = abilitySlot_UI.Ability.AbilityData.Description;

        if (QuestParametresSystemFix != null)
            _manaCost.text = $"Mana Cost: {abilitySlot_UI.Ability.ManaCost * QuestParametresSystemFix.ManaCoef}";

        else
            _manaCost.text = $"Mana Cost: {abilitySlot_UI.Ability.ManaCost * MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.CoefMana}";

        switch (abilitySlot_UI.Ability.AbilityData.GeneralType)
        {
            case GeneralTypeAbility.Attack:
                _primaryTypeAbility.text = $"Primary Type Ability:  Attack";
                break;

            case GeneralTypeAbility.Stats:
                _primaryTypeAbility.text = $"Primary Type Ability:  Amplify";
                break;
        }

        switch (abilitySlot_UI.Ability.AbilityData.TypeAbility)
        {
            case TypeAbilities.DamageSingleTarget:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Single Target";
                break;

            case TypeAbilities.AoEDamage:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  AoE";
                break;

            case TypeAbilities.Buff:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Buff";
                break;

            case TypeAbilities.Aura:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Aura";
                break;

            case TypeAbilities.Summon:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Summon";
                break;

            case TypeAbilities.Debuff:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Debuff";
                break;

            case TypeAbilities.Metamorphosis:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Metamorphosis";
                break;

            case TypeAbilities.Suppression_Aura:
                _secondaryTypeAbility.text = $"Secondary Type Ability:  Suppression Aura";
                break;
        }

        foreach (HeroStats stat in abilitySlot_UI.Ability.Stats)
        {
            if (stat != null)
            {
                (string, string, string, string) statsText = _statDisplayManager.SetAllStatsText(stat, false);

                _powerStatsName.text = statsText.Item1;
                _powerStatsValue.text = statsText.Item2;

                _defenceStatsName.text = statsText.Item3;
                _defenceStatsValue.text = statsText.Item4;
            }
        }


        QuestResistanceSystem.ShowAbilityResistance(abilitySlot_UI);

        //_abilityInfo.ShowAbilityResistance(abilitySlot_UI);

        if (_preparationWindow.gameObject.activeSelf)
            if (MainQuestKeeperDisplay is not TakingQuestUIController)
                TakeAbility(abilitySlot_UI);
    }

    private void TakeAbility(AbilitySlot_UI abilitySlot_UI)
    {
        QuestSlot_UI questSlot = MainQuestKeeperDisplay.SelectedQuest;
        SendHeroesSlots heroes = MainQuestKeeperDisplay.HeroesSlots;

        Hero hero = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(i => i == _selectedHero.Hero);

        if (!hero.IsSentOnQuest)
        {
            List<AbilityData> localAbilList = new List<AbilityData>();

            for (int i = 0; i < hero.AbilityHolder.HeroAbilitySystem.AbilityList.Count; i++)
            {
                if (hero.AbilityHolder.HeroAbilitySystem.AbilityList[i] != null && hero.AbilityHolder.HeroAbilitySystem.AbilityList[i].AbilityData != null)
                    localAbilList.Add(hero.AbilityHolder.HeroAbilitySystem.AbilityList[i].AbilityData);
            }

            bool abilityExists = localAbilList.Any(ability => ability == abilitySlot_UI.Ability.AbilityData);

            for (int i = 0; i < _abilityDisplay.AbilitySlots.Count; i++)
            {
                if (_abilityDisplay.AbilitySlots[i].IsSlotClicked == true && !abilityExists && _abilityDisplay.AbilitySlots[i].Ability.AbilityData == null)
                {
                    hero.AbilityHolder.HeroAbilitySystem.AddAbility(abilitySlot_UI.Ability, i);
                    _abilityDisplay.AbilitySlots[i].SetSlotClicked(false);
                    _abilityDisplay.AbilitySlots[i].ChangeColorFrame(Color.black);
                }

                else
                {
                    _abilityDisplay.AbilitySlots[i].SetSlotClicked(false);
                    _abilityDisplay.AbilitySlots[i].ChangeColorFrame(Color.black);
                }
            }

            HeroAbilitySystem heroAbilities = _selectedHero.Hero.AbilityHolder.HeroAbilitySystem;
            _abilityDisplay.RefreshDynamicAbilities(heroAbilities);

            OnTakeAbility?.Invoke();
        }
    }

    public void ClickedHero(HeroQuestSlot_UI hero)
    {
        if (_selectedHero == hero)
            return;

        else
        {
            _abilityInfo.ClearAllIcons();
        }
    }

    public void ClearHeroResImages()
    {
        for (int i = 0; i < _resImages.Length; i++)
        {
            _resImages[i].Percent.text = $"0%";
        }
    }

    public void SetLearningTextByButton() { }

    public void SetLearningTextFirst()
    {
        if (!_learnCheck)
        {
            LearningGameManager.Instance.SetText(_abiltyInfoHelp);
            _learnCheck = true;
        }
    }
}
