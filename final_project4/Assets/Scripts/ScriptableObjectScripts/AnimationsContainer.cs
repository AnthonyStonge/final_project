using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animation/Animation Container", fileName = "new AnimationContainer")]
public class AnimationsContainer : ScriptableObject
{
    public List<Animation> Animations;
}
