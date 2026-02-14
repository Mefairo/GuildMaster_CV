using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class TextHighlighter
{
    //[SerializeField] private TextMeshProUGUI _changeColorText;

    private Dictionary<string, Color> _wordColors = new Dictionary<string, Color>
    {
         { "Other", Color.gray },  // ÷вет по умолчанию


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
         { "Critical", new Color(1f, 0.23f, 0.65f) },

        {"Physical",new Color(0.98f, 0.76f, 0.53f)},
        {"Fire", new Color(1f, 0.5f, 0f) },
        {"Cold", new Color(0.31f, 0.76f, 1f) },
        {"Lightning", new Color(1, 0.98f, 0.3f) },
        {"Light", new Color(1, 1f, 1f) },
        {"Dark", new Color(0.4f, 0f, 0.43f) },
        {"Necrotic", new Color(0f, 1f, 0.67f) },

         

           { "Blood Infection", new Color(0.6f, 0.3f, 0.97f) },
           { "Burn", new Color(0.6f, 0.3f, 0.97f) },
           { "Curse", new Color(0.6f, 0.3f, 0.97f) },
           { "Fracture", new Color(0.6f, 0.3f, 0.97f) },
           { "Freeze", new Color(0.6f, 0.3f, 0.97f) },
           { "Lightleak", new Color(0.6f, 0.3f, 0.97f) },
           { "Shock", new Color(0.6f, 0.3f, 0.97f) },

           { "Sapping", new Color(0.6f, 0.3f, 0.97f) },
           { "Vulnerability", new Color(0.6f, 0.3f, 0.97f) },

           { "Brittleness", new Color(0.6f, 0.3f, 0.97f) },
           { "Exhaustion", new Color(0.6f, 0.3f, 0.97f) },
           { "Immobilization", new Color(0.6f, 0.3f, 0.97f) },
           { "Madness", new Color(0.6f, 0.3f, 0.97f) },
           { "Penetration", new Color(0.6f, 0.3f, 0.97f) },
           { "Petrification", new Color(0.6f, 0.3f, 0.97f) },

           { "Blinding", new Color(0.6f, 0.3f, 0.97f) },
           { "Bog", new Color(0.6f, 0.3f, 0.97f) },
           { "Charm", new Color(0.6f, 0.3f, 0.97f) },
           { "Pacification", new Color(0.6f, 0.3f, 0.97f) },
           { "Slow", new Color(0.6f, 0.3f, 0.97f) },
           { "Weakness", new Color(0.6f, 0.3f, 0.97f) },

           { "Bleeding", new Color(0.6f, 0.3f, 0.97f) },
           { "Bleed", new Color(0.6f, 0.3f, 0.97f) },
           { "Fear", new Color(0.6f, 0.3f, 0.97f) },
           { "Silence", new Color(0.6f, 0.3f, 0.97f) },
           { "Withering", new Color(0.6f, 0.3f, 0.97f) },



       
    };

    public string ChangeColorText(string originalText)
    {
        string newText = originalText;

        // —ортировка слов по длине, чтобы длинные слова замен€лись раньше коротких
        foreach (var wordPair in _wordColors.OrderByDescending(w => w.Key.Length))
        {
            string colorHex = ColorUtility.ToHtmlStringRGB(wordPair.Value);
            newText = Regex.Replace(newText, @"\b" + Regex.Escape(wordPair.Key) + @"\b", $"<color=#{colorHex}>{wordPair.Key}</color>");
        }

        return newText;
    }
}
