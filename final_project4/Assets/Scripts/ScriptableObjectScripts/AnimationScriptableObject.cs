using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(menuName = ( "Scriptables/Animation"))]
public class AnimationScriptableObject : ScriptableObject
{
    public Mesh[] meshes;
}
