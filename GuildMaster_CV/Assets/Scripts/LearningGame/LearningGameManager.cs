using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LearningGameManager : MonoBehaviour
{
    public static LearningGameManager Instance;

    [SerializeField] private Image _panel;
    [SerializeField] private TextMeshProUGUI _mainText;
    [SerializeField] private Button _next;
    [SerializeField] private Button _back;
    [SerializeField] private int _slideIndex;
    [SerializeField] private List<StringListContainer> _currentList = new List<StringListContainer>();
    [Header("Content Lists")]
    [SerializeField] private List<StringListContainer> _startGameHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _questBoardHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _shopHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _craftHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _hospitalHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _recruitHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _expeditiontHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _guildHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _abilityTreeHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _payTreeHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _abiltyInfoHelp = new List<StringListContainer>();
    [SerializeField] private List<StringListContainer> _reforgeInfoHelp = new List<StringListContainer>();
    [Header("Checks")]
    [SerializeField] private bool _questBoardCheck;
    [SerializeField] private bool _shopHelpCheck;
    [SerializeField] private bool _craftHelpCheck;
    [SerializeField] private bool _hospitalHelpCheck;
    [SerializeField] private bool _recruitHelpCheck;
    [SerializeField] private bool _expeditionHelpCheck;
    [SerializeField] private bool _guildHelpCheck;
    [SerializeField] private bool _abilityTreeHelpCheck;
    [SerializeField] private bool _payHelpCheck;
    [SerializeField] private bool _abilityHelpCheck;
    [SerializeField] private bool _reforgeHelpCheck;
    [Header("Learning Buttons")]
    [SerializeField] private Button _questBoardButton;
    [SerializeField] private Button _shopHelpButton;
    [SerializeField] private Button _craftHelpButton;
    [SerializeField] private Button _hospitalHelpButton;
    [SerializeField] private Button _recruitHelpButton;
    [SerializeField] private Button _expeditionHelpButton;
    [SerializeField] private Button _guildHelpButton;
    [SerializeField] private Button _abilityTreeHelpButton;
    [SerializeField] private Button _payHelpButton;
    [SerializeField] private Button _abilityInfoHelpButton;
    [SerializeField] private Button _reforgeInfoHelpButton;

    public Image Panel => _panel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            return;
        }

        else
            Destroy(gameObject);
    }

    private void Start()
    {
        _panel.gameObject.SetActive(true);

        _next.gameObject.SetActive(false);
        _back.gameObject.SetActive(false);

        _mainText.text = "";

        _next?.onClick.AddListener(NextSlideWrapper);
        _back?.onClick.AddListener(BackSlideWrapper);

        //_questBoardButton.onClick.AddListener(QuestBoardHelperByButton);
        _shopHelpButton?.onClick.AddListener(ShopHelperByButton);
        _craftHelpButton?.onClick.AddListener(CraftHelperByButton);
        _hospitalHelpButton?.onClick.AddListener(HospitalHelperByButton);
        _recruitHelpButton?.onClick.AddListener(RecruitHelperByButton);
        _expeditionHelpButton?.onClick.AddListener(ExpeditionHelperByButton);
        _guildHelpButton?.onClick.AddListener(GuildHelperByButton);
        _abilityTreeHelpButton?.onClick.AddListener(AbilityTreeHelperByButton);
        _payHelpButton?.onClick.AddListener(PayHelperByButton);
        _reforgeInfoHelpButton?.onClick.AddListener(ReforgeHelperByButton);

        SetText(_startGameHelp);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (_currentList != null && _currentList == _startGameHelp)
            {
                _panel.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        QuestKeeper.OnQuestWindowRequested += QuestBoardHelper;
        ShopKeeper.OnShopWindowRequestedForLearn += ShopHelper;
        ShopKeeperDisplay.OnCraftWindowOpen += CraftHelper;
        HospitalKeeper.OnHospitalSystemOpen += HospitalHelper;
        RecruitKeeper.OnHeroesWindowRequested += RecruitHelper;
        ExpeditionalKeeper.OnExpeditionalCompanyOpen += ExpeditionHelper;
        GuildKeeper.OnGuildHeroesWindowRequested += GuildHelper;
        AbilityTree.OnAbilityTreeRequested += AbilityTreeHelper;
        PayKeeper.OnWeeklyPaySystemOpen += PayHelper;
        PrepareQuestUIController.OnAbilityButtonClick += AbilityInfoHelper;
        ReforgeController.OnReforgePanelOpen += ReforgeHelper;
    }

    private void OnDisable()
    {
        QuestKeeper.OnQuestWindowRequested -= QuestBoardHelper;
        ShopKeeper.OnShopWindowRequestedForLearn -= ShopHelper;
        ShopKeeperDisplay.OnCraftWindowOpen -= CraftHelper;
        HospitalKeeper.OnHospitalSystemOpen -= HospitalHelper;
        RecruitKeeper.OnHeroesWindowRequested -= RecruitHelper;
        ExpeditionalKeeper.OnExpeditionalCompanyOpen -= ExpeditionHelper;
        GuildKeeper.OnGuildHeroesWindowRequested -= GuildHelper;
        AbilityTree.OnAbilityTreeRequested -= AbilityTreeHelper;
        PayKeeper.OnWeeklyPaySystemOpen -= PayHelper;
        PrepareQuestUIController.OnAbilityButtonClick -= AbilityInfoHelper;
        ReforgeController.OnReforgePanelOpen -= ReforgeHelper;
    }

    private void NextSlide(List<StringListContainer> list)
    {
        _slideIndex++;

        if (_slideIndex == list.Count - 1)
        {
            _back.gameObject.SetActive(true);
            _next.gameObject.SetActive(false);
        }

        else
        {
            _back.gameObject.SetActive(true);
            _next.gameObject.SetActive(true);
        }

        _mainText.text = list[_slideIndex].Text;
    }

    private void BackSlide(List<StringListContainer> list)
    {
        _slideIndex--;

        if (_slideIndex == 0)
        {
            _back.gameObject.SetActive(false);
            _next.gameObject.SetActive(true);
        }

        else
        {
            _back.gameObject.SetActive(true);
            _next.gameObject.SetActive(true);
        }

        _mainText.text = list[_slideIndex].Text;
    }

    public void SetText(List<StringListContainer> list)
    {
        _slideIndex = 0;
        _currentList = list;
        _panel.gameObject.SetActive(true);

        if (list.Count == 1)
        {
            _back.gameObject.SetActive(false);
            _next.gameObject.SetActive(false);
        }

        else
        {
            _back.gameObject.SetActive(false);
            _next.gameObject.SetActive(true);
        }

        _mainText.text = list[_slideIndex].Text;
    }

    private void QuestBoardHelper()
    {
        if (_questBoardCheck == false)
        {
            _questBoardCheck = true;
            SetText(_questBoardHelp);
        }
    }

    private void QuestBoardHelperByButton()
    {
        SetText(_questBoardHelp);
    }

    private void ShopHelper()
    {
        if (_shopHelpCheck == false)
        {
            _shopHelpCheck = true;
            SetText(_shopHelp);
        }
    }

    private void ShopHelperByButton()
    {
        SetText(_shopHelp);
    }

    private void CraftHelper()
    {
        if (_craftHelpCheck == false)
        {
            _craftHelpCheck = true;
            SetText(_craftHelp);
        }
    }

    private void CraftHelperByButton()
    {
        SetText(_craftHelp);
    }

    private void HospitalHelper(HospitalUIController nul)
    {
        if (_hospitalHelpCheck == false)
        {
            _hospitalHelpCheck = true;
            SetText(_hospitalHelp);
        }
    }

    private void HospitalHelperByButton()
    {
        SetText(_hospitalHelp);
    }

    private void RecruitHelper(HeroesRecruitSystem nul)
    {
        if (_recruitHelpCheck == false)
        {
            _recruitHelpCheck = true;
            SetText(_recruitHelp);
        }
    }

    private void RecruitHelperByButton()
    {
        SetText(_recruitHelp);
    }

    private void ExpeditionHelper()
    {
        if (_expeditionHelpCheck == false)
        {
            _expeditionHelpCheck = true;
            SetText(_expeditiontHelp);
        }
    }

    private void ExpeditionHelperByButton()
    {
        SetText(_expeditiontHelp);
    }

    private void GuildHelper(HeroesRecruitSystem nul)
    {
        if (_guildHelpCheck == false)
        {
            _guildHelpCheck = true;
            SetText(_guildHelp);
        }
    }

    private void ReforgeHelper()
    {
        if (_reforgeHelpCheck == false)
        {
            _reforgeHelpCheck = true;
            SetText(_reforgeInfoHelp);
        }
    }

    private void ReforgeHelperByButton()
    {
        SetText(_reforgeInfoHelp);
    }

    private void GuildHelperByButton()
    {
        SetText(_guildHelp);
    }

    private void AbilityTreeHelper()
    {
        if (_abilityTreeHelpCheck == false)
        {
            _abilityTreeHelpCheck = true;
            SetText(_abilityTreeHelp);
        }
    }

    private void AbilityTreeHelperByButton()
    {
        SetText(_abilityTreeHelp);
    }

    private void PayHelper(WeeklyPaySystem nul)
    {
        if (_payHelpCheck == false)
        {
            _payHelpCheck = true;
            SetText(_payTreeHelp);
        }
    }

    private void PayHelperByButton()
    {
        SetText(_payTreeHelp);
    }

    private void AbilityInfoHelper()
    {
        if (_abilityHelpCheck == false)
        {
            _abilityHelpCheck = true;
            SetText(_abiltyInfoHelp);
        }
    }

    private void NextSlideWrapper()
    {
        NextSlide(_currentList);
    }

    private void BackSlideWrapper()
    {
        BackSlide(_currentList);
    }
}
