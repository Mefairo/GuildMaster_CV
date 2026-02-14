using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Craft System/Craft Item List")]
public class CraftItemList : ScriptableObject
{
    [SerializeField] private List<CraftItemData> _items;

    public List<CraftItemData> Items => _items;
}

//[System.Serializable]
//public struct CraftInventoryItem
//{
//    public InventoryItemData ItemDatas;
//    public CraftItemData ItemData;
//}
