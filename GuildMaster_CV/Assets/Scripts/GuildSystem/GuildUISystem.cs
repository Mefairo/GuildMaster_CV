using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuildUISystem : MonoBehaviour
{
    [SerializeField] private GuildValutes _guildValutes;
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private TextMeshProUGUI _currentRep;
    [SerializeField] private TextMeshProUGUI _requiredRep;
    [SerializeField] private TextMeshProUGUI _gold;

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        _guildValutes.OnChangeLevel += UpdateLevel;
        _guildValutes.OnChangeCurrentRep += UpdateCurrentRep;
        _guildValutes.OnChangeRequiredRep += UpdateRequiredRep;
        _guildValutes.OnChangeGold += UpdateGold;
    }

    private void OnDisable()
    {
        _guildValutes.OnChangeLevel -= UpdateLevel;
        _guildValutes.OnChangeCurrentRep -= UpdateCurrentRep;
        _guildValutes.OnChangeRequiredRep -= UpdateRequiredRep;
        _guildValutes.OnChangeGold -= UpdateGold;
    }

    private void UpdateUI()
    {
        _level.text = $"Level: {_guildValutes.Level}";
        _currentRep.text = $"Reputation:   {_guildValutes.CurrentRep}";
        _requiredRep.text = $"{_guildValutes.RequiredRep}";
        _gold.text = $"{_guildValutes.Gold}";
    }

    private void UpdateLevel(int level)
    {
        _level.text = $"Level: {level}";
    }

    private void UpdateCurrentRep(int currentRep)
    {
        _currentRep.text = $"Reputation:   {currentRep}";
    }

    private void UpdateRequiredRep(int requiredRep)
    {
        _requiredRep.text = $"{requiredRep}";
    }

    private void UpdateGold(int gold)
    {
        _gold.text = $"{gold}";
    }
}
