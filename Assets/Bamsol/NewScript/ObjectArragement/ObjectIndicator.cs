
using UdonSharp;
using UnityEngine;
using VRC.SDK3.Components;
using VRC.SDKBase;
using VRC.Udon;

[RequireComponent(typeof(VRCObjectSync))]
[RequireComponent(typeof(VRC_Pickup))]
[UdonBehaviourSyncMode(BehaviourSyncMode.Continuous)]
public class ObjectIndicator : UdonSharpBehaviour
{
    public string id;
    public Vector3 fixRotation;
    [HideInInspector]
    public VRC_Pickup pickup;
    [HideInInspector]
    public bool isFixed;
    [HideInInspector]
    public ObjectArrangement fixedArrangement;
    
    private Material material;
    private Rigidbody rb; 
    private AudioSource audioSource; 
    void Start()
    {
        pickup = gameObject.GetComponent<VRC_Pickup>();
        rb = GetComponent<Rigidbody>();
        material = gameObject.GetComponent<Renderer>().material;
        audioSource = GetComponent<AudioSource>();
    }
    public override void OnPickup()
    {
        if (fixedArrangement != null)
        {
            fixedArrangement.clearFixedObject();
            fixedArrangement = null;
        }
        
        isFixed = false;
        rb.isKinematic = false; 
        material.SetColor("_EmissionColor", material.color * 1);
        audioSource.Play();
    }

    public override void OnDrop()
    {
        base.OnDrop();
    }

    public void fixObject(ObjectArrangement arrangement)
    {
        fixedArrangement = arrangement;
        isFixed = true;
        rb.isKinematic = true;
        transform.rotation = Quaternion.Euler(fixRotation);
        
        if ( arrangement.checkId())
            material.SetColor("_EmissionColor", material.color * 10);
        else 
            material.SetColor("_EmissionColor", material.color * 1);
    }
}
