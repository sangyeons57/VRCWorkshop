
using System;
using UdonSharp;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDKBase;

public enum InventoryMode 
{
    Basic, // 기본 상호작용 모드
    PickUp, // 인벤토리 줍기 모드
}

// 인벤토리 메인 클래스
[RequireComponent(typeof(InventoryStorage))]
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class Inventory : UdonSharpBehaviour
{
    
    public InventoryUI inventoryUI;
    public InventoryStorage storage;

    public InventoryMode inventoryMode = InventoryMode.Basic;

    public Vector3 offsetPosition = new Vector3(0, 0, 2.5f);
    
    public KeyCode inventoryUIOnOffKey = KeyCode.I;
    
    private void OnValidate()
    {
        storage = GetComponent<InventoryStorage>();
        
        storage.inventory = this;
        
    }
    
    private void Start()
    {
        storage = GetComponent<InventoryStorage>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(inventoryUIOnOffKey))
        {
            if ( inventoryMode == InventoryMode.Basic )
            {
                inventoryMode = InventoryMode.PickUp;
            } 
            else
            {
                inventoryMode = InventoryMode.Basic;
            }
        }
    }

}
