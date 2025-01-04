
using UdonSharp;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using VRC.SDKBase;

public class TeleportTo : UdonSharpBehaviour
{

    //public WakeUp [] wakeup;
    
    public PostProcessVolume postProcessVolume;
    public float speed;
    public Transform teleportPoint;

    private float progress;
    private bool isWakeup;


    private void Update()
    {
        if (isWakeup)
        {
            progress += Time.deltaTime * speed; 
            
            postProcessVolume.weight = 1 - progress;
            
            if (progress >= 1)
                EndWakeup();
        }
    }

    public void Interact() {
        /*
        foreach(WakeUp w in wakeup) {
            w.wakeup();
        }
        */ 
        StartWakeup();
        
        Networking.LocalPlayer.TeleportTo(teleportPoint.position, teleportPoint.rotation);
    }


    public void StartWakeup()
    {
        if (postProcessVolume == null)
            return;

        isWakeup = true;
        progress = 0;
        postProcessVolume.enabled = true;
    }
    public void EndWakeup() 
    {
        isWakeup = false;
        postProcessVolume.enabled = false;
    }
}
