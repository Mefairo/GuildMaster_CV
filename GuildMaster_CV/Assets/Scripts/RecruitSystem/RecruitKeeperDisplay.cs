using UnityEngine;
using UnityEngine.UI;



public class RecruitKeeperDisplay : KeeperDisplay
{
    [SerializeField] private Button _buttonHire;

    protected override void Awake()
    {
        base.Awake();
        _buttonHire.onClick.AddListener(HireHero);
    }


    private void HireHero()
    {
        if (_guildValutes.CurrentHeroPlaces < _guildValutes.MaxHeroPlaces)
        {
            if (_selectedHero != null)
            {
                OnHireNewHero?.Invoke(_selectedHero.Hero);

                _recruitSystem.HeroSlots.Remove(_selectedHero.Hero);
                _heroesList.Remove(_selectedHero);

                Destroy(_selectedHero.gameObject);
                _selectedHero = null;
            }
        }

        RefreshDisplay();
        RefreshInfo();
        DisplayHeroes();

        if (_heroesList.Count > 0 && _heroesList[0] != null)
        {
            _selectedHero = _heroesList[0];
            UpdateHeroPreview(_selectedHero);
        }

        NotificationSystem.Instance.CheckNotifications();
    }
}
