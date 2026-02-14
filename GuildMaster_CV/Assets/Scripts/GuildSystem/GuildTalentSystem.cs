using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuildTalentSystem : MonoBehaviour
{
    [Header("GameObjects")]
    [SerializeField] private GuildValutes _guild;
    [Header("UI")]
    [SerializeField] private Image _guildTalentPanel;
    [SerializeField] private TextMeshProUGUI _nameTalent;
    [SerializeField] private TextMeshProUGUI _descriptionTalent;
    [SerializeField] private TextMeshProUGUI _requiredPoints;
    [SerializeField] private TextMeshProUGUI _availablePoints;
    [SerializeField] private Button _takeButton;
    [Header("Properties")]
    [SerializeField] private GuildTalentSlot_UI _selectedSlot;
    [Header("Slots")]
    [SerializeField] private ExtraPowerStats_Talent _powerUpStats;
    [SerializeField] private ExtraDefenceStats_Talent _defenceUpStats;
    [SerializeField] private ExtraResForHero_Talent _resUpStats;
    [SerializeField] private ExtraHPHero_Talent _healthUpStats;
    [SerializeField] private ExtraAbilitySlot _extraAbilitySlot;

    public ExtraPowerStats_Talent PowerUpStats => _powerUpStats;
    public ExtraDefenceStats_Talent DefenceUpStats => _defenceUpStats;
    public ExtraResForHero_Talent ResUpStats => _resUpStats;
    public ExtraHPHero_Talent HealthUpStats => _healthUpStats;
    public ExtraAbilitySlot ExtraAbilitySlot => _extraAbilitySlot;

    private void Awake()
    {
        _takeButton?.onClick.AddListener(TakeTalent);
        _guildTalentPanel.gameObject.SetActive(false);

        ClearUIInfo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _guildTalentPanel.gameObject.SetActive(false);
            ClearUIInfo();
        }
    }

    private void OnEnable()
    {
        GuildValutes.OnChangeGuildPoints += UpdateUIInfo;
    }

    private void OnDisable()
    {
        GuildValutes.OnChangeGuildPoints -= UpdateUIInfo;
    }

    public void OpenPanel()
    {
        _guildTalentPanel.gameObject.SetActive(true);

        ClearUIInfo();
    }

    public void SelectedSlot(GuildTalentSlot_UI talentSlot)
    {
        if (_selectedSlot != null)
        {
            ColorBlock cbLast = _selectedSlot.ButtonSelf.colors;
            cbLast.normalColor = Color.black;
            _selectedSlot.ButtonSelf.colors = cbLast;
        }

        _selectedSlot = talentSlot;

        ColorBlock cbCurrent = _selectedSlot.ButtonSelf.colors;
        cbCurrent.normalColor = Color.yellow;
        _selectedSlot.ButtonSelf.colors = cbCurrent;

        UpdateUIInfo(talentSlot);
    }

    private void UpdateUIInfo(GuildTalentSlot_UI talentSlot)
    {
        _nameTalent.text = talentSlot.NameTalent;
        _descriptionTalent.text = talentSlot.DescriptionTalent;
        _requiredPoints.text = $"Points required: {talentSlot.RequiredPoints}";
    }

    private void UpdateUIInfo()
    {
        _nameTalent.text = _selectedSlot.NameTalent;
        _descriptionTalent.text = _selectedSlot.DescriptionTalent;
        _requiredPoints.text = $"Points required: {_selectedSlot.RequiredPoints}";
        _availablePoints.text = $"Available Points: {_guild.LevelPoints}";
    }

    private void ClearUIInfo()
    {
        _nameTalent.text = "";
        _descriptionTalent.text = "";
        _requiredPoints.text = "Points required:";
        _availablePoints.text = $"Available Points: {_guild.LevelPoints}";

        if (_selectedSlot != null)
        {
            ColorBlock cb = _selectedSlot.ButtonSelf.colors;
            cb.normalColor = Color.black;
            _selectedSlot.ButtonSelf.colors = cb;
        }

        _selectedSlot = null;
    }

    private void TakeTalent()
    {
        if (_selectedSlot != null)
        {
            if (_guild.LevelPoints >= _selectedSlot.RequiredPoints && !_selectedSlot.IsTakenTalent)
            {
                _selectedSlot.RealizationTalent();
                _guild.ChangeGuildPoints(_selectedSlot.RequiredPoints);
                NotificationSystem.Instance.CheckNotifications();
            }
        }
    }
}
