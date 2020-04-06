using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Player")]
public class PlayerAssetsScriptableObject : ScriptableObject
{
    [Header("Transform")]
    public Translation position;
    public Rotation rotation;
    public Scale scale;
    public LocalToWorld localToWorld;
    
    [Header("Inputs")] 
    public InputsComponent inputs;
    
    [Header("Animation")] 
    public RenderMesh renderMesh;
    public AnimationScriptableObject animationSequence;
    
    [Header("Physics")] 
    public Velocity velocity;
    public PhysicsCollider hitbox;
}
