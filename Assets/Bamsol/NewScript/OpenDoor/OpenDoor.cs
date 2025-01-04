
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class OpenDoor : UdonSharpBehaviour
{
    public Animator animator;
    public Collider collider; 
    
    [UdonSynced(UdonSyncMode.None)] public bool isLocked = false;

    public AudioClip openSound;
    public AudioClip closedSound;
    public AudioClip unlockSound;

    private AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void openDoor()
    {
        if (!isLocked)
        { 
            SendCustomNetworkEvent(NetworkEventTarget.All, "_openDoor");
        }
    }

    public void _openDoor()
    { 
        if (!isLocked)
        {
            animator.SetTrigger("OpenDoor");
            collider.enabled = false;
            audioSource.clip = openSound;
            audioSource.Play();
        }
        else
        {
            audioSource.clip = closedSound;
            audioSource.Play();
        }
    }

    public override void Interact()
    {
        SendCustomNetworkEvent(VRC.Udon.Common.Interfaces.NetworkEventTarget.All, "openDoor");
    }

    public void unlockDoor()
    {
        isLocked = false;
        
        if(!Networking.IsOwner(Networking.LocalPlayer, gameObject))
            Networking.SetOwner(Networking.LocalPlayer, gameObject);
        
        audioSource.clip = unlockSound;
        audioSource.Play();
        
        RequestSerialization();
    }
}
