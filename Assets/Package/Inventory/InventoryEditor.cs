using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(Inventory))]
public class InventoryEditor : Editor
{
    private Inventory inventory;
    
    private InventoryUI inventoryUI;
    private InventoryUIElement inventoryUIElement;
    private GridLayoutGroup gridLayoutGroup;
    private KeyCode inventoryUIOnOffKey;
    
    private InventoryStorage inventoryStorage;

    private bool showStackableList;

    override public void OnInspectorGUI()
    {
        inventory = (Inventory)target;

        EditorGUI.BeginDisabledGroup(true);
        inventoryStorage = (InventoryStorage)EditorGUILayout.ObjectField(
             "Inventory Storage",
             inventory.storage,
            typeof(InventoryStorage),
            false
        ); 
        
        EditorGUI.EndDisabledGroup();
        
        EditorGUILayout.Space();
        inventoryUI = (InventoryUI)EditorGUILayout.ObjectField(
             "인벤토리 UI",
             inventory.inventoryUI,
            typeof(InventoryUI),
            true
        ); 
        
        if (inventory.inventoryUI != inventoryUI)
        {
            if (inventoryUI == null)
                inventory.inventoryUI.inventory = null;
            else 
                inventoryUI.inventory = inventory;
            
            inventory.inventoryUI = inventoryUI;
        }
        
        gridLayoutGroup = (GridLayoutGroup)EditorGUILayout.ObjectField(
            "그리드 오브젝트",
            inventoryUI?.gridLayoutGroup,
            typeof(GridLayoutGroup),
            true
        );
        if(inventoryUI?.gridLayoutGroup != null) 
            inventoryUI.gridLayoutGroup = gridLayoutGroup;
        
        
        inventoryUIElement = (InventoryUIElement)EditorGUILayout.ObjectField(
            "인벤토리 UI 아이탬 리소스",
            inventoryUI?.uiElementResources,
            typeof(InventoryUIElement),
            true
        );
        if(inventoryUI?.uiElementResources != null)
            inventoryUI.uiElementResources = inventoryUIElement;
        
        inventoryUIOnOffKey = (KeyCode)EditorGUILayout.EnumPopup("인벤토리 On&Off 키", inventory.inventoryUIOnOffKey);
        inventory.inventoryUIOnOffKey = inventoryUIOnOffKey;
        
        EditorGUILayout.Space();
        
        inventory.offsetPosition = EditorGUILayout.Vector3Field("Drop Position", inventory.offsetPosition);
        
        EditorGUILayout.Space();
        
                /*
        EditorGUILayout.BeginHorizontal();
        showStackableList = EditorGUILayout.Foldout(showStackableList, "StackableItem 등록 배열");
        // 리스트의 크기 설정
        EditorGUILayout.LabelField("Size", GUILayout.Width(30) );
        int newSize = EditorGUILayout.IntField(inventory.stackableItemRegisterList.Length, GUILayout.Width(50) );

        EditorGUILayout.EndHorizontal();
        
        if (showStackableList)
        {
            
            if (newSize != (inventory.stackableItemRegisterList == null ? 0 : inventory.stackableItemRegisterList.Length))
            {
                Undo.RecordObject(inventory, "Resize List");
                inventory.stackableItemRegisterList = CreateNewRegisterArray(inventory.stackableItemRegisterList, newSize);
            }

            // 리스트 요소들 그리기
            for (int i = 0; i < inventory.stackableItemRegisterList.Length; i++)
            {
                StackableInventoryItem item = inventory.stackableItemRegisterList[i];
                
                EditorGUILayout.BeginHorizontal(); 
                
                EditorGUI.BeginDisabledGroup(true); 
                EditorGUILayout.TextField(item?.key, GUILayout.Width(100));
                EditorGUI.EndDisabledGroup();
                
                StackableInventoryItem newItem = (StackableInventoryItem)EditorGUILayout.ObjectField(item, typeof(StackableInventoryItem), true);
                if (newItem != null)
                {
                    if( newItem.gameObject.scene.rootCount == 0) 
                        inventory.stackableItemRegisterList[i] = newItem; 
                    else 
                        Debug.LogError("씬에 생성된 인스턴스가 아닌 프리팹 이여야합니다.");
                }
                
                EditorGUILayout.EndHorizontal();
            }
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            // 요소 추가 버튼
            if (GUILayout.Button("+", GUILayout.Width(25)))
            {
                newSize++;
                inventory.stackableItemRegisterList = CreateNewRegisterArray(inventory.stackableItemRegisterList, newSize);
            }
            // 요소 제거 버튼
            if (GUILayout.Button("–", GUILayout.Width(25)))
            {
                newSize = Mathf.Max(0, newSize - 1);
                inventory.stackableItemRegisterList = CreateNewRegisterArray(inventory.stackableItemRegisterList, newSize);
            }
            EditorGUILayout.EndHorizontal();
        }
                */
    }

    private StackableInventoryItem[] CreateNewRegisterArray(StackableInventoryItem[] beforeArray, int newSize)
    {
        StackableInventoryItem[] newKeyValuePairs = new StackableInventoryItem[newSize];
        if (beforeArray != null)
        {
            for (int i = 0; i < MathF.Min(newSize, beforeArray.Length); i++)
            {
                newKeyValuePairs[i] = beforeArray[i];
            }
        }
        return newKeyValuePairs;
    }
}
#endif
