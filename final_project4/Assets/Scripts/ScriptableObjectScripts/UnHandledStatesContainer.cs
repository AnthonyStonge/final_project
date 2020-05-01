using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Type = Enums.Type;

[CreateAssetMenu(menuName = "ScriptableObjects/Animation/UnHandledStates Container", fileName = "new UnHandledStatesContainer")]
public class UnHandledStatesContainer : ScriptableObject
{
    [Serializable]
    public struct Links
    {
        public Type Type;
        public State State;
    }

    public List<Links> UnHandledStates;
}
