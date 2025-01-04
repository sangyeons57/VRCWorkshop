
using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using UdonSharp;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using VRC.SDKBase;
using Directory = System.IO.Directory;

public enum AutoBoundaryMode
{
    Off = 0,
    IS_VR,
    IS_DESKTOP,
}
public enum BoundaryType
{
    Include,
    Exclude
}
[RequireComponent(typeof(Collider))]
[UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
public class PlayerBoundary : UdonSharpBehaviour
{
    [HideInInspector] public string id = "";
    public GameObject[] RendererFace; 
    
    private Collider _collider;
    [HideInInspector]  public Material _material = null;
    [HideInInspector] [UdonSynced] public float threshold;
    [HideInInspector] [UdonSynced] public float fadeRange;

    
    [HideInInspector] [UdonSynced] public BoundaryType boundaryType;

    [HideInInspector] [UdonSynced] public int _autoBoundaryMode;
    
    public AutoBoundaryMode autoBoundaryMode
    {
        get => (AutoBoundaryMode)_autoBoundaryMode;
        set
        {
             if (Networking.IsOwner(gameObject))
             {
                 Networking.SetOwner(Networking.LocalPlayer, gameObject);
             }
             
             _autoBoundaryMode = (int)value;
             RequestSerialization();
        }
    } 

    private void Awake()
    {
    }

    private void Start()
    {
        CheckBoundaryMode();
    }

    public void setMaterial(Material material)
    {
        this._material = material;
        foreach (GameObject renderer in RendererFace)
        {
            renderer.GetComponent<Renderer>().material = material;
        }
    }
    public void CheckBoundaryMode()
    {
        switch (autoBoundaryMode)
        {
            case AutoBoundaryMode.Off:
                break;
            case AutoBoundaryMode.IS_VR:
                if (Networking.LocalPlayer.IsUserInVR())
                    SetBoundaryType(BoundaryType.Exclude);
                else 
                    SetBoundaryType(BoundaryType.Include);
                break;
            case AutoBoundaryMode.IS_DESKTOP:
                if (Networking.LocalPlayer.IsUserInVR())
                    SetBoundaryType(BoundaryType.Include);
                else 
                    SetBoundaryType(BoundaryType.Exclude);
                break;
        }
    }
    
    public void SetBoundaryType(BoundaryType boundaryType)
    {
        if (_collider == null && (_collider= GetComponent<Collider>()) == null)
        {
            return;
        }
        
        this.boundaryType = boundaryType;
        Debug.Log(_material);
        
        switch (boundaryType)
        {
            case BoundaryType.Include: 
                _material.SetFloat("_Threshold", 0f);
                _material.SetFloat("_FadeRange", 0f);
                _collider.enabled = false;
                break;
                
            case BoundaryType.Exclude:
                _material.SetFloat("_Threshold", threshold);
                _material.SetFloat("_FadeRange", fadeRange);
                _collider.enabled = true;
                break;
        }
    }
}

