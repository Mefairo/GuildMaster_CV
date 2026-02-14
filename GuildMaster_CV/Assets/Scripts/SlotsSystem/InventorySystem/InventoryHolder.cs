using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int _inventorySize;
    [SerializeField] protected InventorySystem _primaryInventorySystem;
    [SerializeField] protected int _offset = 10;
    [SerializeField] protected int _gold;

    public int Offset => _offset;

    public InventorySystem PrimaryInventorySystem => _primaryInventorySystem;

    public static UnityAction<InventorySystem, int> OnDinamicInventoryDisplayRequested;

    protected virtual void Awake()
    {
        //SaveLoad.OnLoadGame += LoadInventory;

        _primaryInventorySystem = new InventorySystem(_inventorySize, _gold);
    }

    //protected abstract void LoadInventory(SaveData saveData);
}



[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem InvSystem;
    public Vector3 Position;
    public Quaternion Rotation;

    public InventorySaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        InvSystem = _invSystem;
        Position = _position;
        Rotation = _rotation;
    }

    public InventorySaveData(InventorySystem _invSystem)
    {
        InvSystem = _invSystem;
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
    }
}