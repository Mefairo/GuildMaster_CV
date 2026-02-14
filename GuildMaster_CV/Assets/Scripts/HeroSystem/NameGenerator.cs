using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class NameGenerator 
{
    private static string[] _heroNames = {
    // 1-20
    "Vexis", "Korran", "Thalrik", "Zarras", "Mordren",
    "Sylros", "Harken", "Voryn", "Quillis", "Dalken",
    "Xanthar", "Brellis", "Nexar", "Yrris", "Tovarn",
    "Pyrris", "Gorvahn", "Lystra", "Jarrik", "Fyxis",
    
    // 21-40
    "Rethis", "Vosk", "Keldar", "Zorvik", "Myrthas",
    "Pellas", "Ghorren", "Xyris", "Nethro", "Bryx",
    "Sorthun", "Dravik", "Qyro", "Jyrren", "Vhoss",
    "Tarnis", "Phyron", "Klyros", "Wyrth", "Zethis",
    
    // 41-60
    "Ghartok", "Mavren", "Syrrik", "Thyx", "Orvahn",
    "Pryxis", "Ghelren", "Vixas", "Kyrro", "Noxar",
    "Bhelren", "Zyras", "Fyrth", "Krivis", "Tyxar",
    "Hyrren", "Vexar", "Jorth", "Klyx", "Myrth",
    
    // 61-80
    "Phyxis", "Qhorrik", "Rovahn", "Syrth", "Tarvix",
    "Vhyrro", "Wexis", "Xyrren", "Ythrik", "Zorvix",
    "Arvahn", "Bhorris", "Chedren", "Dyrth", "Exar",
    "Fhorrik", "Gryx", "Hyrtis", "Irryn", "Jyxis",
    
    // 81-100
    "Korth", "Lyxar", "Moxis", "Nyxar", "Othrik",
    "Pherren", "Qyxis", "Rhyx", "Sythar", "Tryx",
    "Uvrik", "Vhyx", "Wyrrik", "Xavren", "Ythar",
    "Zyrro", "Aexis", "Byth", "Cyxar", "Dhorren"
};

    public string GetRandomName()
    {
        return _heroNames[Random.Range(0, _heroNames.Length)];
    }
}
