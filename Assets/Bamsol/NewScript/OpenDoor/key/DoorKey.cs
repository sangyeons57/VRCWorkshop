
using System;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class DoorKey : UdonSharpBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        OpenDoor openDoor = other.gameObject.GetComponent<OpenDoor>();
        if (openDoor != null)
        {
            openDoor.unlockDoor();
            gameObject.SetActive(false);
        }
    }

}
