using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using System.Reflection;

public class RecruitQuestKeeperController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private HeroForSendSlot_UI _heroQuestPrefab;
    [SerializeField] private AbilitySlot_UI _abilitySlotPrefab;
    [Header("Content Lists")]
    [SerializeField] private GameObject _heroesList;
    [SerializeField] private GameObject _abilityList;
    [Header("Datas")]
    [SerializeField] private GameObject _sendHeroInfo;
    [SerializeField] private GameObject _mainInfo;
    [SerializeField] private GameObject _abilityHero;
    [Header("HeroInfo")]
    [SerializeField] private TextMeshProUGUI _nameHero;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _power;
    [SerializeField] private TextMeshProUGUI _defence;
    [SerializeField] private DynamicEquipDisplay _equipDisplay;
    [Header("HeroExp")]
    [SerializeField] private SliderBar _expSliderBar;
    [SerializeField] private Slider _expSlider;
    [SerializeField] private TextMeshProUGUI _expText;
    [Header("HeroStats")]
    [SerializeField] private TextMeshProUGUI _powerStatsNameText;
    [SerializeField] private TextMeshProUGUI _powerStatsMultiText;
    [SerializeField] private TextMeshProUGUI _powerStatsValueText;
    [SerializeField] private TextMeshProUGUI _defenceStatsNameText;
    [SerializeField] private TextMeshProUGUI _defenceStatsMultiText;
    [SerializeField] private TextMeshProUGUI _defenceStatsValueText;
    [Header("HeroRes")]
    [SerializeField] private ResImage_UI[] _resImages;
    [Header("Ability")]
    [SerializeField] protected DynamicHeroAbilityDisplay _heroAbilityDisplay;
    [SerializeField] private TextMeshProUGUI _nameAbility;
    [SerializeField] private TextMeshProUGUI _descriptionAbility;
    [SerializeField] private TextMeshProUGUI _primaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _secondaryTypeAbility;
    [SerializeField] private TextMeshProUGUI _manaCost;
    [SerializeField] private TextMeshProUGUI _powerStatsName;
    [SerializeField] private TextMeshProUGUI _powerStatsValue;
    [SerializeField] private TextMeshProUGUI _defenceStatsName;
    [SerializeField] private TextMeshProUGUI _defenceStatsValue;
    [Header("Buttons")]
    [SerializeField] private Button _mainInfoButton;
    [SerializeField] private Button _abilityButton;
    [Header("Other")]
    [SerializeField] private GuildKeeper _guildKeeper;
    [SerializeField] private List<Hero> _heroesCopy;

    [SerializeField] private HeroForSendSlot_UI _selectedHero;
    private StatDisplayManager _statDisplayManager = new StatDisplayManager();
    private TextHighlighter _textHighlighter = new TextHighlighter();

    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }
    public HeroAbilitiesInfo HeroAbilitiesInfo { get; private set; }

    public List<Hero> HeroesCopy => _heroesCopy;

    public event UnityAction<Hero> TakeFreeHero;
    public event UnityAction OnChangeRoster;

    private void Awake()
    {
        MainQuestKeeperDisplay = GetComponent<MainQuestKeeperDisplay>();
        HeroAbilitiesInfo = GetComponent<HeroAbilitiesInfo>();

        _mainInfoButton?.onClick.AddListener(ShowMainInfo);
        _abilityButton?.onClick.AddListener(ShowAbility);
    }

    private void OnEnable()
    {
        MainQuestKeeperDisplay.OnEmptyHeroSlotClick += ShowHeroList;
    }

    private void OnDisable()
    {
        MainQuestKeeperDisplay.OnEmptyHeroSlotClick -= ShowHeroList;
    }

    private void ShowHeroList(HeroQuestSlot_UI heroSlot)
    {
        _sendHeroInfo.gameObject.SetActive(true);
        RefreshDisplay();
        _mainInfo.gameObject.SetActive(true);

        RefreshInfoHero();
        DisplayHeroes();
    }

    public void RefreshDisplay()
    {
        _mainInfo.gameObject.SetActive(false);
        _abilityHero.gameObject.SetActive(false);

        DisplayHeroes();
        RefreshInfoHero();
    }

    private void ShowMainInfo()
    {
        if (_selectedHero != null)
        {
            _abilityHero.gameObject.SetActive(false);
            _mainInfo.gameObject.SetActive(true);
            RefreshInfoHero();

            UpdateHeroInfo(_selectedHero.Hero);
            UpdateHeroExp(_selectedHero.Hero);
        }
    }

    private void ShowAbility()
    {
        if (_selectedHero != null)
        {
            _mainInfo.gameObject.SetActive(false);
            _abilityHero.gameObject.SetActive(true);
            RefreshInfoHero();

            ShowHeroAbilities(_selectedHero.Hero);
        }
    }

    private void RefreshInfoHero()
    {
        if (_nameHero != null) _nameHero.text = "";
        if (_level != null) _level.text = "Level: ";
        if (_power != null) _power.text = "Power: ";
        if (_defence != null) _defence.text = "Defence: ";

        _powerStatsNameText.text = "";
        _powerStatsMultiText.text = "";
        _powerStatsValueText.text = "";

        _defenceStatsNameText.text = "";
        _defenceStatsMultiText.text = "";
        _defenceStatsValueText.text = "";

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

    private void DisplayHeroes()
    {
        ClearHeroSlots();
        ClearHeroResImages();

        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            if (hero.IsRested && hero.WoundType == WoundsType.Healthy && !hero.InSlot && !hero.IsSentOnQuest)
                CreateHeroSlot(hero);
        }
        //CopyGuildHeroesList();
    }

    private void ClearHeroSlots()
    {
        foreach (Transform hero in _heroesList.transform.Cast<Transform>())
        {
            Destroy(hero.gameObject);
        }
    }

    private void CreateHeroSlot(Hero hero)
    {
        HeroForSendSlot_UI heroSlot = Instantiate(_heroQuestPrefab, _heroesList.transform);
        heroSlot.Init(hero);

        //_heroes.Add(heroSlot);
    }

    public void Selecthero(HeroForSendSlot_UI heroSlot)
    {
        _selectedHero = heroSlot;
    }

    public void TakeHero(HeroForSendSlot_UI hero)
    {
        Hero heroToRemove = _guildKeeper.RecruitSystem.HeroSlots.FirstOrDefault(h => h == hero.Hero);

        //_heroesCopy.Remove(heroToRemove);

        TakeFreeHero?.Invoke(heroToRemove);
        OnChangeRoster?.Invoke();

        ClearHeroSlots();

        foreach (Hero heroSlot in _guildKeeper.RecruitSystem.HeroSlots)
        {
            if (heroSlot.IsRested && heroSlot.WoundType == WoundsType.Healthy && !heroSlot.InSlot && !heroSlot.IsSentOnQuest)
                CreateHeroSlot(heroSlot);
        }

        //foreach (Hero heroCopy in _heroesCopy)
        //{
        //    if (heroCopy.HeroData == null)
        //        continue;

        //    if (heroCopy.IsRested && heroCopy.WoundType == WoundsType.Healthy && !heroCopy.InSlot)
        //        CreateHeroSlot(heroCopy);
        //}

        RefreshInfoHero();
    }

    public void UpdateHeroInfo(Hero hero)
    {
        HeroAbilitiesInfo.ClearAllIcons();

        if (_mainInfo.activeSelf)
        {
            ShowHeroInventory(hero);
            ShowResistanceHero(hero);
            ShowStatsHero(hero);
            ShowInfoHero(hero);
            UpdateHeroExp(hero);
        }

        else if (_abilityHero.activeSelf)
        {
            ShowHeroAbilities(hero);
        }
    }

    public void UpdateHeroExp(Hero hero)
    {
        _expSliderBar.MainSlider.maxValue = hero.LevelSystem.RequiredExp;
        _expSliderBar.MainSlider.value = hero.LevelSystem.CurrentExp;

        _expSliderBar.TextPlace.text = $"Exp: {hero.LevelSystem.CurrentExp} / {hero.LevelSystem.RequiredExp}";

        //_expSlider.value = hero.LevelSystem.CurrentExp;
        //_expSlider.maxValue = hero.LevelSystem.RequiredExp;

        //_expText.text = $"{hero.LevelSystem.CurrentExp} / {hero.LevelSystem.RequiredExp}";
    }

    private void ShowHeroInventory(Hero hero)
    {
        EquipSystem heroEquip = hero.EquipHolder.EquipSystem;
        _equipDisplay.RefreshDynamicInventory(heroEquip, 0);
    }

    private void ShowResistanceHero(Hero hero)
    {
        for (int i = 0; i < _resImages.Length; i++)
        {
            _resImages[i].SetTypeDamage(hero.ResistanceSystem.VisibleAllRes[i].TypeDamage, true);
            _resImages[i].Percent.text = $"{hero.ResistanceSystem.VisibleAllRes[i].ValueResistance.ToString()}%";
        }
    }

    private void ShowStatsHero(Hero hero)
    {
        HeroStats stat = hero.VisibleHeroStats;

        (string, string, string, string) multiText = _statDisplayManager.SetStatsMultiText(stat);
        (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(stat, true);

        _powerStatsNameText.text = statsText.Item1;
        _powerStatsMultiText.text = multiText.Item2;
        _powerStatsValueText.text = statsText.Item2;

        _defenceStatsNameText.text = statsText.Item3;
        _defenceStatsMultiText.text = multiText.Item4;
        _defenceStatsValueText.text = statsText.Item4;

        ShowResistanceHero(hero);
    }

    private void ShowInfoHero(Hero hero)
    {
        _nameHero.text = $"{hero.HeroName}";
        _level.text = $"Level: {hero.HeroLevel}";
        _power.text = $"Power: {hero.HeroPowerPoints.VisibleAllPower}";
        _defence.text = $"Defence: {hero.HeroDefencePoints.VisibleAllDefence}";
    }

    private void ShowHeroAbilities(Hero hero)
    {
        ClearAbilitiesSlots();

        foreach (var ability in hero.ListAbilities)
            CreateAbilitySlot(ability);

        hero.AbilityHolder.ShowAbilities();
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
        _manaCost.text = $"Mana Cost: {abilitySlot_UI.Ability.ManaCost}";

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
                (string, string, string, string) statsText = _statDisplayManager.SetStatsValueText(stat, false);

                _powerStatsName.text = statsText.Item1;
                _powerStatsValue.text = statsText.Item2;

                _defenceStatsName.text = statsText.Item3;
                _defenceStatsValue.text = statsText.Item4;
            }
        }

        if (_selectedHero == null)
            return;

        else
        {
            Hero hero = _heroesCopy.FirstOrDefault(i => i == _selectedHero.Hero);
            bool abilityExists = hero.AbilityHolder.HeroAbilitySystem.AbilityList.Any(ability => ability == abilitySlot_UI.Ability);

            //for (int i = 0; i < _heroAbilityDisplay.AbilitySlots.Count; i++)
            //{
            //    if (_heroAbilityDisplay.AbilitySlots[i].IsSlotClicked == true && !abilityExists)
            //    {
            //        hero.AbilityHolder.HeroAbilitySystem.AddAbility(abilitySlot_UI.Ability, i);
            //    }
            //}

            hero.AbilityHolder.ShowAbilities();
        }

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

    public void UpdateHeroList()
    {
        ClearHeroSlots();

        //_heroesCopy = new List<Hero>();

        //foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        //{
        //    if (!hero.IsSentOnQuest)
        //        _heroesCopy.Add(hero);
        //}

        foreach (Hero hero in _guildKeeper.RecruitSystem.HeroSlots)
        {
            if (hero.IsRested && hero.WoundType == WoundsType.Healthy && !hero.InSlot && !hero.IsSentOnQuest)
                CreateHeroSlot(hero);
        }

        //foreach (Hero heroCopy in _heroesCopy)
        //{
        //    if (heroCopy.HeroData == null)
        //        continue;

        //    if (heroCopy.IsRested && heroCopy.WoundType == WoundsType.Healthy && !heroCopy.InSlot)
        //        CreateHeroSlot(heroCopy);
        //}

        RefreshInfoHero();
    }

    public void ClearHeroResImages()
    {
        for (int i = 0; i < _resImages.Length; i++)
        {
            _resImages[i].Percent.text = $"0%";
        }
    }
}
