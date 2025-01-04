using System.Diagnostics.Contracts;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon.Common.Interfaces;

public abstract class StackableInventoryItemAbstact : InventoryItem
{
    public string key;
    
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

    public abstract void InstantiateStackableInventoryItem();
    public abstract void SetThisToStackObject();

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
