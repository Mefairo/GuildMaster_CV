using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[DefaultExecutionOrder(1)]
public class UIController : MonoBehaviour
{
    //[SerializeField] private MenuManager _menuManager;
    [SerializeField] private RecruitKeeperDisplay _recruitKeeperDisplay;
    [SerializeField] private ShopKeeperDisplay _shopKeeperDisplay;
    [SerializeField] private CraftKeeperDisplay _craftKeeperDisplay;
    [SerializeField] private GuildHeroesDisplay _guildHeroes;
    [SerializeField] private QuestKeeperDisplay _questsBoard;
    [SerializeField] private MainQuestKeeperDisplay _mainQuestBoard;
    [SerializeField] private TakingQuestUIController _takingQuestBoard;
    [SerializeField] private WeeklyPaySystem _weeklyPaySystem;
    [SerializeField] private HospitalUIController _hospitalSystem;
    [SerializeField] private ExpeditionalCompany _expeditionalSystem;
    //[SerializeField] private EquipDisplay _equipDisplay;
    [Space]
    [SerializeField] private List<GameObject> _gameObjectActive;

    private void Awake()
    {
        //_menuManager.gameObject.SetActive(false);
        foreach (GameObject obj in _gameObjectActive)
        {
            if (obj.activeSelf)
                obj.gameObject.SetActive(false);
        }

        DisableActiveDisplay();
    }

    private void Start()
    {
        Debug.Log("test start");
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            DisplayMenuManager();
        }

    }

    private void OnEnable()
    {
        RecruitKeeper.OnHeroesWindowRequested += DisplayRecruitWindow;

        ShopKeeper.OnShopWindowRequested += DisplayShopWindow;

        CraftKeeper.OnCraftWindowRequested += DisplayCraftWindow;

        GuildKeeper.OnGuildHeroesWindowRequested += DisplayGuildHeroesWindow;

        //QuestKeeper.OnQuestWindowRequested += DisplayQuestsBoardWindow;

        QuestKeeper.OnSendQuestWindowRequested += DisplayMainQuestsBoardWindow;

        QuestKeeper.OnTakingQuestWindowRequested += DisplayTakingQuestsBoardWindow;

        PayKeeper.OnWeeklyPaySystemOpen += DisplayWeeklyPaySystemWindow;

        HospitalKeeper.OnHospitalSystemOpen += DisplayHospitalSystemWindow;

        ExpeditionalKeeper.OnExpeditionalCompanyOpen += DisplayExpeditionalCompanyWindow;

        //HeroEquipHolder.OnHeroEquipmentRequested += DisplayEquipmentHeroesWindow;
    }

    private void OnDisable()
    {
        RecruitKeeper.OnHeroesWindowRequested -= DisplayRecruitWindow;

        ShopKeeper.OnShopWindowRequested -= DisplayShopWindow;

        CraftKeeper.OnCraftWindowRequested -= DisplayCraftWindow;

        GuildKeeper.OnGuildHeroesWindowRequested -= DisplayGuildHeroesWindow;

        //QuestKeeper.OnQuestWindowRequested -= DisplayQuestsBoardWindow;

        QuestKeeper.OnSendQuestWindowRequested -= DisplayMainQuestsBoardWindow;

        QuestKeeper.OnTakingQuestWindowRequested -= DisplayTakingQuestsBoardWindow;

        PayKeeper.OnWeeklyPaySystemOpen -= DisplayWeeklyPaySystemWindow;

        HospitalKeeper.OnHospitalSystemOpen -= DisplayHospitalSystemWindow;

        ExpeditionalKeeper.OnExpeditionalCompanyOpen -= DisplayExpeditionalCompanyWindow;

        //HeroEquipHolder.OnHeroEquipmentRequested -= DisplayEquipmentHeroesWindow;
    }

    private void DisplayMenuManager()
    {
        bool allInactive = true;

        for (int i = 0; i < _gameObjectActive.Count; i++)
        {
            var obj = _gameObjectActive[i];

            if (obj.gameObject.activeSelf)
            {
                allInactive = false;
                obj.gameObject.SetActive(false);
            }
        }

        if (allInactive)
        {
            //_menuManager.gameObject.SetActive(!_menuManager.gameObject.activeSelf);
        }
    }

    private void DisableActiveDisplay()
    {
        _recruitKeeperDisplay.gameObject.SetActive(false);
        _shopKeeperDisplay.gameObject.SetActive(false);
        _craftKeeperDisplay.gameObject.SetActive(false);
        _guildHeroes.gameObject.SetActive(false);
        _questsBoard.gameObject.SetActive(false);
        //_mainQuestBoard.gameObject.SetActive(false);
        //_equipDisplay.gameObject.SetActive(false);
    }

    private void DisplayRecruitWindow(HeroesRecruitSystem recruitSystem)
    {
        _recruitKeeperDisplay.gameObject.SetActive(true);
        _recruitKeeperDisplay.DisplayHeroWindow(recruitSystem);
    }

    private void DisplayShopWindow(ShopSystem shopSystem, PlayerInventoryHolder playerInventory, ShopTypes shopType, CraftSystem craftSystem)
    {
        _shopKeeperDisplay.gameObject.SetActive(true);
        _shopKeeperDisplay.DisplayShopWindow(shopSystem, playerInventory, shopType, craftSystem);
    }

    private void DisplayCraftWindow(CraftSystem craftSystem, PlayerInventoryHolder playerInventory, ShopTypes craftType, ShopSystem shopSystem)
    {
        _craftKeeperDisplay.gameObject.SetActive(true);
        _craftKeeperDisplay.DisplayCraftWindow(craftSystem, playerInventory, craftType, shopSystem);
    }

    private void DisplayGuildHeroesWindow(HeroesRecruitSystem recruitSystem)
    {
        _guildHeroes.gameObject.SetActive(true);
        _guildHeroes.DisplayHeroWindow(recruitSystem);
    }

    private void DisplayQuestsBoardWindow(QuestBoardSystem boardSystem)
    {
        _questsBoard.gameObject.SetActive(true);
        _questsBoard.DisplayQuestBoard(boardSystem);
    }

    private void DisplayMainQuestsBoardWindow(QuestBoardSystem boardSystem)
    {
        _mainQuestBoard.gameObject.SetActive(true);
        _mainQuestBoard.DisplayQuestBoard(boardSystem);
    }

    private void DisplayTakingQuestsBoardWindow(QuestBoardSystem boardSystem)
    {
        _takingQuestBoard.gameObject.SetActive(true);
        _takingQuestBoard.DisplayQuestBoard(boardSystem);
    }

    private void DisplayWeeklyPaySystemWindow(WeeklyPaySystem paySystem)
    {
        _weeklyPaySystem.gameObject.SetActive(true);
        _weeklyPaySystem.ShowDisplay();
    }

    private void DisplayHospitalSystemWindow(HospitalUIController hospitalSystem)
    {
        _hospitalSystem.gameObject.SetActive(true);
        _hospitalSystem.ShowDisplay();
    }

    private void DisplayExpeditionalCompanyWindow()
    {
        //_expeditionalSystem.gameObject.SetActive(true);
        _expeditionalSystem.ShowDisplay();
    }

    //private void DisplayEquipmentHeroesWindow(EquipSystem equipSystem, int offset)
    //{
    //    _equipDisplay.gameObject.SetActive(true);
    //    _equipDisplay.DisplayEquipWindow(equipSystem, offset);
    //}
}
