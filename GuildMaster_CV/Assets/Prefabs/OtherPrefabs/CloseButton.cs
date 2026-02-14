using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    [SerializeField] private Button _buttonSelf;
    [SerializeField] private bool _isJustButton;
    [SerializeField] private GameObject _panel_1;
    [SerializeField] private List<GameObject> _firstClosedObjects = new List<GameObject>();
    [Header("Gameobjects")]
    [SerializeField] private MainQuestKeeperDisplay _mainQuestKeeperDisplay;
    [SerializeField] private QuestParametresSystem _questParametresSystem;
    [SerializeField] private QuestParametres _questParametres;
    [SerializeField] private AbilityTree _abilityTree;

    private void Awake()
    {
        _buttonSelf?.onClick.AddListener(ClosePanelByButton);
    }

    private void OnEnable()
    {
        NextDayController.OnNextDayStatic += ClosePanelByEscape;
    }

    private void OnDisable()
    {
        NextDayController.OnNextDayStatic -= ClosePanelByEscape;

        if (_abilityTree != null)
            _abilityTree.ClosePanel();

        if (_questParametresSystem != null)
            _questParametresSystem.CalculateAbilitiesSystem.HeroAuraAbilities.Clear();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanelByEscape();
        }
    }

    private void ClosePanelByEscape()
    {
        if (!_isJustButton)
        {
            if (_panel_1 != null)
            {
                if (_firstClosedObjects.Count != 0)
                {
                    foreach (GameObject panel in _firstClosedObjects)
                    {
                        if (panel != null && panel.activeSelf && _panel_1.activeSelf)
                        {
                            panel.SetActive(false); // Закрываем первую найденную
                            return; // Выходим, не трогая главную
                        }
                    }

                    // 2. Если неглавных панелей нет — закрываем главную
                    if (_panel_1.activeSelf)
                    {
                        _panel_1.SetActive(false);
                    }
                }

                else
                {
                    _panel_1.gameObject.SetActive(false);
                }
            }

            if (_abilityTree != null)
                _abilityTree.ClosePanel();

            if(_questParametres != null)
                _questParametres.ClearInfo();
        }
    }

    private void ClosePanelByButton()
    {
        if (_panel_1 != null)
        {
            if (_abilityTree != null)
                _abilityTree.ClosePanel();
        }

        _panel_1.gameObject.SetActive(false);
    }
}
