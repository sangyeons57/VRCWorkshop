
using System;
using UdonSharp;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Serialization;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

public class InventoryUI : UdonSharpBehaviour
{
    public Inventory inventory;
    
    public GridLayoutGroup gridLayoutGroup;
    public InventoryUIElement uiElementResources;
    
    public InventoryUIElement[] elementsArray;

    public Vector3 offsetPosition = new Vector3(0, 0, 1.25f);

    
    private Canvas canvas;

    private void OnValidate()
    {
    }

    void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(inventory.inventoryUIOnOffKey))
        {
            SwitchInventoryState();
        }
    }

    public void SwitchInventoryState()
    {
         canvas.enabled = !canvas.enabled;
         if (canvas.enabled)
         { 
             Vector3 headPosition = Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
             Quaternion headRotation =  Networking.LocalPlayer.GetTrackingData(VRCPlayerApi.TrackingDataType.Head).rotation;

             transform.position = headPosition + headRotation * offsetPosition;
             transform.rotation = Quaternion.LookRotation(transform.position - headPosition);
         }       
    }

    /*
     * 각 element(spriet 를 표시하는 인스턴스)에 item설정하기
     */
    public void SetItem(InventoryItem item, int index)
    {
        Debug.Log("SetItem: "  + item);
        elementsArray[index].SetItem(item);
    }

    /*
     * 모든 element(spriet 를 표시하는 인스턴스)에 item들 제거하기
     */
    public void ClearItem(int index)
    {
        elementsArray[index].ClearItem();
    }
}
