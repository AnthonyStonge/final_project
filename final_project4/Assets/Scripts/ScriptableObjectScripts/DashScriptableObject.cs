using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Dash", fileName = "new Dash")]
public class DashScriptableObject : ScriptableObject
{
    [Header("Internal Variables")]
    public float DefaultLength;
    public float DefaultResetTime;
    
    //TODO ADD ANIMATION BEHIND DASH?
}
