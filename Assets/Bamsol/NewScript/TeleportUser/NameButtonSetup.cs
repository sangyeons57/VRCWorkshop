#if UNITY_EDITOR
using Cysharp.Threading.Tasks.Triggers;
using TMPro;
using UnityEditor;
using UnityEngine;

public class NameButtonSetup : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject parentObject;
    public GameObject nameButtonPrefab;
    public string[] names;


    public void BuildNameButton()
    {
        if (parentObject == null)
        {
            Debug.LogError("Parent object is null");
        }

        if (nameButtonPrefab == null)
        {
            Debug.LogError("Name button prefab is null");
        }

        for (int i = parentObject.transform.childCount - 1; i >= 0; i--)
        {
            GameObject obj = parentObject.transform.GetChild(i).gameObject;
            Debug.Log(obj.name);
            DestroyImmediate(obj);
        }
        
        foreach (string name in names)
        { 
            GameObject nameButton = Instantiate(nameButtonPrefab, parentObject.transform);
            nameButton.name += parentObject.transform.childCount;
            AutofillButton autofillButton = nameButton.GetComponent<AutofillButton>();
            autofillButton.Initialize(inputField, name);
        }
    }
}

[CustomEditor(typeof(NameButtonSetup))]
public class EditorNameButtonSetup : Editor
{
    private void OnEnable()
    {
    }
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        NameButtonSetup script = (NameButtonSetup)target;

        if (GUILayout.Button("build"))
        {
            script.BuildNameButton();
        }
    }
}
#endif