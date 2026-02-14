using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;

public class PossibleRewardsSystem : MonoBehaviour
{
    [SerializeField] private Image _panel;
    [SerializeField] private PossibleRewardSlot_UI _prefabSlot;
    [SerializeField] private Button _rewardsButton;
    [SerializeField] private Transform _rewardsTransform;

    public MainQuestKeeperDisplay MainQuestKeeperDisplay { get; private set; }

    public Image Panel => _panel;

    private void Awake()
    {
        MainQuestKeeperDisplay = GetComponent<MainQuestKeeperDisplay>();

        _rewardsButton?.onClick.AddListener(ShowWindow);
    }

    private void Start()
    {
        _panel.gameObject.SetActive(false);
    }

    public void ShowWindow()
    {
        _panel.gameObject.SetActive(true);

        ClearSlots();

        if (MainQuestKeeperDisplay.SelectedQuest != null)
        {
            List<RewardItemsRegion> rewards = MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.RegionData.RewardRegion;

            foreach (RegionLevelReward reward_1 in rewards[MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Level - 1].RewardItems)
            {
                foreach (RewardItems item in reward_1.RewardItems)
                {
                    CreateRewardSlot(item.RewardItem);
                }
            }
        }
    }

    private void CreateRewardSlot(SlotData slotData)
    {
        PossibleRewardSlot_UI item = Instantiate(_prefabSlot, _rewardsTransform.transform);
        item.Init(slotData);
    }

    private void ClearSlots()
    {
        foreach (Transform item in _rewardsTransform.transform.Cast<Transform>())
        {
            Destroy(item.gameObject);
        }
    }

    public void TabClicked(CheckTypeForTabs tab)
    {
        ClearSlots();

        List<RewardItemsRegion> rewards = MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.RegionData.RewardRegion;

        foreach (RegionLevelReward reward_1 in rewards[MainQuestKeeperDisplay.SelectedQuest.Quest.Quest.Level - 1].RewardItems)
        {
            foreach (RewardItems item in reward_1.RewardItems)
            {
                if (tab.ItemType == ItemType.All)
                    CreateRewardSlot(item.RewardItem);

                else
                {
                    if (item.RewardItem.ItemType == tab.ItemType)
                        CreateRewardSlot(item.RewardItem);

                    else
                        continue;
                }

            }
        }
    }
}
