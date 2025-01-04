using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class StackableInventoryItemSync : StackableInventoryItemAbstact
{
    public override InventoryItemType itemType => InventoryItemType.StackableSync;
    /*
     * 인벤토리 아이탬에서
     * 
     */
    public string key = "";
    
    [HideInInspector]
    public int stack = 1;

    private void Awake()
    {
        this.stack = 1;
    }
    
    public override void Interact()
    {
        Debug.Log("Interact");
        // 인벤토리에 저장할수있는 상태가 아닌경우 Pickup불가
        if (inventory.inventoryMode == InventoryMode.PickUp) 
            PickUp();
        
        foreach (UdonSharpBehaviour u in capsule.GetComponents<UdonSharpBehaviour>())
        {
            // Interact를 먹어버리는 문제 때문에 다른 같은 스크립에 있는 오브젝트한테 ObjectIteracgt이벤트 호출
            u.SendCustomEvent("ObjectInteract");
        }
    }

    public override void PickUp()
    {
        SetOwner();
        if (inventory.storage.AddItem(this)) 
            DestroyAllSyncObject();
        else 
            SendCustomNetworkEvent(NetworkEventTarget.All, "SetThisToStackObject");
        
        Debug.Log("item stack changed" + stack);
    }

    public override void Drop()
    {
        if (stack > 0 && inventory.storage.DropItem(this))
        {
            Debug.Log("Destory: " + stack);
            SetOwner();
            SendCustomNetworkEvent(NetworkEventTarget.All, "InstantiateStackableInventoryItem");
        }
    }

    public override void InstantiateStackableInventoryItem()
    {
        VRCPlayerApi player = Networking.GetOwner(gameObject);
        Vector3 headPosition = player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
        Quaternion headRotation =  player.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

        Quaternion yaw = Quaternion.Euler(0, headRotation.eulerAngles.y, 0);
        Vector3 pos = headPosition + yaw * inventory.offsetPosition;
        
        Instantiate(gameObject, pos, yaw);
    }
    
    public override void SetThisToStackObject()
    {
        Debug.Log("SetThisToStackObject");
        if (collider != null) 
            collider.enabled = false;
        if (rigidbody != null)
            rigidbody.isKinematic = true;
        
        capsule.gameObject.SetActive(false);
    }

    /*
    public void UnlockStackMod()
    {
        if ((collider ??( collider = GetComponent<Collider>())) != null)
        {
            Debug.Log(GetComponent<Collider>());
            collider.enabled = true;
        }

        if ((rigidbody ?? (rigidbody = GetComponent<Rigidbody>())) != null)
        {
            Debug.Log(GetComponent<Rigidbody>());
            rigidbody.isKinematic = false;
        }
        
        capsule.gameObject.SetActive(true);
    }
    */
}
