
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Serialization.OdinSerializer.Utilities;

public class InteractTest : UdonSharpBehaviour
{
    public string interactText;
    
    public override void Interact()
    {
        Debug.Log((interactText == null || interactText.Trim() == "") ? "Interact test": interactText);
    }

    public void ObjectInteract()
    {
        Debug.Log((interactText == null || interactText.Trim() == "") ? "Interact test": interactText);
    }

}
