
using System;
using Cysharp.Threading.Tasks;
using UdonSharp;
using UnityEngine;
using UnityEngine.UI;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.None)]
public class InventoryUIElement : UdonSharpBehaviour
{
    public InventoryUI inventoryUI;
    public bool autoFinding = true;
    public Image itemSpriteImage;
    public Button itemButton;

    public InventoryItem item { get; private set; }
    public int storeIndex;

    public void OnClick()
    {
        Debug.Log("OnClick");
        if (item != null)
        { 
            item.Drop();
        }
    }


    public InventoryUIElement Init(InventoryUI inventoryUI, int storeIndex)
    {
        this.inventoryUI = inventoryUI;
        this.storeIndex = storeIndex;
        
        return this;
    }

    public void SetItem(InventoryItem inventoryItem)
    {
        if (inventoryItem == null)
            return;
        
        Debug.Log("SetItem :: " + inventoryItem.name);
        
        this.item = inventoryItem;
        inventoryItem.SetInventoryUIElement(this);
        
        if (itemSpriteImage == null)
        {
            Debug.LogError("InventoryUIElement::Init: itemSpriteImage is null 제공한 아이탬 리소스가 이미지 설정이 안되어 있음");
            return;
        }
        itemSpriteImage.sprite = item.itemSprite;
    }

    public void ClearItem()
    {
        item.SetInventoryUIElement(null);
        itemSpriteImage.sprite = null;
        this.item = null;
    }
}
