using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckTypeForTabs : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay _invDisplay;
    [Header("Type Tabs")]
    [SerializeField] private ItemType _itemType;
    [SerializeField] private ItemPrimaryType _itemPrimaryType;
    [SerializeField] private HeroTabs _heroTabs;
    [SerializeField] private EquipType _equipType;

    private Button _button;

    public CraftKeeperDisplay CraftKeeper { get; private set; }
    public ShopKeeperDisplay ShopKeeper { get; private set; }
    public KeeperDisplay KeeperDisplay { get; private set; }
    public PossibleRewardsSystem PossibleRewardsSystem { get; private set; }
    //public InventoryUIController InventoryUIController { get; private set; }

    public ItemType ItemType => _itemType;
    public ItemPrimaryType ItemPrimaryType => _itemPrimaryType;
    public HeroTabs HeroTabs => _heroTabs;
    public EquipType EquipType => _equipType;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button?.onClick.AddListener(OnTabClick);

        ShopKeeper = GetComponentInParent<ShopKeeperDisplay>();
        CraftKeeper = GetComponentInParent<CraftKeeperDisplay>();
        KeeperDisplay = GetComponentInParent<KeeperDisplay>();
        PossibleRewardsSystem = GetComponentInParent<PossibleRewardsSystem>();
        //InventoryUIController = GetComponentInParent<InventoryUIController>();
    }


    public void OnTabClick()
    {
        if (ShopKeeper != null)
            ShopKeeper?.TabClicked(this);

        if (CraftKeeper != null)
            CraftKeeper.TabClicked(this);

        if (KeeperDisplay != null)
            KeeperDisplay.TabClicked(this);

        if (PossibleRewardsSystem != null)
            PossibleRewardsSystem.TabClicked(this);

        if (_invDisplay != null)
            _invDisplay.TabClicked(this);
    }

    public void ShowTabItems()
    {

    }
}
