
using System;
using System.Collections;
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;
using VRC.Udon.Serialization.OdinSerializer.Utilities;

public enum InventoryItemType
{
    Stackable,
    StackableSync,
    Unstackable,
    UnstackableSync,
}
public enum InventoryItemState
{
    Error,
    Storage,
    NotStorage,
    OtherPersonStorage,
}
public enum InventoryItemSync
{
    Sync,
    NotSync,
}
public abstract class InventoryItem : UdonSharpBehaviour
{
    
    [HideInInspector] public Inventory inventory;
    [HideInInspector] public Transform capsule;
    [HideInInspector] public Sprite itemSprite;
    
    [HideInInspector] public InventoryItemState inventoryItemState  = InventoryItemState.NotStorage;
    
    public int inventoryStoreIndex;
    
    protected Collider collider;
    protected Rigidbody rigidbody;
    
    protected InventoryUIElement inventoryUIElement;

    public virtual InventoryItemType itemType { get; }

    void Start()
    {
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public abstract void PickUp();
    public abstract void Drop();
    

    /*
     * 문제 확인 로그 찍기 용
     */
    public bool IsProblemExist()
    {
        if (collider == null)
        {
            Debug.LogError("GameObject of \"InventoryItem\" must have \"Collider\"");
            return true;
        }

        if (transform.childCount != 1)
        {
            Debug.LogError("GameObject of \"InventoryItem\" must have only one child");
            return true;
        }
        
        return false;
    }
    
    public void SetInventoryUIElement(InventoryUIElement inventoryUIElement)
    {
        this.inventoryUIElement = inventoryUIElement;
    }

    public bool IsEqual(InventoryItem item)
    {
         if (item == this)
             return true;

         if (item == null || item.itemType != this.itemType)
             return false;
         
         bool result = false; 
         if (itemType == InventoryItemType.Unstackable)
             result = (item == this);
         else if (itemType == InventoryItemType.Stackable)
         {
             Debug.Log("1: " + item.name);
             Debug.Log("2: " + this.name);
             Debug.Log("3: " + ((StackableInventoryItem)item).key);
             Debug.Log("3.1: " + (((StackableInventoryItem)item).key == null));
             Debug.Log("3.2: " + (((StackableInventoryItem)item).key == ""));
             Debug.Log("4: " + ((StackableInventoryItem)this).key);
             
             result = ((StackableInventoryItem)item).key.Equals(((StackableInventoryItem)this).key);
         }
                 
         return result;       
    }
    
    public void DestroyObject()
    {
        Debug.Log("DestroyObject");
        Destroy(gameObject);
    }
    
    public void DestroyAllSyncObject()
    {
        Debug.Log("DestroyAllObject");
        SendCustomNetworkEvent(NetworkEventTarget.All, "DestroyObject");
    }
    
    protected void SetOwner()
    {
        if (!Networking.IsOwner(gameObject))
        {
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        }
    }

    public static bool IsSyncItem(InventoryItem item)
    {
        return item.itemType == InventoryItemType.UnstackableSync || item.itemType == InventoryItemType.StackableSync; 
    }

}
