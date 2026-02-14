using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMonobeh : MonoBehaviour
{
    [SerializeField] private HeroData _heroData;
    [SerializeField] private HeroPowerPoints _powerPoints;
    [SerializeField] private HeroDefencePoints _defencePoints;
    [SerializeField] private List<Resistance> _resistance;
}
