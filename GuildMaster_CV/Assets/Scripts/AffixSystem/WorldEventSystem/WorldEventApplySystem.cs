using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventApplySystem : MonoBehaviour
{
    [Header("Shops")]
    [SerializeField] private ShopKeeper _alchemistShop;
    [SerializeField] private ShopKeeper _cookShop;
    [SerializeField] private ShopKeeper _jewelerShop;
    [SerializeField] private ShopKeeper _workShop;
    [SerializeField] private ShopKeeper _blacksmithShop;
    [SerializeField] private ShopKeeper _tannerShop;
    [SerializeField] private ShopKeeper _tailorShop;
    [SerializeField] private ShopKeeper _magicShop;
    [Header("Recruitment")]
    [SerializeField] private RecruitKeeper _recruitmentObject;

    public ShopKeeper AlchemistShop => _alchemistShop;
    public ShopKeeper CookShop => _cookShop;
    public ShopKeeper JewelerShop => _jewelerShop;
    public ShopKeeper WorkShop => _workShop;
    public ShopKeeper BlacksmithShop => _blacksmithShop;
    public ShopKeeper TannerShop => _tannerShop;
    public ShopKeeper TailorShop => _tailorShop;
    public ShopKeeper MagicShop => _magicShop;
    public RecruitKeeper RecruitmentObject => _recruitmentObject;

    public WorldEventSystem WorldEventSystem { get; private set; }

    private void Awake()
    {
        WorldEventSystem = GetComponent<WorldEventSystem>();
    }

    public void ApplyEvent()
    {

    }
}
