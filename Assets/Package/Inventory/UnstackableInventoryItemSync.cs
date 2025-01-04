using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class UnstackableInventoryItemSync : InventoryItem
{

    public override InventoryItemType itemType => InventoryItemType.UnstackableSync;
    /*
     * 현제 인벤토리 아이탬 기능을 그대로 가지고 오면됨
     * 
     */
    
    public override void Interact()
    {
        Debug.Log("Interact");
        // 인벤토리에 저장할수있는 상태가 아닌경우 Pickup불가
        if (inventoryItemState == InventoryItemState.NotStorage &&
            inventory.inventoryMode == InventoryMode.PickUp) 
            PickUp();
        
        foreach (UdonSharpBehaviour u in capsule.GetComponents<UdonSharpBehaviour>())
        {
            // Interact를 먹어버리는 문제 때문에 다른 같은 스크립에 있는 오브젝트한테 ObjectIteracgt이벤트 호출
            u.SendCustomEvent("ObjectInteract");
        }
    }
    
    public void SetItemState(InventoryItemState state)
    { 
        switch (state) 
        { 
            case InventoryItemState.Storage: 
                SendCustomNetworkEvent(NetworkEventTarget.All, "SetStateToStorage");
                break;
            case InventoryItemState.NotStorage: 
                SendCustomNetworkEvent(NetworkEventTarget.All, "SetStateToNotStorage");
                break;
            case InventoryItemState.OtherPersonStorage:
            
            default:
                inventoryItemState = InventoryItemState.Error;
                break;
        }   
    }

    public void SetStateToStorage()
    {
        
        if (collider != null)
            collider.enabled = false;
        if (rigidbody != null)
            rigidbody.isKinematic = true;
        
        capsule.gameObject.SetActive(false);
        
        if (Networking.IsOwner(Networking.LocalPlayer, gameObject))
        { 
            inventoryItemState = InventoryItemState.Storage;
        }
        else
        {
            inventoryItemState = InventoryItemState.OtherPersonStorage;
        }
    }
    
    public void SetStateToNotStorage()
    {
        inventoryStoreIndex = inventory.storage.NOT_STORED_INDEX;

        if (collider != null) 
            collider.enabled = true;
        if (rigidbody != null)
            rigidbody.isKinematic = false;
        
        capsule.gameObject.SetActive(true);
        inventoryItemState = InventoryItemState.NotStorage;
        
    }

    public override void PickUp()
    {
        Debug.Log("PickUp");
        if (inventoryItemState == InventoryItemState.NotStorage)
        { 
            if (inventory.storage.AddItem(this))
            {
                SetItemState(InventoryItemState.Storage); 
            }
        }
    }

    public override void Drop()
    {
        if (inventoryItemState == InventoryItemState.Storage)
        {
            if (inventory.storage.DropItem(this))
            {
                SetItemState(InventoryItemState.NotStorage);
                
                Vector3 headPosition = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
                Quaternion headRotation =  Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

                Quaternion yaw = Quaternion.Euler(0, headRotation.eulerAngles.y, 0); 
                transform.position = headPosition + yaw * inventory.offsetPosition;
            }
        }
    }
}