using UnityEditor;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

#if UNITY_EDITOR
[CustomEditor(typeof(UnstackableInventoryItemSync))]
public class UnstackableInventoryItemSyncEditor : Editor
{
    private UnstackableInventoryItemSync inventoryItem;
    
    private Inventory inventory;
    private Transform capsule;
    private Sprite itemSprite;
    private InventoryItemSync sync;
    
    

    override public void OnInspectorGUI()
    {
        inventoryItem = (UnstackableInventoryItemSync)target;
            
        if (inventoryItem.inventory == null)
            inventoryItem.inventory = GameObject.FindFirstObjectByType<Inventory>();
        inventory = (Inventory)EditorGUILayout.ObjectField(
             "인벤토리",
             inventoryItem.inventory,
            typeof(Inventory),
            false
        );
        inventoryItem.inventory = inventory;
        
        if (inventoryItem.capsule == null && inventoryItem.transform.childCount == 1)
            inventoryItem.capsule = inventoryItem.transform.GetChild(0); 
        capsule = (Transform)EditorGUILayout.ObjectField(
             "인벤토리 아이탬 캡슐",
             inventoryItem.capsule,
            typeof(Transform),
            true
        );
        inventoryItem.capsule = capsule;

        itemSprite = (Sprite)EditorGUILayout.ObjectField(
            "아이탬 이미지",
            inventoryItem.itemSprite,
            typeof(Sprite),
            true
        );
        inventoryItem.itemSprite = itemSprite;
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryItem);
        }
    }
}
#endif
