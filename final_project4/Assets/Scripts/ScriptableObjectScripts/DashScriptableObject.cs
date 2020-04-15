using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dash", fileName = "new Dash")]
public class DashScriptableObject : ScriptableObject
{
    [Header("Variables")]
    public float DefaultLength;
    public float DefaultResetTime;

    [Header("Internal Variables")]
    public AudioClip DashSound;

    //TODO ADD ANIMATION BEHIND DASH?
}
