using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
[CustomEditor(typeof(InventoryUIElement))]
public class InventoryUIElementEditor : Editor
{
    private InventoryUIElement inventoryUIElement;
    
    private InventoryUI inventoryUI;
    private InventoryItem inventoryItem;
    
    private Image itemSpriteImage;
    private Button itemButton;


    override public void OnInspectorGUI()
    {
        inventoryUIElement = (InventoryUIElement)target;

        //DrawDefaultInspector();

        EditorGUI.BeginDisabledGroup(true);
        
        EditorGUILayout.FloatField("StoreIndex", inventoryUIElement.storeIndex);
        
        inventoryUI = (InventoryUI)EditorGUILayout.ObjectField(
            "Inventory UI",
            inventoryUIElement.inventoryUI,
            typeof(InventoryUI),
            false
        );

        if (inventoryUIElement.item == null)
        {
            inventoryUIElement.SetItem(inventoryUIElement.inventoryUI.inventory.storage.storageItems[inventoryUIElement.storeIndex]);
        }
        inventoryItem = (InventoryItem)EditorGUILayout.ObjectField(
            "Inventory Item",
            inventoryUIElement.item,
            typeof(InventoryItem),
            false
        );
        EditorGUI.EndDisabledGroup();

        inventoryUIElement.autoFinding = EditorGUILayout.Toggle("Auto Finding", inventoryUIElement.autoFinding);
        
        if (inventoryUIElement.autoFinding && itemSpriteImage == null)
            inventoryUIElement.itemSpriteImage = findImageComponent(inventoryUIElement.transform);
        itemSpriteImage = (Image)EditorGUILayout.ObjectField(
            "Item Sprite Image",
            inventoryUIElement.itemSpriteImage,
            typeof (Image),
            true
        ); 
        inventoryUIElement.itemSpriteImage = itemSpriteImage;
        
        if (inventoryUIElement.autoFinding && itemButton == null)
            inventoryUIElement.itemButton = findButtonComponent(inventoryUIElement.transform);
        itemButton = (Button)EditorGUILayout.ObjectField(
            "Item Sprite Image",
            inventoryUIElement.itemButton,
            typeof (Button),
            true
        ); 
        inventoryUIElement.itemButton = itemButton;
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryUI);
        }
    }
    
    private Image findImageComponent(Transform checkingTransform)
    {
        if (checkingTransform.GetComponent<Image>() != null)
            return checkingTransform.GetComponent<Image>();

        if (checkingTransform.childCount > 0)
        {
            foreach (Transform child in checkingTransform)
            {
                Image image = findImageComponent(child);

                if (image != null)
                    return image;
            }
        } 
        
        return null;
    }

    private Button findButtonComponent(Transform checkingTransform)
    {
        if (checkingTransform.GetComponent<Button>() != null)
            return checkingTransform.GetComponent<Button>();

        if (checkingTransform.childCount > 0)
        {
            foreach (Transform child in checkingTransform)
            {
                Button button = findButtonComponent(child);

                if (button != null)
                    return button;
            }
        } 
        
        return null;
    }
}
#endif