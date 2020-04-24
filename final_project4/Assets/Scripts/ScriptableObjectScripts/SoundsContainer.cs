using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Sounds/Sound Container", fileName = "new SoundsContaine")]
public class SoundsContainer : ScriptableObject
{
    public List<SoundLinksScriptableObjects> SoundLinksList;
}
