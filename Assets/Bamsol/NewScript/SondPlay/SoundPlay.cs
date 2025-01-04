
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
[RequireComponent(typeof(AudioSource))]
public class SoundPlay : UdonSharpBehaviour
{

    private AudioSource audioSource;
    
    public void Play()
    {
        if ((audioSource = GetComponent<AudioSource>()) != null)
        { 
            if (!Networking.IsOwner(gameObject))
            {
                Networking.SetOwner(Networking.LocalPlayer, gameObject);
            }
            
            SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "PlaySound");
            Debug.Log("Sound Play");
        }
        else
        {
            Debug.Log("Sound is not exist");
        }
        
    }

    public void PlaySound()
    {
        if ((audioSource = GetComponent<AudioSource>()) != null)
        { 
            audioSource.Play();
        }
    }
}
