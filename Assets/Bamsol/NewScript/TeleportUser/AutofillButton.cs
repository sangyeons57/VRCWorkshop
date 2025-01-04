
using TMPro;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class AutofillButton : UdonSharpBehaviour
{
    [HideInInspector] public TMP_InputField inputField;
    
    public TextMeshProUGUI text;
    
    public string autofillText;

    public void Initialize(TMP_InputField inputField, string autofillText)
    {
        this.inputField = inputField;
        this.autofillText = autofillText;
        text.text = autofillText;
    }

    public void setNameInTextField()
    {
        inputField.text = autofillText;
    }
}
