using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class LevelSystem
{
    [SerializeField] private int _level;
    [Header("Experience")]
    [SerializeField] private int _currentExp;
    [SerializeField] private int _requiredExp;
    [SerializeField] private int _addExp;
    [Header("Points")]
    [SerializeField] private int _statPoints;
    [SerializeField] private int _abilityPoints;
    [SerializeField] private int _addStatPoints;
    [Header("Resistance")]
    [SerializeField] private int _resForLevel;

    public int Level => _level;
    public int CurrentExp => _currentExp;
    public int RequiredExp => _requiredExp;
    public int AddExp => _addExp;
    public int StatPoints => _statPoints;
    public int AbilityPoints => _abilityPoints;

    //public event UnityAction<int> OnLevelUp; // Событие для повышения уровня

    public event UnityAction<int> OnChangeLevel;
    public event UnityAction<int> OnResTalentTaken;

    public LevelSystem(int level, int currentExp, int requiredExp) // ДЛЯ КОПИИ ГЕРОЯ ВЗЯТОГО КВЕСТА
    {
        _level = level;
        _currentExp = currentExp;
        _requiredExp = requiredExp;
    }

    public LevelSystem(int level)
    {
        _level = level;
        _currentExp = 0;
        //_requiredExp = level * 100;
        CheckedRequiredHeroExperience();
        _statPoints = (_level - 1) * 2;
        _abilityPoints = _level - 1;
        _addStatPoints = 2;
        _resForLevel = 0;
    }

    public (int, bool) ChangeExperience(int exp)
    {
        bool isLevelUp = false;

        _addExp = exp;
        _currentExp += exp;

        while (_currentExp >= _requiredExp)
        {
            // Вычисляем остаток опыта после достижения текущего уровня
            _currentExp -= _requiredExp;
            _level++;
            _statPoints += _addStatPoints;
            _abilityPoints += 1;

            isLevelUp = true;

            // Генерируем события обновления
            OnChangeLevel?.Invoke(_level);
            OnResTalentTaken?.Invoke(_resForLevel);

            CheckedRequiredHeroExperience();
        }

        return (_level, isLevelUp);
    }

    public void ChangeStatPoints(int value)
    {
        _statPoints += value;
    }

    public void ChangeExtraStatPoints(int statValue)
    {
        _addStatPoints += statValue;
    }

    public void SpendAbilityPoints()
    {
        _abilityPoints--;
        NotificationSystem.Instance.CheckNotifications();
    }

    public void UpResistanceForLevelByTalent(int resValue)
    {
        _resForLevel += resValue;
    }

    private void CheckedRequiredHeroExperience()
    {
        switch (_level)
        {
            case 1:
                _requiredExp = 150;
                break;

            case 2:
                _requiredExp = 1200;
                break;

            case 3:
                _requiredExp = 9600;
                break;

            case 4:
                _requiredExp = 76800;
                break;

            case 5:
                _requiredExp = 614400;
                break;
        }
    }
}
