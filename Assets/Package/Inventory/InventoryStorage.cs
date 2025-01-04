
using System.Collections.Generic;
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Data;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class InventoryStorage : UdonSharpBehaviour
{
    public readonly int NOT_STORED_INDEX = -1; 
    
    public Inventory inventory;
    public InventoryItem[] storageItems;
    public DataDictionary storedDict;

    private void Start() 
    { 
        ItemsOverlappingCheck();
        for (int i = 0; i < storageItems.Length; i++)
        {
            InventoryItem item = storageItems[i];
            
            if (item == null)
                continue;

            switch (item.itemType)
            {
                case InventoryItemType.Unstackable:
                    initUnstackable((UnstackableInventoryItem)item, i);
                    break;
                case InventoryItemType.Stackable:
                    initStackable((StackableInventoryItem)item, i);
                    break;
            }
        }       
    }

    private void initUnstackable(UnstackableInventoryItem unstackable, int index)
    {
        if (InventoryItem.IsSyncItem(unstackable))
        {
            storageItems[index] = null;
            unstackable.SetItemState(InventoryItemState.NotStorage);
            return;
        }
        
        unstackable.SetItemState(InventoryItemState.Storage);
        setItem(unstackable, index);
    }


    private void initStackable(StackableInventoryItem stackable, int index)
    {
        stackable.SetThisToStackObject();
        setItem(stackable, index);
    }

    public bool AddItem(InventoryItem item)
    {
        switch (item.itemType)
        {
            case InventoryItemType.Stackable: 
                return addStackableItem((StackableInventoryItem)item);
            case InventoryItemType.StackableSync: 
                return addStackableItem((StackableInventoryItem)item);
            case InventoryItemType.Unstackable: 
                return addUnstackableItem((UnstackableInventoryItem) item);
            case InventoryItemType.UnstackableSync: 
                return addUnstackableItem((UnstackableInventoryItem) item);
        }
        
        return false;
    }

    private bool addStackableItem(StackableInventoryItem item)
    {
         int firstNullElementIndex = NOT_STORED_INDEX;
         // 이미 저장된 아이탬을 탐색하고 있는경우 스택 추가
         for (int i = 0; i < storageItems.Length; i++)
         {
             if (storageItems[i] != null &&
                 storageItems[i].itemType == InventoryItemType.Stackable && 
                 storageItems[i].IsEqual(item))
             {
                 ((StackableInventoryItem)storageItems[i]).stack++;
                 return true;
             }
             else if (firstNullElementIndex == NOT_STORED_INDEX && storageItems[i] == null)
                 firstNullElementIndex = i;
         }

         // 이미 저장된 아이탬이 없는경우 스택 1개로 설정
         if (firstNullElementIndex != NOT_STORED_INDEX)
         {
            setItem(item, firstNullElementIndex);
            item.stack = 1;
         }
     
         return false;
    }

    private List<StackableInventoryItemAbstact> stackableItemList = new List<StackableInventoryItemAbstact>();
    private bool addStackablItemSync(StackableInventoryItemAbstact item)
    {
        foreach (StackableInventoryItemAbstact inventoryItem in stackableItemList)
        {
            if (stackableItemList.Contains(inventoryItem))
                inventoryItem.stack++;
            else
            { 
                stackableItemList.Add(item);
                item.inventoryStoreIndex
                setItem(item, );
            }
        }
        
    }

    private bool addUnstackableItem(UnstackableInventoryItem item)
    {
        for (int i = 0; i < storageItems.Length; i++)
        {
            if (storageItems[i] == null)
            {
                setItem(item, i);
                return true;
            }
        }

        return false;
    }

    private void setItem(InventoryItem item, int index)
    {
        storageItems[index] = item;
        storedDict[item] = index;
        
        //ui에 아이탬 이미지 설정하기
        inventory.inventoryUI.SetItem(item, index);
    }
    public bool DropItem(InventoryItem item)
    {
        switch (item.itemType)
        {
            case InventoryItemType.Stackable: 
                return dropStackableItem((StackableInventoryItem)item);
            case InventoryItemType.Unstackable: 
                return dropUnstackableItem((UnstackableInventoryItem) item);
        }
        
        return false;
    }

    private bool dropStackableItem(StackableInventoryItem item)
    {
        if (storedDict.TryGetValue(item, out DataToken index)  && storageItems[index.Int] == item)
        {
            if (--item.stack < 1)
            {
                clearItem(index.Int); 
            } 
            
            return true;
        } 
        return false;   
    }

    private bool dropUnstackableItem(UnstackableInventoryItem item)
    {
        if (storedDict.TryGetValue(item, out DataToken index) && storageItems[index.Int] == item)
        {
            clearItem(index.Int);
            return true;
        }

        return false;
    }

    private void clearItem(int index)
    {
        storageItems[index] = null;
        inventory.inventoryUI.ClearItem(index);
    }


    public void ItemsOverlappingCheck()
    {
        for (int flag = 0; flag < storageItems.Length; flag++)
        {
            if (storageItems[flag] == null)
                continue;
            
            for (int i = flag + 1; i < storageItems.Length; i++)
            {
                if (storageItems[flag].IsEqual(storageItems[i]))
                { 
                    storageItems[i] = null;
                }
            }
        }
    }
    

}
