using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "ScriptableObjects/Effects/Effects Container")]
public class EffectsContainer : ScriptableObject
{
    public List<EffectLinks> Links;
}
