using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatDisplayManager
{
    private Dictionary<string, Color> _statColors = new Dictionary<string, Color>
    {
        { "Strength", Color.red },
        { "Dexterity", Color.green },
        { "Intelligence", Color.blue },

         { "Endurance", Color.red },
        { "Flexibility", Color.green },
        { "Sanity", Color.blue },

         { "Crit", new Color(1f, 0.23f, 0.65f) },
          { "Haste",new Color(1f, 0.23f, 0.65f) },
           { "Accuracy", new Color(1f, 0.23f, 0.65f) },

            { "Durability", new Color(0.55f, 0.71f, 1f) },
             { "Adaptability", new Color(0.55f, 0.71f, 1f) },
              { "Suppression",new Color(0.55f, 0.71f, 1f) },

        {"Health", new Color(0.5f, 0f, 0f) },

               { "Other", Color.gray },  // Цвет по умолчанию
        // Добавьте остальные статы и цвета
    };

    public (string, string, string, string) SetRuneStats(RuneSlotData runeSlotData)
    {
        string powerStatsText = "";
        string powerStatsValue = "";
        string defenceStatsText = "";
        string defenceStatsValue = "";

        // Проверка на null
        if (runeSlotData?.RuneSlotStats == null)
        {
            Debug.LogWarning("RuneSlot data is incomplete!");
            return (powerStatsText, powerStatsValue, defenceStatsText, defenceStatsValue);
        }

        // Словарь для сопоставления StatType с именами
        Dictionary<StatType, string> statNames = new Dictionary<StatType, string>
    {
        { StatType.StrengthValue, "Strength" },
        { StatType.DexterityValue, "Dexterity" },
        { StatType.IntelligenceValue, "Intelligence" },
        { StatType.EnduranceValue, "Endurance" },
        { StatType.FlexibilityValue, "Flexibility" },
        { StatType.SanityValue, "Sanity" },
        { StatType.CritValue, "Crit" },
        { StatType.HasteValue, "Haste" },
        { StatType.AccuracyValue, "Accuracy" },
        { StatType.DurabilityValue, "Durability" },
        { StatType.AdaptabilityValue, "Adaptability" },
        { StatType.SuppressionValue, "Suppression" },
        { StatType.Health, "Health" }
    };

        // Создаем словарь со значениями
        //Dictionary<string, int> statsValues = new Dictionary<string, int>();
        Dictionary<string, string> statsValues = new Dictionary<string, string>();

        foreach (var stat in runeSlotData.RuneSlotStats)
        {
            if (statNames.TryGetValue(stat.StatType, out string statName))
            {
                // Используем MaxValueStat или среднее значение (можно изменить)
                var value = $"{stat.MinValueStat} - {stat.MaxValueStat}";
                statsValues[statName] = value;
            }
        }

        foreach (KeyValuePair<string, string> statEntry in statsValues)
        {
            string statName = statEntry.Key;
            var statValue = statEntry.Value;

            if (statValue != null && _statColors.TryGetValue(statName, out Color color))
            {
                string coloredStatName = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statName}:</color>";
                string coloredStatValue = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statValue}</color>";

                if (IsPowerStat(statName))
                {
                    powerStatsText += coloredStatName + "\n";
                    powerStatsValue += coloredStatValue + "\n";
                }

                else
                {
                    defenceStatsText += coloredStatName + "\n";
                    defenceStatsValue += coloredStatValue + "\n";
                }
            }
        }

        return (powerStatsText, powerStatsValue, defenceStatsText, defenceStatsValue);
    }

    public (string, string, string, string) SetStatsValueText(HeroStats stat, bool allStats)
    {
        string powerStatsText = "";
        string powerStatsValue = "";
        string defenceStatsText = "";
        string defenceStatsValue = "";

        Dictionary<string, int> statsValues = new Dictionary<string, int>
                    {
                        { "Strength", stat.StrengthValue },
                        { "Dexterity", stat.DexterityValue },
                        { "Intelligence", stat.IntelligenceValue },
                        { "Endurance", stat.EnduranceValue },
                        { "Flexibility", stat.FlexibilityValue },
                        { "Sanity", stat.SanityValue },
                        { "Crit", stat.CritValue },
                        { "Haste", stat.HasteValue },
                        { "Accuracy", stat.AccuracyValue },
                        { "Durability", stat.DurabilityValue },
                        { "Adaptability", stat.AdaptabilityValue },
                        { "Suppression", stat.SuppressionValue },
                        { "Health", stat.Health }
                    };

        foreach (KeyValuePair<string, int> statEntry in statsValues)
        {
            string statName = statEntry.Key;
            int statValue = statEntry.Value;

            if (allStats)
            {
                if (_statColors.TryGetValue(statName, out Color color))
                {
                    string coloredStatName = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statName}:</color>";
                    string coloredStatValue = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statValue}</color>";

                    if (IsPowerStat(statName))
                    {
                        powerStatsText += coloredStatName + "\n";
                        powerStatsValue += coloredStatValue + "\n";
                    }
                    else
                    {
                        defenceStatsText += coloredStatName + "\n";
                        defenceStatsValue += coloredStatValue + "\n";
                    }
                }
            }

            else
            {
                if (statValue != 0 && _statColors.TryGetValue(statName, out Color color))
                {
                    string coloredStatName = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statName}:</color>";
                    string coloredStatValue = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statValue}</color>";

                    if (IsPowerStat(statName))
                    {
                        powerStatsText += coloredStatName + "\n";
                        powerStatsValue += coloredStatValue + "\n";
                    }
                    else
                    {
                        defenceStatsText += coloredStatName + "\n";
                        defenceStatsValue += coloredStatValue + "\n";
                    }
                }
            }


        }

        return (powerStatsText, powerStatsValue, defenceStatsText, defenceStatsValue);
    }

    public (string, string, string, string) SetStatsMultiText(HeroStats stat)
    {
        string powerStatsText = "";
        string powerStatsMulti = "";
        string defenceStatsText = "";
        string defenceStatsMulti = "";

        Dictionary<string, int> statsMulties = new Dictionary<string, int>
                    {
                        { "Strength", stat.StrengthMulti },
                        { "Dexterity", stat.DexterityMulti },
                        { "Intelligence", stat.IntelligenceMulti },
                        { "Endurance", stat.EnduranceMulti },
                        { "Flexibility", stat.FlexibilityMulti },
                        { "Sanity", stat.SanityMulti },
                        { "Crit", stat.CritMulti },
                        { "Haste", stat.HasteMulti },
                        { "Accuracy", stat.AccuracyMulti },
                        { "Durability", stat.DurabilityMulti },
                        { "Adaptability", stat.AdaptabilityMulti },
                        { "Suppression", stat.SuppressionMulti }
                    };

        foreach (KeyValuePair<string, int> statEntry in statsMulties)
        {
            string statName = statEntry.Key;
            int statValue = statEntry.Value;

            if (_statColors.TryGetValue(statName, out Color color))
            {
                string coloredStatName = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statName}:</color>";
                //string coloredStatMulti = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statValue}</color>";
                string coloredStatMulti = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>(x{statValue})</color>";

                if (IsPowerStat(statName))
                {
                    powerStatsText += coloredStatName + "\n";
                    powerStatsMulti += coloredStatMulti + "\n";
                }
                else
                {
                    defenceStatsText += coloredStatName + "\n";
                    defenceStatsMulti += coloredStatMulti + "\n";
                }
            }
        }

        return (powerStatsText, powerStatsMulti, defenceStatsText, defenceStatsMulti);
    }

    public (string, string) SetRequiredStatsValueText(HeroStats stat)
    {
        string statsNameText = "";
        string statsValueText = "";

        Dictionary<string, int> statsValues = new Dictionary<string, int>
                    {
                         { "Strength", stat.StrengthMulti },
                        { "Dexterity", stat.DexterityMulti },
                        { "Intelligence", stat.IntelligenceMulti },
                        { "Endurance", stat.EnduranceMulti },
                        { "Flexibility", stat.FlexibilityMulti },
                        { "Sanity", stat.SanityMulti },
                        { "Crit", stat.CritMulti },
                        { "Haste", stat.HasteMulti },
                        { "Accuracy", stat.AccuracyMulti },
                        { "Durability", stat.DurabilityMulti },
                        { "Adaptability", stat.AdaptabilityMulti },
                        { "Suppression", stat.SuppressionMulti }
                    };

        foreach (KeyValuePair<string, int> statEntry in statsValues)
        {
            string statName = statEntry.Key;
            int statValue = statEntry.Value;

            if (statValue != 0 && _statColors.TryGetValue(statName, out Color color))
            {
                string coloredStatName = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statName}:</color>";
                string coloredStatValue = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statValue}</color>";

                statsNameText += coloredStatName + "\n";
                statsValueText += coloredStatValue + "\n";
            }
        }

        return (statsNameText, statsValueText);
    }

    public bool CheckStats(HeroStats heroStats, HeroStats abilityStats)
    {
        Dictionary<string, int> heroStatsValues = new Dictionary<string, int>
                    {
                         { "Strength", heroStats.StrengthMulti },
                        { "Dexterity", heroStats.DexterityMulti },
                        { "Intelligence", heroStats.IntelligenceMulti },
                        { "Endurance", heroStats.EnduranceMulti },
                        { "Flexibility", heroStats.FlexibilityMulti },
                        { "Sanity", heroStats.SanityMulti },
                        { "Crit", heroStats.CritMulti },
                        { "Haste", heroStats.HasteMulti },
                        { "Accuracy", heroStats.AccuracyMulti },
                        { "Durability", heroStats.DurabilityMulti },
                        { "Adaptability", heroStats.AdaptabilityMulti },
                        { "Suppression", heroStats.SuppressionMulti }
                    };

        Dictionary<string, int> abilityStatsValues = new Dictionary<string, int>
                    {
                         { "Strength", abilityStats.StrengthMulti },
                        { "Dexterity", abilityStats.DexterityMulti },
                        { "Intelligence", abilityStats.IntelligenceMulti },
                        { "Endurance", abilityStats.EnduranceMulti },
                        { "Flexibility", abilityStats.FlexibilityMulti },
                        { "Sanity", abilityStats.SanityMulti },
                        { "Crit", abilityStats.CritMulti },
                        { "Haste", abilityStats.HasteMulti },
                        { "Accuracy", abilityStats.AccuracyMulti },
                        { "Durability", abilityStats.DurabilityMulti },
                        { "Adaptability", abilityStats.AdaptabilityMulti },
                        { "Suppression", abilityStats.SuppressionMulti }
                    };

        int amountRequiredStats = 0;

        foreach (KeyValuePair<string, int> abilityStat in abilityStatsValues)
        {
            string statName = abilityStat.Key;
            int statValue = abilityStat.Value;

            if (statValue != 0)
            {
                amountRequiredStats++;

                if (heroStatsValues.TryGetValue(statName, out int value))
                {
                    if (statValue <= value)
                        amountRequiredStats--;
                }
            }
        }

        if (amountRequiredStats == 0)
            return true;

        else
            return false;
    }

    public (string, string, string, string) SetAllStatsText(HeroStats stat, bool allStats)
    {
        string powerStatsText = "";
        string powerStatsValue = "";
        string defenceStatsText = "";
        string defenceStatsValue = "";

        Dictionary<string, (int, int)> stats = new Dictionary<string, (int, int)>
                    {
                        { "Strength", (stat.StrengthMulti,  stat.StrengthValue) },
                        { "Dexterity", (stat.DexterityMulti,  stat.DexterityValue) },
                        { "Intelligence", (stat.IntelligenceMulti,  stat.IntelligenceValue) },
                        { "Endurance", (stat.EnduranceMulti, stat.EnduranceValue ) },
                        { "Flexibility", (stat.FlexibilityMulti,  stat.FlexibilityValue)},
                        { "Sanity", (stat.SanityMulti , stat.SanityValue)},
                        { "Crit", (stat.CritMulti , stat.CritValue )},
                        { "Haste", (stat.HasteMulti , stat.HasteValue)},
                        { "Accuracy", (stat.AccuracyMulti , stat.AccuracyValue)},
                        { "Durability", (stat.DurabilityMulti ,  stat.DurabilityValue)},
                        { "Adaptability", (stat.AdaptabilityMulti ,  stat.AdaptabilityValue)},
                        { "Suppression", (stat.SuppressionMulti,  stat.SuppressionValue ) },
                    };

        foreach (KeyValuePair<string, (int, int)> statEntry in stats)
        {
            string statName = statEntry.Key;
            int statMulti = statEntry.Value.Item1;
            int statValue = statEntry.Value.Item2;

            if (allStats)
            {
                if (_statColors.TryGetValue(statName, out Color color))
                {
                    string coloredStatName = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statName}:</color>";
                    string coloredStatValue = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statValue}(x{statMulti})</color>";

                    if (IsPowerStat(statName))
                    {
                        powerStatsText += coloredStatName + "\n";
                        powerStatsValue += coloredStatValue + "\n";
                    }
                    else
                    {
                        defenceStatsText += coloredStatName + "\n";
                        defenceStatsValue += coloredStatValue + "\n";
                    }
                }
            }

            else
            {
                if ((statValue != 0 || statMulti != 0) && _statColors.TryGetValue(statName, out Color color))
                {
                    string coloredStatName = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statName}:</color>";
                    string coloredStatValue = $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{statValue}(x{statMulti})</color>";

                    if (IsPowerStat(statName))
                    {
                        powerStatsText += coloredStatName + "\n";
                        powerStatsValue += coloredStatValue + "\n";
                    }
                    else
                    {
                        defenceStatsText += coloredStatName + "\n";
                        defenceStatsValue += coloredStatValue + "\n";
                    }
                }
            }


        }

        return (powerStatsText, powerStatsValue, defenceStatsText, defenceStatsValue);
    }

    private bool IsPowerStat(string statName)
    {
        // Список атакующих статов
        HashSet<string> powerStats = new HashSet<string> { "Strength", "Dexterity", "Intelligence", "Crit", "Haste", "Accuracy" };

        return powerStats.Contains(statName);
    }

    private bool IsDefenceStat(string statName)
    {
        // Список атакующих статов
        HashSet<string> powerStats = new HashSet<string> { "Endurance", "Flexibility", "Sanity", "Durability", "Adaptability", "Suppression" };

        return powerStats.Contains(statName);
    }
}
