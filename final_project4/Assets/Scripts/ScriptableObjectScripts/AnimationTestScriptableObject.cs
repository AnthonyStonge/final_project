using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = ( "Scriptables/Animation"))]
public class AnimationTestScriptableObject : ScriptableObject
{
    public Mesh[] meshes;
}
