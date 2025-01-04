using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(InventoryUI))]
public class InventoryUIEditor : Editor
{
    private InventoryUI inventoryUI;
    
    private Inventory inventory;
    private InventoryUIElement inventoryUIElement;
    private GridLayoutGroup gridLayoutGroup;
    

    override public void OnInspectorGUI()
    {
        inventoryUI = (InventoryUI)target;

        //DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(true);
        inventory = (Inventory)EditorGUILayout.ObjectField(
             "인벤토리",
             inventoryUI.inventory,
             typeof(Inventory),
            false
        );
        EditorGUI.EndDisabledGroup();
        
        inventoryUIElement = (InventoryUIElement)EditorGUILayout.ObjectField(
            "인벤토리 UI 아이탬 리소스",
             inventoryUI.uiElementResources,
            typeof(InventoryUIElement),
            true
        );
        inventoryUI.uiElementResources = inventoryUIElement;
        
        gridLayoutGroup = (GridLayoutGroup)EditorGUILayout.ObjectField(
            "그리드 오브젝트",
            inventoryUI.gridLayoutGroup,
            typeof(GridLayoutGroup),
            true
        );
        inventoryUI.gridLayoutGroup = gridLayoutGroup;

        

        
        inventoryUI.offsetPosition = EditorGUILayout.Vector3Field("이동 위치 Position", inventoryUI.offsetPosition);
        /*
        // 기본 라인과 공백
        EditorGUILayout.Space(); // UI 상단 공백
        EditorGUILayout.LabelField("UI 이동 정보", EditorStyles.boldLabel); // Header

        EditorGUI.indentLevel++; // 들여쓰기 시작


        EditorGUI.indentLevel--; // 들여쓰기 종료
        EditorGUILayout.Space(); // 한 계단 내리기
        */

        
        
        if (inventory != null && GUILayout.Button("Init UI"))
        { 
            InitUI();
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryUI);
        }
    }
    
    public void InitUI()
    {
        if (CheckNull())
            return;

        for (int i = gridLayoutGroup.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gridLayoutGroup.transform.GetChild(i).gameObject);
        }

        inventoryUI.elementsArray = new InventoryUIElement[inventory.storage.storageItems.Length];

        for (int i = 0; i < inventory.storage.storageItems.Length; i++)
        {
            InventoryUIElement instance = Instantiate(inventoryUIElement.gameObject, gridLayoutGroup.transform).GetComponent<InventoryUIElement>();
            instance.transform.name = "InventoryUIElement " + i;
            instance.storeIndex = i;
            instance.inventoryUI = inventoryUI;
            instance.SetItem(inventory.storage.storageItems[i]);
            inventoryUI.elementsArray[i] = instance;
        }
    }

    public bool CheckNull()
    {
        if (inventory == null)
        {
            Debug.LogError("Inventory is null, assign this \"InventoryUI\" script to Inventory");
            return true;
        }

        if (inventoryUIElement == null)
        {
            Debug.LogError("InventoryUIElement is null, assign \"Inventory UI Element\" to InventoryUI or Inventory ");
            return true;
        }

        if (gridLayoutGroup == null)
        {
            Debug.LogError("GridLayoutGroup is null, assign this \"Grid Layout Group\" script to GridLayoutGroup");
        } 
        return false;
    }
}
#endif
