using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animation/Create Animation", fileName = "new Animation")]
public class Animation : ScriptableObject
{
    public enum AnimationType
    {
        PlayerRunning,
        EnemyWalking
    }

    public Animation.AnimationType Type;
    public Material Material;
    public List<Mesh> Frames;
}