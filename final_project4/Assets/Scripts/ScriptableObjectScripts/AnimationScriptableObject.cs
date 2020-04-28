using System.Collections.Generic;
using Enums;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animation/Create Animation", fileName = "new Animation")]
public class AnimationScriptableObject : ScriptableObject
{
    [Tooltip("Type of Object animation goes on")]
    public Type Type;
    [Tooltip("The state required to play this animation")]
    public StateActions State;
    
    [Tooltip("The material for the Object")]
    public Material Material;
    public List<Mesh> Frames;
}