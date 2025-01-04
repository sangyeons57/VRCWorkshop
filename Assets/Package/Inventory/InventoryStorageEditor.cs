using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(InventoryStorage))]
public class InventoryStorageEditor : Editor
{
    private InventoryStorage inventoryStorage;
    private int  inventorySize = 0 ;
    

    private bool showArrayFoldout = true;
    override public void OnInspectorGUI()
    {
        inventoryStorage = (InventoryStorage)target;

        /*
         * InventoryUI 클래스 받기
         */
        
        int newSize = EditorGUILayout.IntField("Array Size", inventoryStorage.storageItems != null ? inventoryStorage.storageItems.Length : 0);

        // 크기가 변경되었으면 배열 재생성
        if (inventoryStorage.storageItems == null || newSize != inventoryStorage.storageItems.Length)
        {
            InventoryItem[] newArray = new InventoryItem[newSize];
            if (inventoryStorage.storageItems != null)
            {
                for (int i = 0; i < Mathf.Min(newSize, inventoryStorage.storageItems.Length); i++)
                {
                    newArray[i] = inventoryStorage.storageItems[i];
                }
            }
            inventoryStorage.storageItems = newArray;
        }

        showArrayFoldout = EditorGUILayout.Foldout(showArrayFoldout, "Array Foldout", true);
        if (showArrayFoldout)
        {
            EditorGUI.indentLevel++;
            // 배열 내용 편집
            if (inventoryStorage.storageItems != null)
            {
                for (int i = 0; i < inventoryStorage.storageItems.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    InventoryItem inventoryItem = inventoryStorage.storageItems[i];
                    
                    inventoryItem = (InventoryItem)EditorGUILayout.ObjectField( 
                        "Inventory Item " + (i), 
                        inventoryItem, 
                        typeof(InventoryItem), 
                        true
                    );
                    
                    if (inventoryItem is StackableInventoryItem stackableInventoryItem)
                    {
                        int stack = EditorGUILayout.IntField(stackableInventoryItem.stack, GUILayout.Width(60));
                        if (stack > 0) 
                            stackableInventoryItem.stack = stack;
                    }

                    if (inventoryItem != null && InventoryItem.IsSyncItem(inventoryItem))
                    { 
                        Debug.LogError("Not Sync 인 것만 Storage에 먼저 넣을수 있습니다.");
                        EditorGUILayout.EndHorizontal();
                        continue; 
                    }

                        
                    checkOverlapItems(inventoryItem, i);
                    inventoryStorage.storageItems[i] = inventoryItem;
                    
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUI.indentLevel--;
        }

        // 변경사항 저장
        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryStorage);
        }
    }

    private void checkOverlapItems(InventoryItem inventoryItem, int index)
    {
        if(inventoryItem == null)
            return;
        
        for (int i = 0; i < inventoryStorage.storageItems.Length; i++)
        {
            if (i == index || inventoryStorage.storageItems[i] == null)
                continue;
            else if (inventoryStorage.storageItems[i].IsEqual(inventoryItem))
                inventoryStorage.storageItems[i] = null;
        }
    }
}
#endif