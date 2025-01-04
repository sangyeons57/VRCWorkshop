using UnityEditor;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

#if UNITY_EDITOR
[CustomEditor(typeof(UnstackableInventoryItem))]
public class UnstackableInventoryItemEditor : Editor
{
    private UnstackableInventoryItem inventoryItem;
    
    private Inventory inventory;
    private Transform capsule;
    private Sprite itemSprite;
    private InventoryItemSync sync;
    
    

    override public void OnInspectorGUI()
    {
        inventoryItem = (UnstackableInventoryItem)target;

        //DrawDefaultInspector();

        /*
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("식별코드", inventoryItem.identificationCode);
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("InventoryItem 식별코드 재발급"))
                inventoryItem.CreateIdentificationCode();
        }
        EditorGUILayout.EndHorizontal();
        */
            
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

        /*
        sync = (InventoryItemSync)EditorGUILayout.EnumPopup("싱크 설정", inventoryItem.inventoryItemSync);
        if (sync != inventoryItem.inventoryItemSync)
        {
            inventoryItem.inventoryItemSync = sync;
            
            UdonBehaviour udonBehaviour = inventoryItem.gameObject.GetComponent<UdonBehaviour>();
            if (inventoryItem.inventoryItemSync == InventoryItemSync.Sync)
            {
                udonBehaviour.SyncMethod = Networking.SyncType.Continuous;
                if (!inventoryItem.gameObject.TryGetComponent<VRCObjectSync>(out VRCObjectSync sync))
                { 
                    inventoryItem.gameObject.AddComponent<VRCObjectSync>();
                }
            }
            else
            {
                udonBehaviour.SyncMethod = Networking.SyncType.None;
                if (inventoryItem.gameObject.TryGetComponent<VRCObjectSync>(out VRCObjectSync sync))
                {
                    DestroyImmediate(sync);
                }
            }
        }
        */

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
