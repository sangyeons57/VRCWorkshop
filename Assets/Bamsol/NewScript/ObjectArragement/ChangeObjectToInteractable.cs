
using System;
using System.IO;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ChangeObjectToInteractable : UdonSharpBehaviour
{
    public GameObject udonBehaviour;
    public string callMethodName;

    private void OnEnable()
    {
        Debug.Log(callMethodName);
    }

    public void Interact() 
    {
        Debug.Log("Interact and called " + callMethodName);
        if (udonBehaviour == null)
        {
            foreach (UdonSharpBehaviour element in transform.GetComponents<UdonSharpBehaviour>())
            {
                Debug.Log(element.name);
                element.SendCustomEvent(callMethodName);
            }
        }
        else
        {
            foreach (UdonSharpBehaviour element in udonBehaviour.transform.gameObject.GetComponents<UdonSharpBehaviour>())
            {
                Debug.Log(element.name);
                element.SendCustomEvent(callMethodName);
            }
        }
    }
}