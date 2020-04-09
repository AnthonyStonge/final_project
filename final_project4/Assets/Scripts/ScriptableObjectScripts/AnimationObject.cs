using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptables/Animation/Create Animation")]
public class AnimationObject : ScriptableObject
{
    public List<AnimationFrames> Animations;
}

//Used as a wrapper to create List of List in Inspector
[System.Serializable]
public class AnimationFrames
{
    public AnimationType Type;
    public List<GameObject> Frames;
}

public enum AnimationType
{
    Idle, Run
}