using UdonSharp;
using UnityEngine;

[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class ChangeIneractableOnAllObjectArranged : UdonSharpBehaviour
{
    private BoxCollider interactable;

    private Material material;
    
    private AudioSource audioSource;

    public TeleportTo teleportTo;
    public FullScreen fullScreen;
    private void Start()
    {
        interactable = GetComponent<BoxCollider>();
        interactable.enabled = false;
        material = gameObject.GetComponent<MeshRenderer>().material;
        material.SetColor("_EmissionColor", material.color * 1);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
    }


    public void onAllObjectArranged()
    {
        interactable.enabled = true;
        material.SetColor("_EmissionColor", material.color * 40);
        
        audioSource.Play();
    }

    public void Interact()
    {
        Debug.Log("Interact");
        if (teleportTo != null && fullScreen != null)
        {
            audioSource.Stop();
            teleportTo.Interact();
            fullScreen.Interact();
        }
    }
}
