using System.Collections;
using System.Collections.Generic;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Serialization;
[CreateAssetMenu(menuName = "Scriptables/SimpleEnemy")]
public class SimpleEnemyScriptableObject : ScriptableObject
{
    [Header("Transform")]
    public Translation position;
    public Rotation rotation;
    public Scale scale;
    public LocalToWorld localToWorld;

    [Header("Render")] 
    public RenderMesh renderMesh;

    [Header("Animation")] 
    public AnimationTestScriptableObject animationTestSequence;

    [Header("Physics")] 
    public Velocity velocity;
    public PhysicsCollider hitbox;
}
