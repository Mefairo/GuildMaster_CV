using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

//[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField] private PlayerInventoryHolder _player;
    public SlotData SlotData;

    //private ItemPrefabData _itemPrefab;
    private bool canPickUp = true;

    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;

    private void Awake()
    {
        //SaveLoad.OnLoadGame += LoadGame;
        //itemSaveData = new ItemPickUpSaveData(SlotData, transform.position, transform.rotation);

        //_itemPrefab = GetComponentInParent<ItemPrefabData>();
    }

    private void Start()
    {
        //id = GetComponent<UniqueID>().ID;
        //SaveGameManager.data.activeItems.Add(id, itemSaveData);
    }

    //private void LoadGame(SaveData data)
    //{
    //    if (data.collectedItems.Contains(id))
    //    {
    //        Destroy(this.gameObject);
    //    }
    //}

    //private void OnDestroy()
    //{
    //    if (SaveGameManager.data.activeItems.ContainsKey(id))
    //    {
    //        SaveGameManager.data.activeItems.Remove(id);
    //    }

    //    SaveLoad.OnLoadGame -= LoadGame;
    //}

    private void Update()
    {
        canPickUp = true;
    }

    private void OnMouseDown()
    {
        if (canPickUp)
        {
            var inventory = _player.transform.GetComponent<PlayerInventoryHolder>();

            if (!inventory)
            {
                return;
            }

            if (inventory.AddToInventory(SlotData, 1))
            {
                //SaveGameManager.data.collectedItems.Add(id);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (canPickUp && other.CompareTag("Player"))
        {
            //FixCollider(gameObject);
            var inventory = other.transform.GetComponent<PlayerInventoryHolder>();

            if (!inventory) return;

            if (inventory.AddToInventory(SlotData, 1))
            {
                //SaveGameManager.data.collectedItems.Add(id);

            }

            //else
            //{
            //    if (_itemPrefab.EquipSlot.ItemData.ItemType == ItemType.Equipment)
            //    {
            //        if (inventory.AddToInventory(_itemPrefab, 1))
            //        {
            //            SaveGameManager.data.collectedItems.Add(id);

            //            Destroy(this.gameObject);
            //            Destroy(this._itemPrefab.gameObject);
            //        }
            //    }
            //}
        }
    }

    private void FixCollider(GameObject item)
    // Исправление бага с подбором предметов с двойным тригером коллайдера
    {
        CapsuleCollider2D itemCollider = item.GetComponent<CapsuleCollider2D>();
        //itemCollider.isTrigger = false;
        //Destroy(item);
        canPickUp = false;
        //itemCollider.isTrigger = true;
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public SlotData ItemData;
    public Vector3 Position;
    public Quaternion Rotation;

    public ItemPickUpSaveData(SlotData _data, Vector3 _position, Quaternion _rotation)
    {
        ItemData = _data;
        Position = _position;
        Rotation = _rotation;
    }
}
