using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerBoundarySetting", menuName = "Custom/PlayerBoundarySetting")]
[System.Serializable]
public class PlayerBoundaryScriptableObject : ScriptableObject
{
    public String id;
    public Material material;
    public AutoBoundaryMode autoBoundaryMode;
    public float threshold;
    public float fadeRange;
    public BoundaryType boundaryType;
}
