
#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using VRC.Udon.Serialization.OdinSerializer.Utilities;


[CustomEditor(typeof(PlayerBoundary))]
public class PlayerBoundaryGUI : Editor
{
    private PlayerBoundaryScriptableObject scriptableObject;
    private Material OriginalMaterial;
    private Material coppiedMaterial;

    private const string materialName = "Unlit_CloseUpEffect";

    private void OnEnable()
    { 
        PlayerBoundary playerBoundary = (PlayerBoundary)target;
        if (!(EditorApplication.isPlaying || playerBoundary.id.IsNullOrWhitespace()))
        {
            string directoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(playerBoundary))); 
            directoryPath = Path.Combine(directoryPath, $"ScriptableObjects/{playerBoundary.id}.asset"); 
            scriptableObject = AssetDatabase.LoadAssetAtPath<PlayerBoundaryScriptableObject>(directoryPath);
        }
    }

    override public void OnInspectorGUI()
    { 
        PlayerBoundary playerBoundary = (PlayerBoundary)target; 
        
        EditorGUI.BeginDisabledGroup(true);
        EditorGUILayout.TextField("ID", playerBoundary.id); 
        EditorGUI.EndDisabledGroup(); 
        
        DrawDefaultInspector();

        if (EditorApplication.isPlaying)
        {
            return;
        }
        
        
        if (scriptableObject != null)
        {
            EditorGUI.BeginDisabledGroup(true);
            scriptableObject.material = (Material)EditorGUILayout.ObjectField( 
                "Material", 
                scriptableObject.material,
                typeof(Material),
                false
            );
            EditorGUI.EndDisabledGroup(); 
            
            scriptableObject.autoBoundaryMode = (AutoBoundaryMode) EditorGUILayout.EnumPopup("경계 자동설정 모드", scriptableObject.autoBoundaryMode);
         
            EditorGUI.BeginChangeCheck();
            scriptableObject.threshold = EditorGUILayout.FloatField("쉐이더 임계값", scriptableObject.threshold);
            scriptableObject.fadeRange = EditorGUILayout.FloatField("쉐이더 퍼짐", scriptableObject.fadeRange);
            bool isChanged = EditorGUI.EndChangeCheck();
         
            scriptableObject.boundaryType = (BoundaryType) EditorGUILayout.EnumPopup("경계 수동설정 enum", scriptableObject.boundaryType);
        }
     
        GUILayout.Space(20);
        EditorGUILayout.LabelField("설정", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        {
            scriptableObject = (PlayerBoundaryScriptableObject)EditorGUILayout.ObjectField(
                 "ScriptableObject",
                 scriptableObject,
                typeof(PlayerBoundaryScriptableObject),
                false
            );
         
            if (GUILayout.Button("새 설정 생성"))
            {
                createScriptableObject();
            }
        } 
        EditorGUILayout.EndHorizontal();

        if (scriptableObject != null && GUILayout.Button("세팅 적용"))
        { 
            applySetting();
        } 
    }

    public void createScriptableObject()
    { 
        PlayerBoundary playerBoundary = (PlayerBoundary)target; 
        string directoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(playerBoundary))); 
        directoryPath = Path.Combine(directoryPath, "ScriptableObjects"); 
         
        if (!Directory.Exists(directoryPath)) 
            Directory.CreateDirectory(directoryPath);

        PlayerBoundaryScriptableObject scriptableObject = ScriptableObject.CreateInstance<PlayerBoundaryScriptableObject>(); 
        scriptableObject.id = System.Guid.NewGuid().ToString();
        scriptableObject.material = createMaterial(scriptableObject.id);
        UnityEditor.AssetDatabase.CreateAsset(scriptableObject, Path.Combine(directoryPath, $"{scriptableObject.id}.asset"));
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
         
         
        this.scriptableObject = scriptableObject;
        applySetting();
    }

    private Material createMaterial(string id)
    {
        PlayerBoundary playerBoundary = (PlayerBoundary)target;

        string[] guid = AssetDatabase.FindAssets("t:Material Unlit_CloseUpEffect");
        foreach (string g in guid)
        {
            string path = AssetDatabase.GUIDToAssetPath(g);
            Material material = AssetDatabase.LoadAssetAtPath<Material>(path);

            if (material != null && material.name == materialName)
            {
                OriginalMaterial = material;
                Debug.Log("Find Material: " + material.name + " " + path);
                break;
            }
        }

        if (OriginalMaterial == null) 
        { 
            Debug.LogError("Lost Material");
        }

        {
            Debug.Log("Create Material");
            string directoryPath = Path.GetDirectoryName(UnityEditor.AssetDatabase.GetAssetPath(
                MonoScript.FromMonoBehaviour(playerBoundary)));

            string materialFolderPath = Path.Combine(directoryPath, "Materials");
            if (!Directory.Exists(materialFolderPath)) 
                Directory.CreateDirectory(materialFolderPath);

            coppiedMaterial = new Material(OriginalMaterial);
            AssetDatabase.CreateAsset(coppiedMaterial, Path.Combine(materialFolderPath, materialName + "_" + id + ".mat"));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return coppiedMaterial;
        }
    }

    public void applySetting()
    {
        PlayerBoundary playerBoundary = (PlayerBoundary)target;
        playerBoundary.id = scriptableObject.id;
        playerBoundary.threshold = scriptableObject.threshold;
        playerBoundary.fadeRange = scriptableObject.fadeRange;
        playerBoundary.autoBoundaryMode = scriptableObject.autoBoundaryMode;
        playerBoundary.setMaterial(scriptableObject.material);
        playerBoundary.SetBoundaryType(scriptableObject.boundaryType);
        
        PrefabUtility.RecordPrefabInstancePropertyModifications(playerBoundary);
        EditorUtility.SetDirty(target);
        EditorUtility.SetDirty(scriptableObject);
        Debug.Log("세팅 적용 완료");
    }
}
#endif
